// ScarnaUI — позиционирование поповеров (Select, Menu, Tooltip, DatePicker и т.д.).
// Простая самостоятельная реализация flip/shift без внешних зависимостей
// (аналог того, что делает @floating-ui/dom, но в разы проще — ровно под наши нужды).

const registry = new Map(); // popoverId -> { anchor, popover, cleanup[] }

/**
 * @typedef {"top"|"bottom"|"left"|"right"} Side
 * @typedef {"start"|"center"|"end"} Align
 */

const OPPOSITE = { top: "bottom", bottom: "top", left: "right", right: "left" };

/**
 * Вычисляет координаты поповера относительно якоря с учётом границ окна
 * (flip — переворот на противоположную сторону, если не помещается;
 *  shift — сдвиг вдоль оси, чтобы не вылезать за края экрана).
 */
function computePosition(anchorRect, popoverRect, side, align, offset) {
    // Fallback на clientWidth/Height — на случай, если innerWidth/innerHeight временно
    // равны 0 (скрытая/неактивная вкладка, вкладка в процессе рендера и т.п.): без этого
    // вся математика ниже уезжает в отрицательные координаты (see: расчёт shift ниже).
    const viewport = {
        width: window.innerWidth || document.documentElement.clientWidth || 0,
        height: window.innerHeight || document.documentElement.clientHeight || 0,
    };

    function place(currentSide) {
        let x = 0;
        let y = 0;

        switch (currentSide) {
            case "bottom":
                y = anchorRect.bottom + offset;
                break;
            case "top":
                y = anchorRect.top - popoverRect.height - offset;
                break;
            case "left":
                x = anchorRect.left - popoverRect.width - offset;
                break;
            case "right":
                x = anchorRect.right + offset;
                break;
        }

        if (currentSide === "top" || currentSide === "bottom") {
            x = align === "start" ? anchorRect.left
                : align === "end" ? anchorRect.right - popoverRect.width
                    : anchorRect.left + anchorRect.width / 2 - popoverRect.width / 2;
        } else {
            y = align === "start" ? anchorRect.top
                : align === "end" ? anchorRect.bottom - popoverRect.height
                    : anchorRect.top + anchorRect.height / 2 - popoverRect.height / 2;
        }

        return { x, y };
    }

    function overflows(pos) {
        return pos.x < 0 || pos.y < 0
            || pos.x + popoverRect.width > viewport.width
            || pos.y + popoverRect.height > viewport.height;
    }

    // Насколько положение "не помещается" по главной оси — используется, чтобы решить,
    // стоит ли переворачивать сторону, и (если перевёрнутая тоже не помещается целиком)
    // выбрать тот вариант, где вылезания меньше, а не слепо один из двух.
    function mainAxisOverflowAmount(pos, currentSide) {
        return currentSide === "top" || currentSide === "bottom"
            ? Math.max(0, -pos.y) + Math.max(0, pos.y + popoverRect.height - viewport.height)
            : Math.max(0, -pos.x) + Math.max(0, pos.x + popoverRect.width - viewport.width);
    }

    let resolvedSide = side;
    let position = place(resolvedSide);

    const originalOverflow = mainAxisOverflowAmount(position, resolvedSide);
    if (originalOverflow > 0) {
        const flipped = OPPOSITE[resolvedSide];
        const flippedPosition = place(flipped);
        const flippedOverflow = mainAxisOverflowAmount(flippedPosition, flipped);

        // Переворачиваем, если так вылезание строго меньше — даже если не помещается
        // идеально ни один из вариантов (иначе поповер молча "залипал" бы в исходной,
        // худшей стороне — именно так проявлялся баг с некорректным позиционированием).
        if (flippedOverflow < originalOverflow) {
            resolvedSide = flipped;
            position = flippedPosition;
        }
    }

    // shift: сдвигаем вдоль второстепенной оси, чтобы не вылезать за края.
    // Если сам поповер шире/выше доступного viewport — просто прижимаем к margin
    // (Math.max ниже не даёт верхней границе диапазона уйти ниже margin).
    const margin = 8;
    const maxX = Math.max(margin, viewport.width - popoverRect.width - margin);
    const maxY = Math.max(margin, viewport.height - popoverRect.height - margin);
    position.x = Math.min(Math.max(position.x, margin), maxX);
    position.y = Math.min(Math.max(position.y, margin), maxY);

    return { x: Math.round(position.x), y: Math.round(position.y), side: resolvedSide };
}

