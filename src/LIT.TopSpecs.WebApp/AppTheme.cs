using MudBlazor;

namespace LIT.TopSpecs.WebApp;

public static class AppTheme
{
    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight()
        {
            // LIGHT
            Primary = "#5B63A6",         // dezentes Indigo/Blau statt grell
            Secondary = "#647087",       // gedämpftes Blaugrau
            Tertiary = "#71717A",
            Background = "#FFFFFF",     // sehr helles Grau
            Surface = "#FFFFFF",
            AppbarBackground = "#111",
            AppbarText = "#eee",      // sorgt für Kontrast in der Navbar
            DrawerBackground = "#111", // dunkle Sidebar
            DrawerText = "#fff",
            DrawerIcon = "#9CA3AF",
            TextPrimary = "#111827",
            TextSecondary = "#6B7280",
            LinesDefault = "#E5E7EB",
            TableLines = "#E5E7EB",
            Divider = "#E5E7EB",
            Success = "#4C9A6A",
            Error = "#C7524A",
            Warning = "#C0863A",
            Info = "#4A7FA8",
        },

        PaletteDark = new PaletteDark()
        {
            // DARK
            Primary = "#8B92C9",         // dezentes, helleres Indigo für Dark-Mode
            Secondary = "#8A96AC",
            Tertiary = "#94A3B8",
            Background = "#221e2d",      // sehr dunkles Blau/Schwarz
            Surface = "#333",
            AppbarBackground = "#000",
            AppbarText = "#E5E7EB",
            DrawerBackground = "#000",
            DrawerText = "#fff",
            DrawerIcon = "#9CA3AF",
            TextPrimary = "#F9FAFB",
            TextSecondary = "#9CA3AF",
            LinesDefault = "#1F2937",
            TableLines = "#1F2937",
            Divider = "#1F2937",
            Success = "#6EAF88",
            Error = "#D3766F",
            Warning = "#D1A15E",
            Info = "#6D9CC0",
        },

        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "10px"
        }
    };
}
