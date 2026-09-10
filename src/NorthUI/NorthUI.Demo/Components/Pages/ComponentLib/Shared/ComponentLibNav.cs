namespace NorthUI.Demo.Components.Pages.ComponentLib.Shared;

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
            new ComponentLibNavItem("NorthButton", "/componentlib/button", "Кнопка"),
            new ComponentLibNavItem("NorthToggleButton", "/componentlib/toggle-button", "Кнопка-переключатель"),
        }),
        new("Forms", new[]
        {
            new ComponentLibNavItem("NorthInput", "/componentlib/input", "Текстовое поле"),
            new ComponentLibNavItem("NorthTextArea", "/componentlib/textarea", "Многострочное поле"),
            new ComponentLibNavItem("NorthPasswordBox", "/componentlib/password-box", "Поле пароля"),
            new ComponentLibNavItem("NorthNumericField", "/componentlib/numeric-field", "Числовое поле"),
            new ComponentLibNavItem("NorthSlider", "/componentlib/slider", "Слайдер"),
            new ComponentLibNavItem("NorthTimeField", "/componentlib/time-field", "Выбор времени"),
            new ComponentLibNavItem("NorthDateTimePicker", "/componentlib/datetime-picker", "Выбор даты и времени"),
            new ComponentLibNavItem("NorthCheckbox", "/componentlib/checkbox", "Чекбокс"),
            new ComponentLibNavItem("NorthSwitch", "/componentlib/switch", "Переключатель"),
            new ComponentLibNavItem("NorthSelect", "/componentlib/select", "Выпадающий список"),
            new ComponentLibNavItem("NorthSelectGroup", "/componentlib/select-group", "Сегментированный переключатель"),
        }),
        new("Data display", new[]
        {
            new ComponentLibNavItem("NorthCard", "/componentlib/card", "Карточка"),
            new ComponentLibNavItem("NorthPaper", "/componentlib/paper", "Простая поверхность"),
            new ComponentLibNavItem("NorthChip", "/componentlib/chip", "Метка/тег"),
            new ComponentLibNavItem("NorthTag", "/componentlib/tag", "Компактная метка"),
            new ComponentLibNavItem("NorthBadge", "/componentlib/badge", "Индикатор-оверлей"),
            new ComponentLibNavItem("NorthAvatar", "/componentlib/avatar", "Аватар"),
            new ComponentLibNavItem("NorthDivider", "/componentlib/divider", "Разделительная линия"),
            new ComponentLibNavItem("NorthSeparator", "/componentlib/separator", "Разделитель с подписью"),
            new ComponentLibNavItem("NorthImage", "/componentlib/image", "Изображение с fallback"),
            new ComponentLibNavItem("NorthExpansionPanel", "/componentlib/expansion-panel", "Аккордеон-секция"),
            new ComponentLibNavItem("NorthList", "/componentlib/list", "Список с выбором"),
            new ComponentLibNavItem("NorthTable", "/componentlib/table", "Таблица"),
            new ComponentLibNavItem("NorthProgress", "/componentlib/progress", "Полоса прогресса"),
            new ComponentLibNavItem("NorthSpinner", "/componentlib/spinner", "Индикатор загрузки"),
            new ComponentLibNavItem("NorthCodeBlock", "/componentlib/code-block", "Блок кода с кнопкой копирования"),
            new ComponentLibNavItem("NorthText", "/componentlib/text", "Типографический текст"),
        }),
        new("Layout", new[]
        {
            new ComponentLibNavItem("NorthContainer", "/componentlib/container", "Центрирующий контейнер"),
            new ComponentLibNavItem("NorthStack", "/componentlib/stack", "Flex-контейнер"),
            new ComponentLibNavItem("NorthGrid", "/componentlib/grid", "12-колоночная сетка"),
            new ComponentLibNavItem("NorthNavBar", "/componentlib/navbar", "Верхняя панель"),
            new ComponentLibNavItem("NorthDrawer", "/componentlib/drawer", "Выезжающая панель"),
            new ComponentLibNavItem("NorthBottomMenu", "/componentlib/bottom-menu", "Нижнее меню"),
        }),
        new("Navigation", new[]
        {
            new ComponentLibNavItem("NorthTabs", "/componentlib/tabs", "Вкладки"),
            new ComponentLibNavItem("NorthMenu", "/componentlib/menu", "Контекстное меню"),
        }),
        new("Overlays", new[]
        {
            new ComponentLibNavItem("NorthPopover", "/componentlib/popover", "Позиционирование поповера"),
            new ComponentLibNavItem("NorthTooltip", "/componentlib/tooltip", "Всплывающая подсказка"),
            new ComponentLibNavItem("Диалоги", "/componentlib/dialog", "INorthDialogService"),
            new ComponentLibNavItem("Тосты", "/componentlib/toast", "IToastManager"),
        }),
        new("Тема", new[]
        {
            new ComponentLibNavItem("NorthThemeProvider", "/componentlib/theme-provider", "Провайдер темы"),
            new ComponentLibNavItem("NorthThemeSwitch", "/componentlib/theme-switch", "Кнопка переключения темы"),
        }),
        new("Сервисы", new[]
        {
            new ComponentLibNavItem("Navigator", "/componentlib/navigator", "INavigator — обёртка над NavigationManager"),
        }),
    };
}
