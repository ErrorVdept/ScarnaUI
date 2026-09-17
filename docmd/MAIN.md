# ScarnaUI — обзор

Шпаргалка: что есть в библиотеке и с чего собрать типовую страницу. Полный список
с ссылками на подробные страницы — [index.md](index.md); установка —
[getting-started.md](getting-started.md).

## Компоненты коротко

### Layout — из чего собирать каркас страницы

| Компонент | Зачем |
|---|---|
| [ScarnaLayout](layout.md) | Корневая обёртка приложения (аналог MudLayout) — min-height экрана + базовые цвета темы, кладётся в `MainLayout.razor` |
| [ScarnaContainer](container.md) | Центрирующий контейнер с максимальной шириной — оборачивает контент страницы |
| [ScarnaStack](stack.md) | Flex-ряд/колонка с равным отступом между элементами — самый частый layout-компонент |
| [ScarnaGrid / ScarnaItem](grid.md) | 12-колоночная адаптивная сетка (Xs/Sm/Md/Lg) |
| [ScarnaNavBar](navbar.md) | Верхняя панель приложения (шапка, лого, меню) |
| [ScarnaDrawer](drawer.md) | Выезжающая боковая панель (мобильное меню, фильтры) |
| [ScarnaBottomMenu / ScarnaBottomMenuItem](bottom-menu.md) | Нижнее меню как в мобильных приложениях |
| [ScarnaNavMenu / ScarnaNavLink](nav-menu.md) | Вертикальное меню навигации — активный пункт определяется сам по текущему URL |

### Actions

| Компонент | Зачем |
|---|---|
| [ScarnaButton](button.md) | Кнопка: цвета, варианты заливки, размеры, `Loading`, рендер как `<a>` через `Href` |
| [ScarnaToggleButton](toggle-button.md) | Кнопка-переключатель (`bool`, выглядит как кнопка) |

### Forms — поля ввода

| Компонент | Зачем |
|---|---|
| [ScarnaInput](input.md) | Однострочное текстовое поле, `@bind-Value`, валидация в `EditForm` |
| [ScarnaTextArea](textarea.md) | Многострочное текстовое поле |
| [ScarnaPasswordBox](password-box.md) | Поле пароля с переключаемым отображением |
| [ScarnaNumericField](numeric-field.md) | Числовое поле со ступенчатыми кнопками +/− |
| [ScarnaSlider](slider.md) | Слайдер числового значения |
| [ScarnaTimeField](timefield.md) | Поле выбора времени |
| [ScarnaDateTimePicker](datetimepicker.md) | Поле выбора даты и времени |
| [ScarnaCheckbox](checkbox.md) | Чекбокс |
| [ScarnaSwitch](switch.md) | Переключатель (toggle) |
| [ScarnaSelect / ScarnaSelectItem](select.md) | Выпадающий список, одиночный выбор |
| [ScarnaSelectGroup / ScarnaSelectGroupItem](select-group.md) | Тот же выбор, но в виде сегментированного переключателя |

