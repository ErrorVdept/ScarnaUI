// Слайдер сам не может "запретить" перетаскивание нативного input[type=range] (readonly
// на range браузеры игнорируют) — оно просто произошло в DOM ещё до всякого oninput.
// Раз Blazor считает, что Value не менялся, он не станет сам возвращать value="..."
// обратно (в его дереве рендера оно и так уже то же самое) — приходится форсировать
// это явно из C# (см. HandleInputAsync в ScarnaSlider.razor, readonly-ветка).
export function resetValue(input, value) {
    input.value = value;
}
