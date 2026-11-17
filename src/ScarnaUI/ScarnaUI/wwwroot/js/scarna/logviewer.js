const states = new WeakMap();
const baseHeight = 24;
const overscan = 12;
let nextHighlightId = 0;
const viewers = new Set();
let activeViewer = null;

function offsetWithin(element, node, offset) {
    const range = document.createRange();
    range.selectNodeContents(element);
    range.setEnd(node, offset);
    return range.toString().length;
}

function captureSelection(state) {
    const selection = window.getSelection();
    if (!selection || selection.isCollapsed || selection.rangeCount === 0) return;
    const range = selection.getRangeAt(0);
    const startElement = (range.startContainer.nodeType === Node.ELEMENT_NODE ? range.startContainer : range.startContainer.parentElement)?.closest(".scarna-logviewer__text");
    const endElement = (range.endContainer.nodeType === Node.ELEMENT_NODE ? range.endContainer : range.endContainer.parentElement)?.closest(".scarna-logviewer__text");
    // При удалении виртуализированной строки браузер переносит границу в контейнер.
    // Такое усечённое выделение не должно перезаписать сохранённое пользователем.
    if (!startElement || !endElement || !state.viewport.contains(startElement) || !state.viewport.contains(endElement)) return;
    const segments = [];
    for (const element of state.viewport.querySelectorAll(".scarna-logviewer__text")) {
        if (!range.intersectsNode(element)) continue;
        const length = element.textContent.length;
        const start = element === startElement ? offsetWithin(element, range.startContainer, range.startOffset) : 0;
        const end = element === endElement ? offsetWithin(element, range.endContainer, range.endOffset) : length;
        if (start < end || (length === 0 && element !== startElement && element !== endElement))
            segments.push({ number: Number(element.closest("[data-log-number]").dataset.logNumber), start, end, text: element.textContent.slice(start, end) });
    }
    if (segments.length) {
        state.selection = segments;
        state.selectionText = segments.map(segment => segment.text).join("\n");
        updateHighlights(state);
    }
}

function textPosition(element, offset) {
    const walker = document.createTreeWalker(element, NodeFilter.SHOW_TEXT);
    let node;
    while ((node = walker.nextNode())) {
        if (offset <= node.length) return [node, offset];
        offset -= node.length;
    }
    return [element, element.childNodes.length];
}

function updateHighlights(state) {
    if (!globalThis.CSS?.highlights || !globalThis.Highlight) return;
    const ranges = [];
    for (const segment of state.selection) {
        const element = state.viewport.querySelector(`[data-log-number="${segment.number}"] .scarna-logviewer__text`);
        if (!element) continue;
        const [startNode, startOffset] = textPosition(element, segment.start);
        const [endNode, endOffset] = textPosition(element, segment.end);
        const range = document.createRange();
        range.setStart(startNode, startOffset);
        range.setEnd(endNode, endOffset);
        ranges.push(range);
    }
    CSS.highlights.set(state.highlightName, new Highlight(...ranges));
}

function clearSelection(state) {
    state.selection = [];
    state.selectionText = "";
    if (globalThis.CSS?.highlights) CSS.highlights.delete(state.highlightName);
}

function prefix(state, count) {
    let sum = count * baseHeight;
    for (let i = count; i > 0; i -= i & -i) sum += state.tree[i];
    return sum;
}

function lineAt(state, offset) {
    let low = 0, high = state.count;
    while (low < high) {
        const middle = (low + high + 1) >>> 1;
        if (prefix(state, middle) <= offset) low = middle;
        else high = middle - 1;
    }
    return Math.min(low, Math.max(0, state.count - 1));
}

function measure(state, element) {
    const index = Number(element.dataset.logIndex);
    if (!Number.isInteger(index) || index < 0 || index >= state.count) return;
    const height = element.getBoundingClientRect().height;
    if (!height) return;
    const old = state.heights[index] || baseHeight;
    const delta = height - old;
    if (Math.abs(delta) < 0.1) return;
    const topLine = lineAt(state, state.viewport.scrollTop);
    state.heights[index] = height;
    for (let i = index + 1; i <= state.count; i += i & -i) state.tree[i] += delta;
    if (index < topLine) state.viewport.scrollTop += delta;
}

