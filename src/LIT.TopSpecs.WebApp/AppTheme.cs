using MudBlazor;

namespace LIT.TopSpecs.WebApp;

public static class AppTheme
{
    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#C1652F",
            Secondary = "#2C6E6A",
            Tertiary = "#6B8F6B",
            Background = "#F7F2EA",
            Surface = "#FCFAF6",
            AppbarBackground = "#2B2823",
            AppbarText = "#EFEAE0",
            DrawerBackground = "#2B2823",
            DrawerText = "#EFEAE0",
            DrawerIcon = "#8FBDB8",
            TextPrimary = "#333333",
            TextSecondary = "#5C5A56",
            LinesDefault = "#DCD5C8",
            TableLines = "#DCD5C8",
            Divider = "#DCD5C8",
            Success = "#6B8F6B",
            Error = "#A65C52",
            Warning = "#C9A227",
            Info = "#4B607F",
        },

        PaletteDark = new PaletteDark()
        {
            Primary = "#1F4A47",
            Secondary = "#C07A6E",
            Tertiary = "#85AD85",
            Background = "#211F1B",
            Surface = "#2B2823",
            AppbarBackground = "#1A1A19",
            AppbarText = "#EFEAE0",
            DrawerBackground = "#1A1A19",
            DrawerText = "#EFEAE0",
            DrawerIcon = "#6FA39D",
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
            },
            H1 = new H1Typography() { FontFamily = ["Fraunces", "Georgia", "Times New Roman", "serif"] },
            H2 = new H2Typography() { FontFamily = ["Fraunces", "Georgia", "Times New Roman", "serif"] },
            H3 = new H3Typography() { FontFamily = ["Fraunces", "Georgia", "Times New Roman", "serif"] },
        },

        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "6px"
        }
    };
}