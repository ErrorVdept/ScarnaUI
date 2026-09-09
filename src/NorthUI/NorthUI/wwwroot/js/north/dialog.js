// NorthUI — вспомогательные JS-функции для модальных диалогов:
// фокус-ловушка (focus trap), закрытие по Escape, блокировка скролла страницы.

const state = new Map(); // dialogId -> { cleanup, previouslyFocused }

const FOCUSABLE_SELECTOR = [
    "a[href]", "button:not([disabled])", "textarea:not([disabled])",
    "input:not([disabled])", "select:not([disabled])", "[tabindex]:not([tabindex='-1'])",
].join(",");

let lockCount = 0;
let previousBodyOverflow = "";

function lockScroll() {
    if (lockCount === 0) {
        previousBodyOverflow = document.body.style.overflow;
        document.body.style.overflow = "hidden";
    }
    lockCount++;
}

function unlockScroll() {
    lockCount = Math.max(0, lockCount - 1);
    if (lockCount === 0) {
        document.body.style.overflow = previousBodyOverflow;
    }
}

/**
 * Открывает диалог: блокирует скролл, ставит фокус внутрь, включает focus trap и Escape.
 * @param {string} dialogId
 * @param {HTMLElement} dialogEl - корневой элемент содержимого диалога (панель)
 * @param {import("@microsoft/dotnet-js-interop").DotNetObject} dotNetRef
 */
export function open(dialogId, dialogEl, dotNetRef) {
    close(dialogId);
    lockScroll();

    const previouslyFocused = document.activeElement;

    const focusFirst = () => {
        const focusable = dialogEl.querySelectorAll(FOCUSABLE_SELECTOR);
        if (focusable.length > 0) {
            focusable[0].focus();
        } else {
            dialogEl.setAttribute("tabindex", "-1");
            dialogEl.focus();
        }
    };
    // небольшая задержка нужна, чтобы дать браузеру отрисовать открывшийся диалог
    setTimeout(focusFirst, 0);

    const onKeyDown = (event) => {
        if (event.key === "Escape") {
            dotNetRef.invokeMethodAsync("OnEscapeAsync");
            return;
        }

        if (event.key !== "Tab") {
            return;
        }

        const focusable = Array.from(dialogEl.querySelectorAll(FOCUSABLE_SELECTOR))
            .filter((el) => el.offsetParent !== null);
        if (focusable.length === 0) {
            return;
        }

        const first = focusable[0];
        const last = focusable[focusable.length - 1];

        if (event.shiftKey && document.activeElement === first) {
            event.preventDefault();
            last.focus();
        } else if (!event.shiftKey && document.activeElement === last) {
            event.preventDefault();
            first.focus();
        }
    };

    document.addEventListener("keydown", onKeyDown, true);

    state.set(dialogId, {
        previouslyFocused,
        cleanup: () => document.removeEventListener("keydown", onKeyDown, true),
    });
}

export function close(dialogId) {
    const entry = state.get(dialogId);
    if (!entry) {
        return;
    }

    entry.cleanup();
    unlockScroll();

    if (entry.previouslyFocused && typeof entry.previouslyFocused.focus === "function") {
        entry.previouslyFocused.focus();
    }

    state.delete(dialogId);
}
