# ScarnaTabs / ScarnaTabPanel

## Файлы
- `src/ScarnaUI/ScarnaUI/Components/Navigation/ScarnaTabs.razor`
- `src/ScarnaUI/ScarnaUI/Components/Navigation/ScarnaTabPanel.razor`

## Описание

Вкладки с **pill-фоном под активной вкладкой** — заливка, а не 2px-подчёркивание.
Это прямой ответ на проблему, из-за которой проект вообще ушёл от стилизации
MudBlazor (см. `INFO.md`): у `MudTabs` активная вкладка подсвечивается тонкой
полоской `.mud-tab-slider`, и её нельзя перекрасить в фон без переписывания разметки
самого компонента. Здесь разметка своя, поэтому pill — обычный CSS-класс.

`ScarnaTabPanel` не рисует свой заголовок — заголовки (кнопки с `Text`) рисует сам
`ScarnaTabs`, опрашивая зарегистрированные панели. Видна только активная панель
(неактивные не рендерятся — при переключении вкладки её внутреннее состояние сбрасывается).

## Пример использования

```razor
<ScarnaTabs @bind-SelectedIndex="_tab">
    <ScarnaTabPanel Text="Профиль">
        <ProfileForm />
    </ScarnaTabPanel>
    <ScarnaTabPanel Text="Настройки">
        <SettingsForm />
    </ScarnaTabPanel>
</ScarnaTabs>

@code {
    private int _tab;
}
```

## Параметры ScarnaTabs

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `SelectedIndex` / `SelectedIndexChanged` | `int` | `0` | Индекс активной вкладки, поддерживает `@bind-SelectedIndex` |
| `Justify` | `Justify` | `Start` | К какому краю прижимать заголовки (заметно, только если `FullWidth="false"`) — как AlignItems в MudBlazor |
| `FullWidth` | `bool` | `false` | Растянуть заголовки на всю ширину дорожки поровну |
| `ChildContent` | `RenderFragment?` | — | Набор `<ScarnaTabPanel>` |

## Параметры ScarnaTabPanel

| Параметр | Тип | По умолчанию | Назначение |
|---|---|---|---|
| `Text` | `string` | — | **Обязательный.** Текст заголовка вкладки |
| `ChildContent` | `RenderFragment?` | — | Содержимое, видимое только когда вкладка активна |

`ScarnaTabPanel` обязан использоваться внутри `ScarnaTabs` — иначе бросает исключение
при инициализации.
