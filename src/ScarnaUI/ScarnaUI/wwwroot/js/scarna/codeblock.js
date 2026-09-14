// Кнопка копирования у ScarnaCodeBlock — navigator.clipboard недоступен из C# напрямую,
// только через JS interop, отсюда этот крошечный модуль.
export async function copyText(text) {
    await navigator.clipboard.writeText(text);
}
