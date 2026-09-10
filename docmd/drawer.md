# NorthDrawer

## Файлы
- `src/NorthUI/NorthUI/Components/Layout/NorthDrawer.razor`
- `src/NorthUI/NorthUI/Components/Layout/NorthDrawerEnums.cs` (`NorthDrawerAnchor`, `NorthDrawerVariant`)

## Описание

Выезжающая панель (боковое меню, панель фильтров). В отличие от
[диалогов](dialog-service.md) — не императивный сервис, а обычный декларативный
компонент: кладётся в разметку (обычно в layout) и управляется через `@bind-Open`,
как `MudDrawer`.

## Variant — как панель ведёт себя относительно остального контента

Три варианта, аналог `DrawerVariant` в MudBlazor:

- **`Temporary`** (по умолчанию) — как было изначально: `position: fixed` поверх
  содержимого, с полупрозрачной подложкой (клик по ней закрывает панель). Не занимает
  места в потоке документа — сама по себе ничего не "раздвигает". Подходит для
  мобильного/оверлейного меню.
- **`Persistent`** — без подложки. Панель становится обычным flex-элементом
  (`position: relative`, `shrink-0`) с анимируемой шириной/высотой (`transition-[width,height]`).
  В закрытом состоянии схлопывается до `0`. Раз это настоящий элемент потока, а не
  оверлей — положите `NorthDrawer` рядом с содержимым внутри общего `flex`-контейнера,
  и всё, что рядом с ней (в том числе `NorthNavBar`, если он тоже внутри этого
  контейнера), само сдвинется вместе с открытием/закрытием, безо всякой ручной
  синхронизации отступов на стороне страницы:

  ```razor
  <div class="flex">
      <NorthDrawer @bind-Open="_open" Variant="NorthDrawerVariant.Persistent">...</NorthDrawer>
      <div class="flex-1">
          <NorthNavBar>...</NorthNavBar>  @* тоже сдвинется, если она внутри этой колонки *@
          <main>...</main>
      </div>
  </div>
  ```

- **`Mini`** — как `Persistent`, но в закрытом состоянии не схлопывается до нуля, а
  остаётся узкой полосой шириной/высотой `MiniWidth` (обычно под одни иконки) —
  аналог `DrawerVariant.Mini` в MudBlazor. Показывать подписи пунктов меню — решение
  самой страницы, компонент лишь анимирует размер контейнера, но **не прячьте подпись
  через `@if (Open) { ... }`** — так она появляется/исчезает рывком, не в такт с
  анимацией ширины панели, и выглядит как "прыжок" содержимого. Вместо этого держите
  `<span>` с подписью в DOM всегда, а видимость анимируйте той же длительностью
  (`transition-all duration-200`), что и у самого `NorthDrawer`:

  ```razor
  <span>
      @item.Icon
      <span class="@(Open ? "max-w-40 opacity-100" : "max-w-0 opacity-0") overflow-hidden whitespace-nowrap transition-all duration-200">
          @item.Text
      </span>
  </span>
  ```

Живые примеры всех трёх — на странице [`/showcase/main-layout`](../src/NorthUI/NorthUI.Demo/Components/Pages/Showcase/MainLayoutShowcase.razor)
(воспроизводит "Main Layouts" с wireframe'ов MudBlazor + дополнительные комбинации).

## Пример использования

```razor
<NorthDrawer @bind-Open="_menuOpen" Anchor="NorthDrawerAnchor.Left">
    <NorthList TValue="string" SelectionMode="NorthListSelectionMode.None" Class="p-3">
        <NorthListItem Value="@("home")">Главная</NorthListItem>
    </NorthList>
</NorthDrawer>

<NorthButton OnClick="() => _menuOpen = true">Меню</NorthButton>
```

## Параметры

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Open` / `OpenChanged` | `bool` / ... | — | Открыта ли панель. Поддерживает `@bind-Open` |
| `Anchor` | `NorthDrawerAnchor` | `Left` | `Left`/`Right`/`Top`/`Bottom` |
| `Variant` | `NorthDrawerVariant` | `Temporary` | `Temporary` (overlay+подложка) / `Persistent` (раздвигает соседей) / `Mini` (в закрытом состоянии — узкая полоса) |
| `Width` | `string` | `"280px"` | Ширина панели (для `Left`/`Right`) — любое CSS-значение |
| `Height` | `string` | `"280px"` | Высота панели (для `Top`/`Bottom`) |
| `MiniWidth` | `string` | `"72px"` | Ширина/высота в закрытом состоянии у `Variant.Mini`. Не используется у `Temporary`/`Persistent` |
| `CloseOnBackdropClick` | `bool` | `true` | Закрывать по клику на подложку (актуально только для `Temporary` — у остальных вариантов подложки нет) |
| `ChildContent` | `RenderFragment?` | — | Содержимое панели |
