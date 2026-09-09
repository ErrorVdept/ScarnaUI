// NorthUI — скользящий индикатор (pill) для NorthTabs.
// Активную кнопку находит сам по data-tab-active="true" — компоненту не нужно
// прокидывать в JS ElementReference на каждую кнопку по отдельности.

export function syncIndicator(trackEl) {
    if (!trackEl) return;

    const active = trackEl.querySelector('[data-tab-active="true"]');
    const indicator = trackEl.querySelector(".north-tabs__indicator");
    if (!active || !indicator) return;

    indicator.style.width = `${active.offsetWidth}px`;
    indicator.style.transform = `translateX(${active.offsetLeft}px)`;
}