function restoreBottom(state) {
    if (state.bottomPinned)
        state.viewport.scrollTop = Math.max(0, state.viewport.scrollHeight - state.viewport.clientHeight);
}

function schedule(state) {
    if (state.frame) return;
    state.frame = requestAnimationFrame(() => {
        state.frame = 0;
        if (!state.count) return;
        const start = Math.max(0, lineAt(state, state.viewport.scrollTop) - overscan);
        const end = Math.min(state.count, lineAt(state, state.viewport.scrollTop + state.viewport.clientHeight) + overscan + 1);
        if (start === state.start && end === state.end) return;
        state.start = start; state.end = end;
        state.dotnet.invokeMethodAsync("SetViewportAsync", start, end - start).catch(() => {});
    });
}

export function initialize(viewport, root, dotnet) {
    if (states.has(viewport)) return;
    const state = { viewport, root, dotnet, count: 0, tree: new Float64Array(1), heights: new Float64Array(0), start: -1, end: -1, frame: 0, selection: [], selectionText: "", selectedCount: 0, selectedText: null, selectionVersion: 0, highlightName: `scarna-log-selection-${++nextHighlightId}` };
    viewers.add(state);
    if (!activeViewer) activeViewer = state;
    state.highlightStyle = document.createElement("style");
    state.highlightStyle.textContent = `::highlight(${state.highlightName}) { background-color: #2768b7; color: white; }`;
    root.appendChild(state.highlightStyle);
    state.onSelectionChange = () => captureSelection(state);
    document.addEventListener("selectionchange", state.onSelectionChange);
    state.onCopy = event => {
        if (event.target instanceof Element && event.target.closest("input, textarea, [contenteditable=true]")) return;
        if (state.selectionText && event.clipboardData) {
            event.preventDefault();
            event.clipboardData.setData("text/plain", state.selectionText);
        } else if (state.selectedCount && state.selectedText !== null && event.clipboardData) {
            event.preventDefault();
            event.clipboardData.setData("text/plain", state.selectedText);
        } else if (state.selectedCount) {
            event.preventDefault();
            const content = dotnet.invokeMethodAsync("GetSelectedLinesAsync");
            if (globalThis.ClipboardItem && navigator.clipboard?.write) {
                const blob = content.then(text => new Blob([text ?? ""], { type: "text/plain" }));
                navigator.clipboard.write([new ClipboardItem({ "text/plain": blob })]).catch(() => {});
            } else {
                content.then(text => navigator.clipboard.writeText(text ?? "")).catch(() => {});
            }
        }
    };
    root.addEventListener("copy", state.onCopy);
    state.onKeyDown = event => {
        if (activeViewer !== state) return;
        const target = event.target;
        if (target instanceof Element && !root.contains(target) && target.closest("input, textarea, select, [contenteditable]:not([contenteditable=false])")) return;
        if ((event.ctrlKey || event.metaKey) && !event.altKey && !event.shiftKey && (event.code === "KeyF" || event.key?.toLowerCase() === "f")) {
            event.preventDefault();
            event.stopPropagation();
            dotnet.invokeMethodAsync("OpenSearchAsync").catch(() => {});
        }
    };
    state.onActivate = () => { activeViewer = state; };
    state.onPointerDown = event => {
        if (event.button === 0 && (event.target.closest(".scarna-logviewer__text") || event.target.closest(".scarna-logviewer__marker"))) {
            clearSelection(state);
            if (event.target.closest(".scarna-logviewer__marker")) { state.selectedText = null; window.getSelection()?.removeAllRanges(); }
        }
        if (event.target.closest(".scarna-logviewer__text") && !root.contains(document.activeElement))
            viewport.focus({ preventScroll: true });
    };
    document.addEventListener("keydown", state.onKeyDown, true);
    root.addEventListener("focusin", state.onActivate);
    root.addEventListener("pointerdown", state.onActivate);
    viewport.addEventListener("pointerdown", state.onPointerDown);
    state.onScroll = () => {
        const remaining = viewport.scrollHeight - viewport.clientHeight - viewport.scrollTop;
        state.bottomPinned = viewport.scrollHeight > viewport.clientHeight && remaining <= 2;
        schedule(state);
    };
    state.resize = new ResizeObserver(entries => {
        for (const entry of entries) {
            if (entry.target === viewport) {
                const width = entry.contentRect.width;
                if (state.width !== undefined && Math.abs(state.width - width) > 0.5) {
                    state.tree = new Float64Array(state.count + 1);
                    state.heights = new Float64Array(state.count);
                    state.start = -1; state.end = -1;
                }
                state.width = width;
            } else measure(state, entry.target);
        }
        updateSpacers(state);
        restoreBottom(state);
        schedule(state);
    });
    state.resize.observe(viewport);
    viewport.addEventListener("scroll", state.onScroll, { passive: true });
    states.set(viewport, state);
}