/**
 * Позиционирует popoverEl относительно anchorEl. Вызывается один раз при открытии
 * и повторно при скролле/ресайзе (см. startTracking).
 */
export function position(popoverId, anchorEl, popoverEl, side, align, offset, matchAnchorWidth) {
    if (!anchorEl || !popoverEl) return;

    const anchorRect = anchorEl.getBoundingClientRect();

    // Ширину (если запрошена IsParentFullSize) выставляем ДО измерения popoverRect —
    // иначе высота посчитается по старой ширине (текст ещё не перенёсся по-новому),
    // и мы наступим на те же грабли, что и с position:fixed выше по файлу.
    if (matchAnchorWidth) {
        popoverEl.style.width = `${anchorRect.width}px`;
    }

    const popoverRect = popoverEl.getBoundingClientRect();
    const result = computePosition(anchorRect, popoverRect, side, align, offset ?? 8);

    popoverEl.style.position = "fixed";
    popoverEl.style.left = `${result.x}px`;
    popoverEl.style.top = `${result.y}px`;
    popoverEl.style.visibility = "visible";
    popoverEl.setAttribute("data-scarna-side", result.side);

    return result.side;
}

/**
 * Открывает "сессию" поповера: позиционирует его и подписывается на события,
 * которые должны его закрыть (клик снаружи, Escape, скролл/ресайз — репозиционирование).
 * @param {string} popoverId
 * @param {HTMLElement} anchorEl
 * @param {HTMLElement} popoverEl
 * @param {string} side
 * @param {string} align
 * @param {number} offset
 * @param {boolean} closeOnOutsideClick
 * @param {import("@microsoft/dotnet-js-interop").DotNetObject} dotNetRef
 * @param {boolean} [matchAnchorWidth] IsParentFullSize — растянуть поповер на ширину якоря
 */
export function open(popoverId, anchorEl, popoverEl, side, align, offset, closeOnOutsideClick, dotNetRef, matchAnchorWidth) {
    close(popoverId);

    position(popoverId, anchorEl, popoverEl, side, align, offset, matchAnchorWidth);

    // Класс анимации добавляем здесь, а не в разметке .razor: анимация должна начать
    // отсчёт именно в момент появления, а не в момент создания DOM-узла (который мог
    // произойти на десятки мс раньше — например, из-за задержки Blazor Server SignalR).
    popoverEl.classList.add("animate-scarna-fade-in");

    const reposition = () => position(popoverId, anchorEl, popoverEl, side, align, offset, matchAnchorWidth);
    const onScroll = () => requestAnimationFrame(reposition);
    const onResize = () => requestAnimationFrame(reposition);

    // Подстраховка сверх исправления в ScarnaPopover.razor (см. комментарий там про
    // position:fixed в разметке): если размер поповера или якоря всё же поменяется уже
    // ПОСЛЕ открытия (подгрузился шрифт/картинка, содержимое перерендерилось асинхронно),
    // ResizeObserver перепозиционирует без ожидания скролла/resize окна пользователем.
    let resizeObserver;
    if (typeof ResizeObserver !== "undefined") {
        resizeObserver = new ResizeObserver(() => requestAnimationFrame(reposition));
        resizeObserver.observe(popoverEl);
        resizeObserver.observe(anchorEl);
    }

    const onPointerDown = (event) => {
        if (!closeOnOutsideClick) return;
        const target = event.target;
        if (anchorEl.contains(target) || popoverEl.contains(target)) return;
        dotNetRef.invokeMethodAsync("OnOutsideInteractionAsync");
    };

    const onKeyDown = (event) => {
        if (event.key === "Escape") {
            dotNetRef.invokeMethodAsync("OnEscapeAsync");
        }
    };

    window.addEventListener("scroll", onScroll, { capture: true, passive: true });
    window.addEventListener("resize", onResize, { passive: true });
    document.addEventListener("pointerdown", onPointerDown, { capture: true });
    document.addEventListener("keydown", onKeyDown);

    registry.set(popoverId, {
        cleanup: () => {
            window.removeEventListener("scroll", onScroll, { capture: true });
            window.removeEventListener("resize", onResize);
            document.removeEventListener("pointerdown", onPointerDown, { capture: true });
            document.removeEventListener("keydown", onKeyDown);
            resizeObserver?.disconnect();
        },
    });
}

export function close(popoverId) {
    const entry = registry.get(popoverId);
    if (entry) {
        entry.cleanup();
        registry.delete(popoverId);
    }
}
