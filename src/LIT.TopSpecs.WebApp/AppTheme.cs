using MudBlazor;

namespace LIT.TopSpecs.WebApp;

public static class AppTheme
{
    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#3E5160",
            Secondary = "#BF5C5C",
            Tertiary = "#6B8F6B",
            Background = "#C7C7C7",
            Surface = "#D1D1D1",
            AppbarBackground = "#A6A6A6",
            AppbarText = "#EFEAE0",
            DrawerBackground = "#A6A6A6",
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
            Primary = "#344049",
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