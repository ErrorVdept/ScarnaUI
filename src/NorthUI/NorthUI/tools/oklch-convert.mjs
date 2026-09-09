// Разовый скрипт: конвертирует OKLCH-токены темы HeroUI в HSL-триплеты ("H S% L%"),
// в которых NorthTheme.cs/variables.css хранят цвета. Точная реализация формул
// OKLab<->linear sRGB по спецификации Björn Ottosson (https://bottosson.github.io/posts/oklab/).

function oklchToSrgb(L, C, Hdeg) {
    const h = (Hdeg * Math.PI) / 180;
    const a = C * Math.cos(h);
    const b = C * Math.sin(h);

    const l_ = L + 0.3963377774 * a + 0.2158037573 * b;
    const m_ = L - 0.1055613458 * a - 0.0638541728 * b;
    const s_ = L - 0.0894841775 * a - 1.2914855480 * b;

    const l = l_ ** 3;
    const m = m_ ** 3;
    const s = s_ ** 3;

    let r = +4.0767416621 * l - 3.3077115913 * m + 0.2309699292 * s;
    let g = -1.2684380046 * l + 2.6097574011 * m - 0.3413193965 * s;
    let bl = -0.0041960863 * l - 0.7034186147 * m + 1.7076147010 * s;

    const gamma = (c) => {
        c = Math.min(1, Math.max(0, c));
        return c <= 0.0031308 ? 12.92 * c : 1.055 * Math.pow(c, 1 / 2.4) - 0.055;
    };

    return [gamma(r), gamma(g), gamma(bl)].map((c) => Math.round(Math.min(1, Math.max(0, c)) * 255));
}

function rgbToHsl(r, g, b) {
    r /= 255; g /= 255; b /= 255;
    const max = Math.max(r, g, b), min = Math.min(r, g, b);
    let h = 0, s = 0;
    const l = (max + min) / 2;
    const d = max - min;
    if (d !== 0) {
        s = d / (1 - Math.abs(2 * l - 1));
        switch (max) {
            case r: h = ((g - b) / d) % 6; break;
            case g: h = (b - r) / d + 2; break;
            case b: h = (r - g) / d + 4; break;
        }
        h *= 60;
        if (h < 0) h += 360;
    }
    return [Math.round(h * 100) / 100, Math.round(s * 1000) / 10, Math.round(l * 1000) / 10];
}

function hsl(name, Lpct, C, H) {
    const [r, g, b] = oklchToSrgb(Lpct / 100, C, H);
    const [h, s, l] = rgbToHsl(r, g, b);
    console.log(`${name.padEnd(28)} oklch(${Lpct}% ${C} ${H}) -> ${h} ${s}% ${l}%   (rgb ${r},${g},${b})`);
}

console.log("=== LIGHT ===");
hsl("Primary/Focus/Accent", 62.04, 0.1950, 253.83);
hsl("PrimaryForeground", 99.11, 0, 0);
hsl("Background", 97.02, 0.0015, 253.83);
hsl("Danger", 65.32, 0.2335, 25.74);
hsl("DangerForeground", 99.11, 0, 0);
hsl("Default", 94.00, 0.0015, 253.83);
hsl("DefaultForeground", 21.03, 0.0059, 253.83);
hsl("Foreground", 21.03, 0.0015, 253.83);
hsl("Overlay", 100.00, 0.0004, 253.83);
hsl("Divider(separator)", 92.00, 0.0015, 253.83);
hsl("Success", 73.29, 0.1941, 150.81);
hsl("SuccessForeground", 21.03, 0.0059, 150.81);
hsl("Content1(surface)", 100.00, 0.0008, 253.83);
hsl("Content2(surface-2)", 95.24, 0.0012, 253.83);
hsl("Content3(surface-3)", 93.73, 0.0012, 253.83);
hsl("Content4(extrapolated)", 91.50, 0.0012, 253.83);
hsl("Warning", 78.19, 0.1590, 72.33);
hsl("WarningForeground", 21.03, 0.0059, 72.33);

console.log("\n=== DARK ===");
hsl("Primary/Focus/Accent", 62.04, 0.1950, 253.83);
hsl("PrimaryForeground", 99.11, 0, 0);
hsl("Background", 12.00, 0.0015, 253.83);
hsl("Danger", 59.40, 0.1973, 24.63);
hsl("DangerForeground", 99.11, 0, 0);
hsl("Default", 27.40, 0.0015, 253.83);
hsl("DefaultForeground", 99.11, 0, 0);
hsl("Foreground", 99.11, 0.0015, 253.83);
hsl("Overlay", 21.03, 0.0030, 253.83);
hsl("Divider(separator)", 25.00, 0.0015, 253.83);
hsl("Success", 73.29, 0.1941, 150.81);
hsl("SuccessForeground", 21.03, 0.0059, 150.81);
hsl("Content1(surface)", 21.03, 0.0030, 253.83);
hsl("Content2(surface-2)", 25.70, 0.0023, 253.83);
hsl("Content3(surface-3)", 27.21, 0.0023, 253.83);
hsl("Content4(extrapolated)", 30.50, 0.0023, 253.83);
hsl("Warning", 82.03, 0.1392, 76.34);
hsl("WarningForeground", 21.03, 0.0059, 76.34);

console.log("\n=== SECONDARY (не входит в токены HeroUI — берём аналог по методологии) ===");
hsl("Secondary Light", 60, 0.19, 310);
hsl("SecondaryForeground Light", 99.11, 0, 0);
hsl("Secondary Dark", 65, 0.19, 310);
hsl("SecondaryForeground Dark", 99.11, 0, 0);
