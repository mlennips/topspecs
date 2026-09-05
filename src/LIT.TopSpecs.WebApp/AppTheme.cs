using MudBlazor;

namespace LIT.TopSpecs.WebApp;

public static class AppTheme
{
    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight()
        {
            // LIGHT (Aspire-inspiriert, aber hell)
            Primary = "#7A5AF8",         // Aspire-Violett
            Secondary = "#647087",       // gedämpftes Blaugrau
            Tertiary = "#71717A",
            Background = "#FAFAFA",
            Surface = "#FFFFFF",
            AppbarBackground = "#1B1A19",
            AppbarText = "#F3F2F1",
            DrawerBackground = "#1B1A19",
            DrawerText = "#F3F2F1",
            DrawerIcon = "#C8C6C4",
            TextPrimary = "#201F1E",
            TextSecondary = "#605E5C",
            LinesDefault = "#E1DFDD",
            TableLines = "#E1DFDD",
            Divider = "#E1DFDD",
            Success = "#4C9A6A",
            Error = "#C7524A",
            Warning = "#C0863A",
            Info = "#4A7FA8",
        },

        PaletteDark = new PaletteDark()
        {
            // DARK (Aspire-Dashboard-Look): einheitliches, sehr dunkles Blau ueber AppBar, Drawer und Main-Bereich
            Primary = "#9B7BFF",         // helles Violett als Akzent, wie im Aspire-Dashboard
            Secondary = "#8A96AC",
            Tertiary = "#94A3B8",
            Background = "#221e2d",      // sehr dunkles Blau, durchgaengig fuer Main-Bereich
            Surface = "#1B2140",         // etwas hellere Flaeche fuer Cards/Paper
            AppbarBackground = "#111",
            AppbarText = "#F3F2F1",
            DrawerBackground = "#111",
            DrawerText = "#F3F2F1",
            DrawerIcon = "#C8C6C4",
            TextPrimary = "#F3F2F1",
            TextSecondary = "#C8C6C4",
            LinesDefault = "#262B4A",    // dezente, blau abgestimmte Trennlinie statt Grau
            TableLines = "#262B4A",
            Divider = "#262B4A",
            Success = "#6EAF88",
            Error = "#D3766F",
            Warning = "#D1A15E",
            Info = "#6D9CC0",
        },

        Typography = new Typography()
        {
            Default = new DefaultTypography()
            {
                FontFamily = ["Segoe UI", "-apple-system", "BlinkMacSystemFont", "Roboto", "Helvetica Neue", "sans-serif"]
            }
        },

        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "4px"
        }
    };
}
