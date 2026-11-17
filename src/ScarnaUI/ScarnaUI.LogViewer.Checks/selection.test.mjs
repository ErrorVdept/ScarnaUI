import assert from "node:assert/strict";
import { initialize, observe, dispose, copySelection, setSelectedCount, setSelectedText } from "../ScarnaUI/wwwroot/js/scarna/logviewer.js";

class ElementMock {
    constructor() { this.handlers = {}; this.dataset = {}; this.children = []; }
    addEventListener(name, fn) { this.handlers[name] = fn; }
    removeEventListener(name) { delete this.handlers[name]; }
    appendChild(child) { this.children.push(child); }
    closest(selector) { return this.matches?.(selector) ? this : null; }
    contains(node) { return node === this || this.children.includes(node); }
    querySelectorAll() { return []; }
    querySelector() { return null; }
}
globalThis.Element = ElementMock;
globalThis.Node = { ELEMENT_NODE: 1 };
const documentHandlers = {};
const root = new ElementMock();
const viewport = new ElementMock();
root.children.push(viewport);
viewport.focus = () => {};
viewport.clientHeight = 240;
viewport.scrollTop = 0;
let elements = [];
viewport.querySelectorAll = selector => selector === ".scarna-logviewer__text" ? elements : [];
viewport.contains = element => elements.includes(element);
const makeText = (number, text) => {
    const line = new ElementMock(); line.dataset.logNumber = String(number);
    const element = new ElementMock(); element.textContent = text; element.matches = selector => selector === ".scarna-logviewer__text";
    element.closest = selector => selector === "[data-log-number]" ? line : element.matches(selector) ? element : null;
    const node = { nodeType: 3, parentElement: element };
    return { element, node };
};
const first = makeText(1, "alpha");
const second = makeText(2, "beta");
elements = [first.element, second.element];
let selection = { isCollapsed: false, rangeCount: 1, getRangeAt: () => ({ startContainer: first.node, startOffset: 1, endContainer: second.node, endOffset: 2, intersectsNode: element => elements.includes(element) }) };
globalThis.window = { getSelection: () => selection };
globalThis.document = {
    activeElement: viewport,
    addEventListener: (name, fn) => { documentHandlers[name] = fn; },
    removeEventListener: name => { delete documentHandlers[name]; },
    createElement: () => ({ remove() {} }),
    createRange: () => ({ selectNodeContents(element) { this.element = element; }, setEnd(node, offset) { this.offset = offset; }, toString() { return this.element.textContent.slice(0, this.offset); } })
};
globalThis.ResizeObserver = class { observe() {} disconnect() {} };
globalThis.requestAnimationFrame = () => 1;
globalThis.cancelAnimationFrame = () => {};
let copied;
Object.defineProperty(globalThis, "navigator", { configurable: true, value: { clipboard: { writeText: async value => { copied = value; } } } });
const calls = [];
const dotnet = { invokeMethodAsync: async method => { calls.push(method); return method === "GetSelectedLinesAsync" ? "line 1\nline 100" : undefined; } };

initialize(viewport, root, dotnet);
let findPrevented = false;
documentHandlers.keydown({ target: viewport, ctrlKey: true, metaKey: false, altKey: false, shiftKey: false, code: "KeyF", key: "а", preventDefault() { findPrevented = true; }, stopPropagation() {} });
assert.equal(findPrevented, true, "Ctrl+F must work with a non-Latin keyboard layout");
assert.ok(calls.includes("OpenSearchAsync"));
const unrelatedInput = new ElementMock(); unrelatedInput.matches = selector => selector.startsWith("input,");
findPrevented = false;
documentHandlers.keydown({ target: unrelatedInput, ctrlKey: true, metaKey: false, altKey: false, shiftKey: false, code: "KeyF", key: "f", preventDefault() { findPrevented = true; }, stopPropagation() {} });
assert.equal(findPrevented, false, "an unrelated input must keep its own Ctrl+F behavior");
documentHandlers.selectionchange();
elements = [];
selection = { isCollapsed: true, rangeCount: 0 };
documentHandlers.selectionchange();
observe(viewport);
const clipboard = { setData(type, value) { copied = value; } };
let prevented = false;
root.handlers.copy({ target: viewport, clipboardData: clipboard, preventDefault() { prevented = true; } });
assert.equal(prevented, true);
assert.equal(copied, "lpha\nbe", "text selection must survive removal of rendered rows");
copied = null;
await copySelection(viewport);
assert.equal(copied, "lpha\nbe", "context menu must copy stored text");

const marker = new ElementMock(); marker.matches = selector => selector === ".scarna-logviewer__marker";
selection.removeAllRanges = () => {};
viewport.handlers.pointerdown({ button: 0, target: marker });
setSelectedCount(viewport, 2, 1);
setSelectedText(viewport, "line 1\nline 100", 1);
copied = null;
root.handlers.copy({ target: viewport, clipboardData: clipboard, preventDefault() {} });
assert.equal(copied, "line 1\nline 100", "marked lines must be copied synchronously from the clipboard event");
setSelectedCount(viewport, 2, 2);
copied = null;
root.handlers.copy({ target: viewport, clipboardData: clipboard, preventDefault() {} });
await new Promise(resolve => setTimeout(resolve, 0));
assert.equal(copied, "line 1\nline 100", "copying marked lines must request data outside the viewport");

dispose(viewport);
assert.equal(documentHandlers.selectionchange, undefined);
assert.equal(documentHandlers.keydown, undefined);
console.log("Selection and copy checks passed.");
