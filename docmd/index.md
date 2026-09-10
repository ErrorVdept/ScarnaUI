# NorthUI — документация

Библиотека Blazor-компонентов: паттерны взаимодействия (`@bind-Value`, императивные
сервисы, `Class`/`Style` на каждом компоненте) — от **MudBlazor**, дизайн-система
(цвета, радиусы, шрифт Inter) — вдохновлена **HeroUI**. Стилизация — Tailwind CSS,
собранный standalone-бинарником (без Node.js в проектах-потребителях).

Код библиотеки: `src/NorthUI/NorthUI/`. Полигон для проверки: `src/NorthUI/NorthUI.Demo/`.

## С чего начать

- [Установка и базовые классы](getting-started.md) — как подключить NorthUI к своему
  проекту и что такое `NorthComponentBase`/`NorthFormComponentBase`.
- [Тема и NorthThemeProvider](theme-provider.md) — как устроена рантайм-тема.
- [Кастомные CSS-классы](custom-classes.md) — `north-width-full`/`north-height-full`,
  `scrollable`/`scrollable-h`/`scrollable-v` и другие "фишки" NorthUI.

## Компоненты

### Тема
- [NorthThemeProvider](theme-provider.md) — провайдер темы, CSS-переменные, светлая/тёмная тема
- [NorthThemeSwitch](theme-switch.md) — готовая кнопка переключения темы

### Layout
- [NorthContainer](container.md) — центрирующий контейнер страницы
- [NorthStack](stack.md) — flex-контейнер (ряд/колонка + отступы, Justify/AlignItems как в MudBlazor)
- [NorthGrid / NorthItem](grid.md) — 12-колоночная сетка (тоже с Justify/Align)
- [NorthNavBar](navbar.md) — верхняя панель приложения
- [NorthDrawer](drawer.md) — выезжающая боковая панель
- [NorthBottomMenu / NorthBottomMenuItem](bottom-menu.md) — нижнее меню как в мобильных приложениях

### Actions
- [NorthButton](button.md) — кнопка (цвета, варианты заливки, размеры, анимация нажатия)
- [NorthToggleButton](toggle-button.md) — кнопка-переключатель (bool, выглядит как кнопка)

### Forms
- [NorthInput](input.md) — текстовое поле (Dense-плотность, Bordered)
- [NorthTextArea](textarea.md) — многострочное текстовое поле
- [NorthPasswordBox](password-box.md) — поле пароля с переключаемым отображением и настраиваемым символом маски
- [NorthNumericField](numeric-field.md) — числовое поле со ступенчатыми кнопками +/−
- [NorthSlider](slider.md) — слайдер числового значения
- [NorthTimeField](timefield.md) — поле выбора времени
- [NorthDateTimePicker](datetimepicker.md) — поле выбора даты и времени
- [NorthCheckbox](checkbox.md) — чекбокс (Dense-плотность, анимированная галочка)
- [NorthSwitch](switch.md) — переключатель (toggle, Dense-плотность)
- [NorthSelect / NorthSelectItem](select.md) — выпадающий список с одиночным выбором
- [NorthSelectGroup / NorthSelectGroupItem](select-group.md) — тот же выбор, что у Select, но в виде сегментированного переключателя (как Tabs)

### Data display
- [NorthCard](card.md) — карточка
- [NorthPaper](paper.md) — простая поверхность (без Header/Footer), опционально кликабельная с hover-анимацией
- [NorthChip](chip.md) — метка/тег
- [NorthTag](tag.md) — маленькая компактная метка (облегчённый Chip)
- [NorthBadge](badge.md) — индикатор-оверлей (счётчик, точка)
- [NorthAvatar](avatar.md) — аватар
- [NorthDivider](divider.md) — разделительная линия
- [NorthSeparator](separator.md) — разделитель, умеющий показать подпись посередине линии
- [NorthImage](image.md) — изображение с fallback при ошибке загрузки
- [NorthExpansionPanel](expansion-panel.md) — сворачиваемая панель (аккордеон-секция)
- [NorthList / NorthListItem](list.md) — список без выбора / с одиночным / с множественным выбором
- [NorthTable](table.md) — стилизация над обычной HTML-таблицей (аналог MudSimpleTable)
- [NorthProgress](progress.md) — линейная полоса прогресса (determinate/indeterminate)
- [NorthSpinner](spinner.md) — вращающийся индикатор загрузки (indeterminate)
- [NorthCodeBlock](code-block.md) — блок кода с кнопкой копирования, авто/фиксированная высота
- [NorthText](text.md) — типографический текст (аналог MudText)

### Navigation
- [NorthTabs / NorthTabPanel](tabs.md) — вкладки со скользящим pill-индикатором
- [NorthMenu / NorthMenuItem](menu.md) — выпадающее контекстное меню

### Overlays
- [NorthPopoverProvider / NorthPopover](popover.md) — примитив позиционирования (`NorthPosition`; на нём построены Tooltip, Select, SelectGroup, Menu и будущие DatePicker/Autocomplete)
- [NorthTooltip](tooltip.md) — всплывающая подсказка
- [Диалоги: INorthDialogService, NorthDialogProvider, NorthDialog](dialog-service.md) — императивные модалки
- [Тосты: IToastManager, NorthSnackbarProvider](toast-manager.md) — императивные уведомления

### Сервисы
- [Navigator (INavigator)](navigator.md) — обёртка над NavigationManager (переход, IsActive для подсветки активного пункта меню)

## Bordered

Большинство компонентов поддерживают параметр `Bordered` (`bool`) — включает/выключает
обводку независимо от `Variant`. Смотрите таблицу параметров конкретного компонента —
семантика чуть отличается там, где уже есть `Variant.Bordered` (Button/Chip/Select/Input):
там `Bordered` — это ДОПОЛНИТЕЛЬНАЯ обводка (или её принудительное отключение), а не
замена варианта.

## Плотность полей (Dense)

`NorthInput`, `NorthNumericField`, `NorthSelect`, `NorthCheckbox` и `NorthSwitch`
поддерживают параметр `Density="NorthDensity.Dense"` — уменьшенная высота/паддинги для
плотных панелей фильтров и таблиц, в дополнение к обычному размеру (`Normal`, по умолчанию).

## Анимации

Диалог, поповер/тултип/select/menu и тосты появляются с плавной анимацией входа
(`animate-north-fade-in`/`-scale-in`/`-slide-up-in`, уважают `prefers-reduced-motion`).
Кнопка чуть сжимается при нажатии (`active:scale-[0.97]`), чекбокс анимирует появление
галочки, а `NorthTabs`/`NorthSelectGroup` двигают pill-индикатор через JS-измерение
(`wwwroot/js/north/tabs.js`) вместо мгновенной подмены класса. Подробности —
в [getting-started.md](getting-started.md#анимации).

## Что дальше (не реализовано в этом проходе)

[NorthTimeField](timefield.md) и [NorthDateTimePicker](datetimepicker.md) уже есть, но
через нативные `input[type=time]`/`input[type=datetime-local]` — без своего попап-календаря.
По плану из `INFO.md` в библиотеке также предполагаются: `NorthAutocomplete`,
полноценный `NorthDatePicker` с календарной сеткой (как в MudDatePicker), `NorthColorPicker`,
`NorthNavMenu`/`NorthNavLink`. Все они технически строятся на уже готовом примитиве
`NorthPopoverProvider`/`NorthPopover` (см. [popover.md](popover.md)) — это следующий шаг.
