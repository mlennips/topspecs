using MudBlazor;

namespace LIT.TopSpecs.WebApp;

public static class AppTheme
{
    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#d58526",            
            Secondary = "#475e7d",
            Tertiary = "#6B8F6B",
            Background = "#E4E3E0",
            Surface = "#F2F1EE",
            AppbarBackground = "#4A4A48",
            AppbarText = "#E4E3E0",
            DrawerBackground = "#4A4A48",
            DrawerText = "#E4E3E0",
            DrawerIcon = "#9FB2CC",
            TextPrimary = "#333333",
            TextSecondary = "#5C5A56",
            LinesDefault = "#CFCDC8",
            TableLines = "#CFCDC8",
            Divider = "#CFCDC8",
            Success = "#6B8F6B",
            Error = "#A65C52",
            Warning = "#C9A227",
            Info = "#4B607F",
        },

        PaletteDark = new PaletteDark()
        {
            Primary = "#354459",
            Secondary = "#C07A6E",
            Tertiary = "#85AD85",
            Background = "#222222",
            Surface = "#2F2E2C",
            AppbarBackground = "#1A1A19",
            AppbarText = "#E4E3E0",
            DrawerBackground = "#1A1A19",
            DrawerText = "#E4E3E0",
            DrawerIcon = "#6D84A3",
            TextPrimary = "#E4E3E0",
            TextSecondary = "#B0AEA9",
            LinesDefault = "#454340",
            TableLines = "#454340",
            Divider = "#454340",
            Success = "#85AD85",
            Error = "#C07A6E",
            Warning = "#D9B04A",
            Info = "#6D84A3",
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
            DefaultBorderRadius = "6px"
        }
    };
}