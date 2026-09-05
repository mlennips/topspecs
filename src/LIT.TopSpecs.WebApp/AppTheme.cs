using MudBlazor;

namespace LIT.TopSpecs.WebApp;

public static class AppTheme
{
    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2563eb",
            Secondary = "#0ea5e9",
            Tertiary = "#64748b",
            Success = "#16a34a",
            Warning = "#d97706",
            Error = "#dc2626",
            Info = "#0284c7",

            Background = "#f4f5f7",
            Surface = "#ffffff",
            AppbarBackground = "#ffffff",
            AppbarText = "#1e293b",
            DrawerBackground = "#222",
            DrawerText = "#e2e8f0",
            DrawerIcon = "#cbd5e1",

            TextPrimary = "#1e293b",
            TextSecondary = "#64748b",
            LinesDefault = "#e2e8f0",
            TableLines = "#e2e8f0",
            Divider = "#e2e8f0",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#3b82f6",
            Secondary = "#38bdf8",
            Tertiary = "#94a3b8",
            Success = "#22c55e",
            Warning = "#f59e0b",
            Error = "#ef4444",
            Info = "#38bdf8",

            Background = "#0f172a",
            Surface = "#1e293b",
            AppbarBackground = "#111",
            AppbarText = "#e2e8f0",
            DrawerBackground = "#222",
            DrawerText = "#e2e8f0",
            DrawerIcon = "#cbd5e1",

            TextPrimary = "#e2e8f0",
            TextSecondary = "#94a3b8",
            LinesDefault = "#334155",
            TableLines = "#334155",
            Divider = "#334155",
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Inter", "Roboto", "Helvetica", "Arial", "sans-serif"],
                FontWeight = "400",
            },
            H1 = new H1Typography { FontFamily = ["Inter", "Roboto", "sans-serif"], FontWeight = "500", FontSize = "2.25rem" },
            H2 = new H2Typography { FontFamily = ["Inter", "Roboto", "sans-serif"], FontWeight = "500", FontSize = "1.875rem" },
            H3 = new H3Typography { FontFamily = ["Inter", "Roboto", "sans-serif"], FontWeight = "500", FontSize = "1.5rem" },
            H4 = new H4Typography { FontFamily = ["Inter", "Roboto", "sans-serif"], FontWeight = "500", FontSize = "1.25rem" },
            H5 = new H5Typography { FontFamily = ["Inter", "Roboto", "sans-serif"], FontWeight = "500", FontSize = "1.125rem" },
            H6 = new H6Typography { FontFamily = ["Inter", "Roboto", "sans-serif"], FontWeight = "500", FontSize = "1rem" },
            Button = new ButtonTypography { FontWeight = "500", FontSize = "0.875rem" },
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "6px",
        },
    };
}
