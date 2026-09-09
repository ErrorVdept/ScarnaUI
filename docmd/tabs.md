# NorthTabs / NorthTabPanel

## Файлы
- `src/NorthUI/NorthUI/Components/Navigation/NorthTabs.razor`
- `src/NorthUI/NorthUI/Components/Navigation/NorthTabPanel.razor`

## Описание

Вкладки с **pill-фоном под активной вкладкой** — заливка, а не 2px-подчёркивание.
Это прямой ответ на проблему, из-за которой проект вообще ушёл от стилизации
MudBlazor (см. `INFO.md`): у `MudTabs` активная вкладка подсвечивается тонкой
полоской `.mud-tab-slider`, и её нельзя перекрасить в фон без переписывания разметки
самого компонента. Здесь разметка своя, поэтому pill — обычный CSS-класс.

`NorthTabPanel` не рисует свой заголовок — заголовки (кнопки с `Text`) рисует сам
`NorthTabs`, опрашивая зарегистрированные панели. Видна только активная панель
(неактивные не рендерятся — при переключении вкладки её внутреннее состояние сбрасывается).

## Пример использования

```razor
<NorthTabs @bind-SelectedIndex="_tab">
    <NorthTabPanel Text="Профиль">
        <ProfileForm />
    </NorthTabPanel>
    <NorthTabPanel Text="Настройки">
        <SettingsForm />
    </NorthTabPanel>
</NorthTabs>

@code {
    private int _tab;
}
```

## Параметры NorthTabs

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `SelectedIndex` / `SelectedIndexChanged` | `int` | `0` | Индекс активной вкладки, поддерживает `@bind-SelectedIndex` |
| `Justify` | `NorthJustify` | `Start` | К какому краю прижимать заголовки (заметно, только если `FullWidth="false"`) — как AlignItems в MudBlazor |
| `FullWidth` | `bool` | `false` | Растянуть заголовки на всю ширину дорожки поровну |
| `ChildContent` | `RenderFragment?` | — | Набор `<NorthTabPanel>` |

## Параметры NorthTabPanel

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Text` | `string` | — | **Обязательный.** Текст заголовка вкладки |
| `ChildContent` | `RenderFragment?` | — | Содержимое, видимое только когда вкладка активна |

`NorthTabPanel` обязан использоваться внутри `NorthTabs` — иначе бросает исключение
при инициализации.
