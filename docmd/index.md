# ScarnaUI — документация

Библиотека Blazor-компонентов: паттерны взаимодействия (`@bind-Value`, императивные
сервисы, `Class`/`Style` на каждом компоненте) — от **MudBlazor**, дизайн-система
(цвета, радиусы, шрифт Inter) — вдохновлена **HeroUI**. Стилизация — Tailwind CSS,
собранный standalone-бинарником (без Node.js в проектах-потребителях).

Код библиотеки: `src/ScarnaUI/ScarnaUI/`. Полигон для проверки: `src/ScarnaUI/ScarnaUI.Demo/`.

## С чего начать

- [MAIN — обзор компонентов и примеры вёрстки](MAIN.md) — краткая шпаргалка: что
  есть в библиотеке и как собрать типовую страницу.
- [Установка и базовые классы](getting-started.md) — как подключить ScarnaUI к своему
  проекту и что такое `ScarnaComponentBase`/`ScarnaFormComponentBase`.
- [Тема и ScarnaThemeProvider](theme-provider.md) — как устроена рантайм-тема.
- [Кастомные CSS-классы](custom-classes.md) — `scarna-width-full`/`scarna-height-full`,
  `scrollable`/`scrollable-h`/`scrollable-v` и другие "фишки" ScarnaUI.

## Компоненты

### Тема
- [ScarnaThemeProvider](theme-provider.md) — провайдер темы, CSS-переменные, светлая/тёмная тема
- [ScarnaThemeSwitch](theme-switch.md) — готовая кнопка переключения темы

### Layout
- [ScarnaLayout](layout.md) — корневая обёртка приложения (аналог MudLayout), min-height экрана + базовые цвета темы
- [ScarnaContainer](container.md) — центрирующий контейнер страницы
- [ScarnaStack](stack.md) — flex-контейнер (ряд/колонка + отступы, Justify/AlignItems как в MudBlazor)
- [ScarnaGrid / ScarnaItem](grid.md) — 12-колоночная сетка (тоже с Justify/Align)
- [ScarnaNavBar](navbar.md) — верхняя панель приложения
- [ScarnaDrawer](drawer.md) — выезжающая боковая панель
- [ScarnaBottomMenu / ScarnaBottomMenuItem](bottom-menu.md) — нижнее меню как в мобильных приложениях
- [ScarnaNavMenu / ScarnaNavLink](nav-menu.md) — вертикальное меню навигации с автоопределением активного пункта

### Actions
- [ScarnaButton](button.md) — кнопка (цвета, варианты заливки, размеры, анимация нажатия)
- [ScarnaToggleButton](toggle-button.md) — кнопка-переключатель (bool, выглядит как кнопка)

### Forms
- [ScarnaInput](input.md) — текстовое поле (Dense-плотность, Bordered)
- [ScarnaTextArea](textarea.md) — многострочное текстовое поле
- [ScarnaPasswordBox](password-box.md) — поле пароля с переключаемым отображением и настраиваемым символом маски
- [ScarnaNumericField](numeric-field.md) — числовое поле со ступенчатыми кнопками +/−
- [ScarnaSlider](slider.md) — слайдер числового значения
- [ScarnaTimeField](timefield.md) — поле выбора времени
- [ScarnaDateTimePicker](datetimepicker.md) — поле выбора даты и времени
- [ScarnaCheckbox](checkbox.md) — чекбокс (Dense-плотность, анимированная галочка)
- [ScarnaSwitch](switch.md) — переключатель (toggle, Dense-плотность)
- [ScarnaSelect / ScarnaSelectItem](select.md) — выпадающий список с одиночным выбором
- [ScarnaSelectGroup / ScarnaSelectGroupItem](select-group.md) — тот же выбор, что у Select, но в виде сегментированного переключателя (как Tabs)

