# Собирает ScarnaUI в локальный NuGet-пакет и кладёт его в подключаемый локальный
# источник — чтобы можно было `dotnet add package ScarnaUI` в другом проекте на
# этой же машине, без публикации куда-либо.
#
# По умолчанию версия пакета фиксированная (1.0.0-local) — специально, а не
# "версия под каждый запуск": NuGet резолвит/кэширует пакеты по номеру версии,
# и если бы каждый запуск давал новую версию, в проекте-потребителе пришлось бы
# вручную переставлять её после каждой пересборки. Вместо этого при КАЖДОМ
# запуске скрипт сам чистит именно эту версию в локальном кэше NuGet
# (~/.nuget/packages/scarnaui/<version>) — так `dotnet restore` в потребителе
# после пересборки всегда подтягивает свежие бинарники, а не старые из кэша.
#
# Использование:
#   ./pack-local.ps1                     - собрать 1.0.0-local, зарегистрировать источник (один раз)
#   ./pack-local.ps1 -Version 1.2.3      - собрать конкретную версию
#   ./pack-local.ps1 -SkipCacheClear     - не трогать глобальный кэш NuGet
#   ./pack-local.ps1 -SkipDemo           - только пакет, без сборки демо-проекта
#   ./pack-local.ps1 -DemoRuntime linux-x64 - демо под другую платформу (по умолчанию win-x64)
#
# Демо-проект (ScarnaUI.Demo) собирается в папку buildDemo одним self-contained exe
# (PublishSingleFile) — .NET на целевой машине ставить не нужно. Полностью "в один файл"
# не получится: статика (wwwroot, _content/ScarnaUI) и манифест *.staticwebassets.endpoints.json
# в ASP.NET Core лежат рядом с exe — переносите папку buildDemo целиком.
# Запуск: buildDemo\ScarnaUI.Demo.exe (по умолчанию http://localhost:5000).

param(
    [string]$Version = "1.0.0-local",
    [switch]$SkipCacheClear,
    [switch]$SkipDemo,
    [string]$DemoRuntime = "win-x64"
)

$ErrorActionPreference = "Stop"

$ProjectPath = Join-Path $PSScriptRoot "src/ScarnaUI/ScarnaUI/ScarnaUI.csproj"
$DemoProjectPath = Join-Path $PSScriptRoot "src/ScarnaUI/ScarnaUI.Demo/ScarnaUI.Demo.csproj"
$OutputDir = Join-Path $PSScriptRoot "nupkgs"
$DemoOutputDir = Join-Path $PSScriptRoot "buildDemo"
$SourceName = "ScarnaUILocal"

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null
}

Write-Host "Собираю ScarnaUI $Version..." -ForegroundColor Cyan
dotnet pack $ProjectPath -c Release -o $OutputDir -p:Version=$Version
if ($LASTEXITCODE -ne 0) {
    throw "dotnet pack завершился с ошибкой (код $LASTEXITCODE)"
}

# dotnet restore в проекте-потребителе берёт пакет из ~/.nuget/packages (глобальный
# кэш), если версия там уже когда-то встречалась — даже если содержимое .nupkg
# в $OutputDir с тех пор поменялось. Чистим кэш именно для scarnaui/$Version,
# чтобы следующий restore у потребителя гарантированно увидел свежую сборку.
if (-not $SkipCacheClear) {
    $globalPackagesLine = dotnet nuget locals global-packages --list
    $globalPackagesRoot = ($globalPackagesLine -split ":\s*", 2)[1].Trim()
    $cachedPackageDir = Join-Path $globalPackagesRoot "scarnaui\$Version"

    if (Test-Path $cachedPackageDir) {
        Write-Host "Чищу кэш NuGet для scarnaui/$Version..." -ForegroundColor Cyan
        Remove-Item -Recurse -Force $cachedPackageDir
    }
}

# Регистрируем локальную папку как источник NuGet — идемпотентно, пропускаем,
# если источник с таким именем уже подключён (повторный add упал бы с ошибкой).
# Важно: сравниваем со СКЛЕЕННОЙ в одну строку выдачей, а не с массивом строк —
# "$array -notmatch $x" в PowerShell возвращает массив НЕсовпавших строк (а не
# булево "совпадений вообще нет"), и этот массив почти всегда непустой (значит
# "истина") просто потому, что остальные источники не содержат нашего имени.
$existingSourcesText = (dotnet nuget list source) -join "`n"
if ($existingSourcesText -notmatch [regex]::Escape($SourceName)) {
    Write-Host "Регистрирую источник '$SourceName' -> $OutputDir..." -ForegroundColor Cyan
    dotnet nuget add source $OutputDir --name $SourceName | Out-Null
}
else {
    Write-Host "Источник '$SourceName' уже зарегистрирован." -ForegroundColor DarkGray
}

# Демо собирается в чистую папку: иначе файлы прошлых сборок (например, хэшированные
# имена статики вида app.<hash>.css) копились бы рядом с актуальными. Если демо сейчас
# запущено из buildDemo, exe занят и папку не удалить — об этом и сообщаем.
# Не используем IncludeAllContentForSelfExtract: с ним приложение распаковывается во
# временную папку и не находит рядом манифест статики (падает на MapStaticAssets).
if (-not $SkipDemo) {
    if (Test-Path $DemoOutputDir) {
        try {
            Remove-Item -Recurse -Force $DemoOutputDir
        }
        catch {
            throw "Не удалось очистить $DemoOutputDir — закройте запущенный ScarnaUI.Demo.exe и повторите. $($_.Exception.Message)"
        }
    }

    Write-Host ""
    Write-Host "Собираю демо ($DemoRuntime, один exe) в $DemoOutputDir..." -ForegroundColor Cyan
    dotnet publish $DemoProjectPath -c Release -r $DemoRuntime --self-contained true `
        -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=none `
        -o $DemoOutputDir
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish демо-проекта завершился с ошибкой (код $LASTEXITCODE)"
    }
}

Write-Host ""
Write-Host "Готово: $OutputDir\ScarnaUI.$Version.nupkg" -ForegroundColor Green
if (-not $SkipDemo) {
    $demoExe = if ($DemoRuntime -like "win-*") { "ScarnaUI.Demo.exe" } else { "ScarnaUI.Demo" }
    Write-Host "Демо:   $DemoOutputDir\$demoExe" -ForegroundColor Green
}
Write-Host ""
Write-Host "В проекте-потребителе:" -ForegroundColor Yellow
Write-Host "  dotnet add package ScarnaUI --version $Version --source $SourceName"

exit 0
