# Пересобирает wwwroot/css/scarna/scarna.css из Styles/input.css.
# Требует скачанный standalone-бинарник Tailwind CSS CLI (см. блок ниже) —
# Node.js/npm не нужны, см. docmd/theming.md.
#
# Использование:
#   ./build-css.ps1          - собрать один раз
#   ./build-css.ps1 -Watch   - пересобирать при изменении файлов

param(
    [switch]$Watch
)

$ToolsDir = Join-Path $PSScriptRoot "tools"
$Exe = Join-Path $ToolsDir "tailwindcss.exe"
$Version = "v4.3.3"

if (-not (Test-Path $Exe)) {
    Write-Host "Tailwind CLI не найден, скачиваю $Version..."
    New-Item -ItemType Directory -Force -Path $ToolsDir | Out-Null
    $Url = "https://github.com/tailwindlabs/tailwindcss/releases/download/$Version/tailwindcss-windows-x64.exe"
    Invoke-WebRequest -Uri $Url -OutFile $Exe
}

$InputCss = Join-Path $PSScriptRoot "Styles/input.css"
$OutputCss = Join-Path $PSScriptRoot "wwwroot/css/scarna/scarna.css"

if ($Watch) {
    & $Exe -i $InputCss -o $OutputCss --watch
} else {
    & $Exe -i $InputCss -o $OutputCss
}