### Data display
- [ScarnaCard](card.md) — карточка
- [ScarnaPaper](paper.md) — простая поверхность (без Header/Footer), опционально кликабельная с hover-анимацией
- [ScarnaChip](chip.md) — метка/тег
- [ScarnaTag](tag.md) — маленькая компактная метка (облегчённый Chip)
- [ScarnaBadge](badge.md) — индикатор-оверлей (счётчик, точка)
- [ScarnaAvatar](avatar.md) — аватар
- [ScarnaDivider](divider.md) — разделительная линия
- [ScarnaSeparator](separator.md) — разделитель, умеющий показать подпись посередине линии
- [ScarnaImage](image.md) — изображение с fallback при ошибке загрузки
- [ScarnaExpansionPanel](expansion-panel.md) — сворачиваемая панель (аккордеон-секция)
- [ScarnaList / ScarnaListItem](list.md) — список без выбора / с одиночным / с множественным выбором
- [ScarnaTable](table.md) — стилизация над обычной HTML-таблицей (аналог MudSimpleTable)
- [ScarnaProgress](progress.md) — линейная полоса прогресса (determinate/indeterminate)
- [ScarnaSpinner](spinner.md) — вращающийся индикатор загрузки (indeterminate)
- [ScarnaCodeBlock](code-block.md) — блок кода с кнопкой копирования, авто/фиксированная высота
- [ScarnaText](text.md) — типографический текст (аналог MudText)

### Navigation
- [ScarnaTabs / ScarnaTabPanel](tabs.md) — вкладки со скользящим pill-индикатором
- [ScarnaMenu / ScarnaMenuItem](menu.md) — выпадающее контекстное меню

### Overlays
- [ScarnaPopoverProvider / ScarnaPopover](popover.md) — примитив позиционирования (`ScarnaPosition`; на нём построены Tooltip, Select, SelectGroup, Menu и будущие DatePicker/Autocomplete)
- [ScarnaTooltip](tooltip.md) — всплывающая подсказка
- [Диалоги: IScarnaDialogService, ScarnaDialogProvider, ScarnaDialog](dialog-service.md) — императивные модалки
- [Тосты: IToastManager, ScarnaSnackbarProvider](toast-manager.md) — императивные уведомления

### Сервисы
- [Navigator (INavigator)](navigator.md) — обёртка над NavigationManager (переход, IsActive для подсветки активного пункта меню)

## Bordered

Большинство компонентов поддерживают параметр `Bordered` (`bool`) — включает/выключает
обводку независимо от `Variant`. Смотрите таблицу параметров конкретного компонента —
семантика чуть отличается там, где уже есть `Variant.Bordered` (Button/Chip/Select/Input):
там `Bordered` — это ДОПОЛНИТЕЛЬНАЯ обводка (или её принудительное отключение), а не
замена варианта.

## Плотность полей (Dense)

`ScarnaInput`, `ScarnaNumericField`, `ScarnaSelect`, `ScarnaCheckbox` и `ScarnaSwitch`
поддерживают параметр `Density="ScarnaDensity.Dense"` — уменьшенная высота/паддинги для
плотных панелей фильтров и таблиц, в дополнение к обычному размеру (`Normal`, по умолчанию).

## Анимации

Диалог, поповер/тултип/select/menu и тосты появляются с плавной анимацией входа
(`animate-scarna-fade-in`/`-scale-in`/`-slide-up-in`, уважают `prefers-reduced-motion`).
Кнопка чуть сжимается при нажатии (`active:scale-[0.97]`), чекбокс анимирует появление
галочки, а `ScarnaTabs`/`ScarnaSelectGroup` двигают pill-индикатор через JS-измерение
(`wwwroot/js/scarna/tabs.js`) вместо мгновенной подмены класса. Подробности —
в [getting-started.md](getting-started.md#анимации).

## Что дальше (не реализовано в этом проходе)

[ScarnaTimeField](timefield.md) и [ScarnaDateTimePicker](datetimepicker.md) уже есть, но
через нативные `input[type=time]`/`input[type=datetime-local]` — без своего попап-календаря.
По плану из `INFO.md` в библиотеке также предполагаются: `ScarnaAutocomplete`,
полноценный `ScarnaDatePicker` с календарной сеткой (как в MudDatePicker), `ScarnaColorPicker`,
`ScarnaNavMenu`/`ScarnaNavLink`. Все они технически строятся на уже готовом примитиве
`ScarnaPopoverProvider`/`ScarnaPopover` (см. [popover.md](popover.md)) — это следующий шаг.