Все поля наследуют общий `@bind-Value`/валидацию из `ScarnaFormComponentBase` —
см. [getting-started.md](getting-started.md#scarnaformcomponentbaset).

### Data display — вывод контента

| Компонент | Зачем |
|---|---|
| [ScarnaCard](card.md) | Карточка с необязательными заголовком/подвалом |
| [ScarnaPaper](paper.md) | Простая поверхность без Header/Footer, опционально кликабельная |
| [ScarnaChip](chip.md) | Метка/тег |
| [ScarnaTag](tag.md) | Компактная метка (облегчённый Chip) |
| [ScarnaBadge](badge.md) | Индикатор-оверлей (счётчик, точка) поверх другого элемента |
| [ScarnaAvatar](avatar.md) | Аватар пользователя |
| [ScarnaDivider](divider.md) | Разделительная линия |
| [ScarnaSeparator](separator.md) | Разделитель с подписью посередине |
| [ScarnaImage](image.md) | Изображение с fallback при ошибке загрузки |
| [ScarnaExpansionPanel](expansion-panel.md) | Сворачиваемая панель (аккордеон-секция) |
| [ScarnaList / ScarnaListItem](list.md) | Список без выбора / с одиночным / с множественным выбором |
| [ScarnaTable](table.md) | Стилизация над обычной HTML-таблицей |
| [ScarnaProgress](progress.md) | Линейная полоса прогресса |
| [ScarnaSpinner](spinner.md) | Вращающийся индикатор загрузки |
| [ScarnaCodeBlock](code-block.md) | Блок кода с кнопкой копирования |
| [ScarnaText](text.md) | Типографический текст (заголовки, параграфы по токенам темы) |

### Navigation

| Компонент | Зачем |
|---|---|
| [ScarnaTabs / ScarnaTabPanel](tabs.md) | Вкладки со скользящим pill-индикатором |
| [ScarnaMenu / ScarnaMenuItem](menu.md) | Выпадающее контекстное меню |

### Overlays — модалки, всплывающие элементы

| Компонент | Зачем |
|---|---|
| [ScarnaPopoverProvider / ScarnaPopover](popover.md) | Базовый примитив позиционирования (на нём построены Tooltip/Select/Menu) |
| [ScarnaTooltip](tooltip.md) | Всплывающая подсказка |
| [Диалоги (IScarnaDialogService, ScarnaDialogProvider, ScarnaDialog)](dialog-service.md) | Императивные модальные окна из C#-кода |
| [Тосты (IToastManager, ScarnaSnackbarProvider)](toast-manager.md) | Императивные всплывающие уведомления |

### Тема и сервисы

| Компонент | Зачем |
|---|---|
| [ScarnaThemeProvider](theme-provider.md) | Корневой провайдер темы — оборачивает всё приложение |
| [ScarnaThemeSwitch](theme-switch.md) | Готовая кнопка переключения светлой/тёмной темы |
| [Navigator (INavigator)](navigator.md) | Обёртка над `NavigationManager` (переход, `IsActive` для подсветки меню) |

## Обязательный каркас приложения

Провайдеры регистрируются один раз в `MainLayout.razor` (порядок важен — Popover
должен оборачивать `@Body`):

```razor
<ScarnaThemeProvider>
    <ScarnaPopoverProvider>
        <ScarnaLayout>
            <ScarnaNavBar>...</ScarnaNavBar>
            <main class="py-8">@Body</main>
        </ScarnaLayout>
    </ScarnaPopoverProvider>

    <ScarnaDialogProvider />
    <ScarnaSnackbarProvider />
</ScarnaThemeProvider>
```

Без `ScarnaPopoverProvider` не будут работать Tooltip/Select/Menu, без
`ScarnaDialogProvider`/`ScarnaSnackbarProvider` — диалоги через `IScarnaDialogService`
и тосты через `IToastManager`. `ScarnaLayout` — просто холст (min-height экрана +
цвета темы), логики не содержит, см. [layout.md](layout.md). Подключение CSS,
FOUC-скрипт и регистрация DI — в [getting-started.md](getting-started.md).

## Базовые примеры вёрстки

### Страница с шапкой и центрированным контентом

```razor
<ScarnaNavBar>
    <a href="/" class="text-lg font-semibold">MyApp</a>
    <ScarnaStack Row="true" Spacing="6">
        <NavLink href="/">Главная</NavLink>
        <NavLink href="/about">О нас</NavLink>
    </ScarnaStack>
</ScarnaNavBar>

<ScarnaContainer Class="py-8">
    <ScarnaText Typo="Typo.H4">Заголовок страницы</ScarnaText>

    <ScarnaStack Spacing="4" Class="mt-4">
        <ScarnaCard>
            <HeaderContent><span class="font-semibold">Заказ #1234</span></HeaderContent>
            Товар: Кроссовки, размер 42. Статус: в пути.
            <FooterContent><ScarnaButton Size="Size.Small">Подробнее</ScarnaButton></FooterContent>
        </ScarnaCard>
    </ScarnaStack>
</ScarnaContainer>
```

### Сетка карточек (адаптивная)

```razor
<ScarnaGrid Spacing="4">
    <ScarnaItem Xs="12" Md="6" Lg="4"><ScarnaCard>Карточка 1</ScarnaCard></ScarnaItem>
    <ScarnaItem Xs="12" Md="6" Lg="4"><ScarnaCard>Карточка 2</ScarnaCard></ScarnaItem>
    <ScarnaItem Xs="12" Md="6" Lg="4"><ScarnaCard>Карточка 3</ScarnaCard></ScarnaItem>
</ScarnaGrid>
```

### Панель действий (ряд с выравниванием по краям)

```razor
<ScarnaStack Row="true" Spacing="3" Justify="Justify.SpaceBetween" Align="Align.Center">
    <span>Итого: 1 200 ₽</span>
    <ScarnaStack Row="true" Spacing="2">
        <ScarnaButton Variant="Variant.Light">Отмена</ScarnaButton>
        <ScarnaButton Color="Color.Primary">Оформить</ScarnaButton>
    </ScarnaStack>
</ScarnaStack>
```

### Форма

```razor
<EditForm Model="_model" OnValidSubmit="Submit">
    <DataAnnotationsValidator />
    <ScarnaStack Spacing="4" Class="max-w-sm">
        <ScarnaInput Label="Email" Type="email" @bind-Value="_model.Email" />
        <ScarnaPasswordBox Label="Пароль" @bind-Value="_model.Password" />
        <ScarnaButton Type="submit" Color="Color.Primary" FullWidth="true">Войти</ScarnaButton>
    </ScarnaStack>
</EditForm>
```

### Мобильное приложение (боковое меню + нижнее меню)

```razor
<ScarnaDrawer @bind-Open="_menuOpen" Anchor="ScarnaDrawerAnchor.Left">
    <ScarnaNavMenu Class="p-3">
        <ScarnaNavLink Href="/" OnClick="() => _menuOpen = false">Главная</ScarnaNavLink>
        <ScarnaNavLink Href="/profile" OnClick="() => _menuOpen = false">Профиль</ScarnaNavLink>
    </ScarnaNavMenu>
</ScarnaDrawer>

@Body

<ScarnaBottomMenu @bind-SelectedIndex="_tab">
    <ScarnaBottomMenuItem Text="Главная"><HomeIcon /></ScarnaBottomMenuItem>
    <ScarnaBottomMenuItem Text="Поиск"><SearchIcon /></ScarnaBottomMenuItem>
    <ScarnaBottomMenuItem Text="Профиль"><ProfileIcon /></ScarnaBottomMenuItem>
</ScarnaBottomMenu>
```

## Дальше

- Полный справочник по всем компонентам с таблицами параметров — [index.md](index.md).
- Дизайн-токены темы (цвета, радиусы) — [theme-provider.md](theme-provider.md).
- Кастомные CSS-утилиты ScarnaUI (`scarna-width-full`, `scrollable` и т.п.) —
  [custom-classes.md](custom-classes.md).
