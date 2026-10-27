// ScarnaUI — модуль применения темы в рантайме.
// Загружается один раз через IJSRuntime.InvokeAsync<IJSObjectReference>("import", ...)
// из ScarnaThemeProvider. Никаких внешних зависимостей.

const STORAGE_KEY = "scarna-theme-mode"; // "light" | "dark" | "system"

/**
 * Применяет набор CSS custom properties на <html> и переключает data-theme.
 * @param {Record<string,string>} cssVars - например { "--scarna-primary": "212 100% 47%" }
 * @param {boolean} isDark
 */
export function applyTheme(cssVars, isDark) {
    const root = document.documentElement;
    for (const [name, value] of Object.entries(cssVars)) {
        root.style.setProperty(name, value);
    }
    root.setAttribute("data-theme", isDark ? "dark" : "light");
    root.style.colorScheme = isDark ? "dark" : "light";
}

export function getStoredMode() {
    try {
        return localStorage.getItem(STORAGE_KEY);
    } catch {
        return null;
    }
}

export function setStoredMode(mode) {
    try {
        localStorage.setItem(STORAGE_KEY, mode);
    } catch {
        // приватный режим браузера / localStorage недоступен — молча игнорируем
    }
}

export function systemPrefersDark() {
    return window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches;
}

let unwatchCurrent = null;

/**
 * Подписывается на смену системной темы ОС и вызывает .NET-колбэк,
 * пока активен режим Mode.System. Отписка — unwatchSystemTheme() (функцию из JS в .NET
 * вернуть нельзя — interop сериализует её в null, поэтому раньше подписка не снималась
 * никогда и после Dispose провайдера вызывала уже освобождённый DotNetObjectReference).
 * @param {import("@microsoft/dotnet-js-interop").DotNetObject} dotNetRef
 */
export function watchSystemTheme(dotNetRef) {
    unwatchSystemTheme();

    if (!window.matchMedia) {
        return;
    }

    const media = window.matchMedia("(prefers-color-scheme: dark)");
    const listener = (event) => {
        dotNetRef.invokeMethodAsync("OnSystemPreferenceChangedAsync", event.matches);
    };

    media.addEventListener("change", listener);
    unwatchCurrent = () => media.removeEventListener("change", listener);
}

export function unwatchSystemTheme() {
    unwatchCurrent?.();
    unwatchCurrent = null;
}
