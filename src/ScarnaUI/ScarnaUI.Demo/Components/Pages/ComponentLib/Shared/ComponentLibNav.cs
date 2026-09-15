namespace ScarnaUI.Demo.Components.Pages.ComponentLib.Shared;

/// <summary>Один пункт левого меню документации — компонент/сервис, ссылка на его страницу и короткое описание (для карточек на Index.razor).</summary>
public sealed record ComponentLibNavItem(string Name, string Href, string Description);

/// <summary>Категория левого меню документации (Actions, Forms, Data display, ...).</summary>
public sealed record ComponentLibNavGroup(string Title, ComponentLibNavItem[] Items);

/// <summary>
/// Единый источник списка страниц ComponentLib — и для левого меню (ComponentLibSidebar),
/// и для карточек на странице-оглавлении (Index.razor), чтобы список не расходился
/// между двумя местами.
/// </summary>
public static class ComponentLibNav
{
    public static readonly ComponentLibNavGroup[] Groups =
    {
        new("Actions", new[]
        {
            new ComponentLibNavItem("ScarnaButton", "/componentlib/button", "Кнопка"),
            new ComponentLibNavItem("ScarnaToggleButton", "/componentlib/toggle-button", "Кнопка-переключатель"),
        }),
        new("Forms", new[]
        {
            new ComponentLibNavItem("ScarnaInput", "/componentlib/input", "Текстовое поле"),
            new ComponentLibNavItem("ScarnaTextArea", "/componentlib/textarea", "Многострочное поле"),
            new ComponentLibNavItem("ScarnaPasswordBox", "/componentlib/password-box", "Поле пароля"),
            new ComponentLibNavItem("ScarnaNumericField", "/componentlib/numeric-field", "Числовое поле"),
            new ComponentLibNavItem("ScarnaSlider", "/componentlib/slider", "Слайдер"),
            new ComponentLibNavItem("ScarnaTimeField", "/componentlib/time-field", "Выбор времени"),
            new ComponentLibNavItem("ScarnaDateTimePicker", "/componentlib/datetime-picker", "Выбор даты и времени"),
            new ComponentLibNavItem("ScarnaCheckbox", "/componentlib/checkbox", "Чекбокс"),
            new ComponentLibNavItem("ScarnaSwitch", "/componentlib/switch", "Переключатель"),
            new ComponentLibNavItem("ScarnaSelect", "/componentlib/select", "Выпадающий список"),
            new ComponentLibNavItem("ScarnaSelectGroup", "/componentlib/select-group", "Сегментированный переключатель"),
        }),
        new("Data display", new[]
        {
            new ComponentLibNavItem("ScarnaCard", "/componentlib/card", "Карточка"),
            new ComponentLibNavItem("ScarnaPaper", "/componentlib/paper", "Простая поверхность"),
            new ComponentLibNavItem("ScarnaChip", "/componentlib/chip", "Метка/тег"),
            new ComponentLibNavItem("ScarnaTag", "/componentlib/tag", "Компактная метка"),
            new ComponentLibNavItem("ScarnaBadge", "/componentlib/badge", "Индикатор-оверлей"),
            new ComponentLibNavItem("ScarnaAvatar", "/componentlib/avatar", "Аватар"),
            new ComponentLibNavItem("ScarnaDivider", "/componentlib/divider", "Разделительная линия"),
            new ComponentLibNavItem("ScarnaSeparator", "/componentlib/separator", "Разделитель с подписью"),
            new ComponentLibNavItem("ScarnaImage", "/componentlib/image", "Изображение с fallback"),
            new ComponentLibNavItem("ScarnaExpansionPanel", "/componentlib/expansion-panel", "Аккордеон-секция"),
            new ComponentLibNavItem("ScarnaList", "/componentlib/list", "Список с выбором"),
            new ComponentLibNavItem("ScarnaTable", "/componentlib/table", "Таблица"),
            new ComponentLibNavItem("ScarnaProgress", "/componentlib/progress", "Полоса прогресса"),
            new ComponentLibNavItem("ScarnaSpinner", "/componentlib/spinner", "Индикатор загрузки"),
            new ComponentLibNavItem("ScarnaCodeBlock", "/componentlib/code-block", "Блок кода с кнопкой копирования"),
            new ComponentLibNavItem("ScarnaText", "/componentlib/text", "Типографический текст"),
        }),
        new("Layout", new[]
        {
            new ComponentLibNavItem("ScarnaContainer", "/componentlib/container", "Центрирующий контейнер"),
            new ComponentLibNavItem("ScarnaStack", "/componentlib/stack", "Flex-контейнер"),
            new ComponentLibNavItem("ScarnaGrid", "/componentlib/grid", "12-колоночная сетка"),
            new ComponentLibNavItem("ScarnaNavBar", "/componentlib/navbar", "Верхняя панель"),
            new ComponentLibNavItem("ScarnaDrawer", "/componentlib/drawer", "Выезжающая панель"),
            new ComponentLibNavItem("ScarnaBottomMenu", "/componentlib/bottom-menu", "Нижнее меню"),
            new ComponentLibNavItem("ScarnaNavMenu", "/componentlib/nav-menu", "Вертикальная навигация"),
        }),
        new("Navigation", new[]
        {
            new ComponentLibNavItem("ScarnaTabs", "/componentlib/tabs", "Вкладки"),
            new ComponentLibNavItem("ScarnaMenu", "/componentlib/menu", "Контекстное меню"),
        }),
        new("Overlays", new[]
        {
            new ComponentLibNavItem("ScarnaPopover", "/componentlib/popover", "Позиционирование поповера"),
            new ComponentLibNavItem("ScarnaTooltip", "/componentlib/tooltip", "Всплывающая подсказка"),
            new ComponentLibNavItem("Диалоги", "/componentlib/dialog", "IScarnaDialogService"),
            new ComponentLibNavItem("Тосты", "/componentlib/toast", "IToastManager"),
        }),
        new("Тема", new[]
        {
            new ComponentLibNavItem("ScarnaThemeProvider", "/componentlib/theme-provider", "Провайдер темы"),
            new ComponentLibNavItem("ScarnaThemeSwitch", "/componentlib/theme-switch", "Кнопка переключения темы"),
        }),
        new("Сервисы", new[]
        {
            new ComponentLibNavItem("Navigator", "/componentlib/navigator", "INavigator — обёртка над NavigationManager"),
        }),
    };
}