export function reset(viewport, count) {
    const state = states.get(viewport);
    if (!state) return;
    state.count = count;
    state.bottomPinned = false;
    state.tree = new Float64Array(count + 1);
    state.heights = new Float64Array(count);
    state.start = -1; state.end = -1;
    const before = viewport.querySelector('[data-log-spacer="before"]');
    const after = viewport.querySelector('[data-log-spacer="after"]');
    if (before) before.style.height = "0px";
    if (after) after.style.height = `${count * baseHeight}px`;
    schedule(state);
}

function updateSpacers(state) {
    const rows = state.viewport.querySelectorAll("[data-log-index]");
    const before = state.viewport.querySelector('[data-log-spacer="before"]');
    const after = state.viewport.querySelector('[data-log-spacer="after"]');
    if (!before || !after || !rows.length) return;
    const first = Number(rows[0].dataset.logIndex);
    const last = Number(rows[rows.length - 1].dataset.logIndex) + 1;
    before.style.height = `${prefix(state, first)}px`;
    after.style.height = `${Math.max(0, prefix(state, state.count) - prefix(state, last))}px`;
}

export function observe(viewport) {
    const state = states.get(viewport);
    if (!state) return;
    state.resize.disconnect();
    state.resize.observe(viewport);
    for (const row of viewport.querySelectorAll("[data-log-index]")) {
        measure(state, row);
        state.resize.observe(row);
    }
    updateSpacers(state);
    restoreBottom(state);
    updateHighlights(state);
    if (state.targetIndex !== undefined && viewport.querySelector(`[data-log-index="${state.targetIndex}"]`)) {
        viewport.scrollTop = prefix(state, state.targetIndex);
        delete state.targetIndex;
    }
    schedule(state);
}

export function goTo(viewport, index) {
    const state = states.get(viewport);
    if (!state) return;
    state.bottomPinned = false;
    state.targetIndex = index;
    viewport.scrollTop = prefix(state, index);
    schedule(state);
}

export function dispose(viewport) {
    const state = states.get(viewport);
    if (!state) return;
    if (state.frame) cancelAnimationFrame(state.frame);
    state.resize.disconnect();
    document.removeEventListener("selectionchange", state.onSelectionChange);
    state.root.removeEventListener("copy", state.onCopy);
    clearSelection(state);
    state.highlightStyle.remove();
    document.removeEventListener("keydown", state.onKeyDown, true);
    state.root.removeEventListener("focusin", state.onActivate);
    state.root.removeEventListener("pointerdown", state.onActivate);
    viewport.removeEventListener("pointerdown", state.onPointerDown);
    viewport.removeEventListener("scroll", state.onScroll);
    states.delete(viewport);
    viewers.delete(state);
    if (activeViewer === state) activeViewer = viewers.values().next().value ?? null;
}

export async function copy(text) { await navigator.clipboard.writeText(text); }
export function clearSavedSelection(viewport) { const state = states.get(viewport); if (state) { clearSelection(state); state.selectedText = null; state.selectedCount = 0; } }
export function setSelectedCount(viewport, count, version) { const state = states.get(viewport); if (state) { if (state.selectionVersion !== version) state.selectedText = null; state.selectionVersion = version; state.selectedCount = count; } }
export function setSelectedText(viewport, text, version) { const state = states.get(viewport); if (state && version >= state.selectionVersion) { state.selectionVersion = version; state.selectedText = text; } }
export async function copySelection(viewport) { await navigator.clipboard.writeText(states.get(viewport)?.selectionText ?? ""); }
