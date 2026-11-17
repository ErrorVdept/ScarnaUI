import assert from "node:assert/strict";
import { initialize, reset, observe, dispose } from "../ScarnaUI/wwwroot/js/scarna/logviewer.js";

class ElementMock {
    constructor() { this.handlers = {}; this.dataset = {}; this.style = {}; }
    addEventListener(name, handler) { this.handlers[name] = handler; }
    removeEventListener(name) { delete this.handlers[name]; }
    appendChild() {}
    contains() { return false; }
}
globalThis.Element = ElementMock;
globalThis.document = { addEventListener() {}, removeEventListener() {}, createElement: () => ({ remove() {} }) };
globalThis.ResizeObserver = class { observe() {} disconnect() {} };
globalThis.requestAnimationFrame = () => 1;
globalThis.cancelAnimationFrame = () => {};
const root = new ElementMock();
const viewport = new ElementMock();
const before = { style: { height: "0px" } };
const after = { style: { height: "0px" } };
const rows = Array.from({ length: 5 }, (_, i) => ({ dataset: { logIndex: String(95 + i) }, height: 24, getBoundingClientRect() { return { height: this.height }; } }));
viewport.clientHeight = 240;
viewport.scrollTop = 0;
viewport.querySelectorAll = selector => selector === "[data-log-index]" ? rows : [];
viewport.querySelector = selector => selector.includes('"before"') ? before : selector.includes('"after"') ? after : null;
Object.defineProperty(viewport, "scrollHeight", { get: () => parseFloat(before.style.height) + rows.reduce((sum, row) => sum + row.height, 0) + parseFloat(after.style.height) });

initialize(viewport, root, { invokeMethodAsync: async () => {} });
reset(viewport, 100);
before.style.height = "2280px";
after.style.height = "0px";
viewport.scrollTop = viewport.scrollHeight - viewport.clientHeight;
viewport.handlers.scroll();
for (const row of rows) row.height = 48;
observe(viewport);
assert.equal(viewport.scrollTop, viewport.scrollHeight - viewport.clientHeight, "the bottom must remain visible after wrapped rows grow");

viewport.scrollTop -= 120;
viewport.handlers.scroll();
rows[4].height = 72;
observe(viewport);
assert.ok(viewport.scrollTop < viewport.scrollHeight - viewport.clientHeight, "scrolling upward must release the bottom anchor");
dispose(viewport);
console.log("Virtualized bottom scroll checks passed.");
