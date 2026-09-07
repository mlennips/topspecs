using MudBlazor;

namespace LIT.TopSpecs.WebApp.Shared;

/// <summary>
/// Verfügbare Icon-Stile der MudBlazor/Material-Icon-Bibliothek.
/// </summary>
public enum IconVariant
{
    Filled,
    Outlined,
    Rounded,
    Sharp,
    TwoTone,
}

/// <summary>
/// Zentrale Zuordnung von Icons zu fachlichen Entitäten/Konzepten, damit dieselbe
/// Entität in der gesamten UI konsistent dargestellt wird. Für jede Entität kann
/// wahlweise der Icon-Stil (<see cref="IconVariant"/>) gewählt werden.
/// </summary>
public static class IconHelper
{
    public static string Asset(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.Inventory2,
        IconVariant.Rounded => Icons.Material.Rounded.Inventory2,
        IconVariant.Sharp => Icons.Material.Sharp.Inventory2,
        IconVariant.TwoTone => Icons.Material.TwoTone.Inventory2,
        _ => Icons.Material.Filled.Inventory2,
    };

    public static string Component(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.Build,
        IconVariant.Rounded => Icons.Material.Rounded.Build,
        IconVariant.Sharp => Icons.Material.Sharp.Build,
        IconVariant.TwoTone => Icons.Material.TwoTone.Build,
        _ => Icons.Material.Filled.Build,
    };

    public static string Bubble(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.BubbleChart,
        IconVariant.Rounded => Icons.Material.Rounded.BubbleChart,
        IconVariant.Sharp => Icons.Material.Sharp.BubbleChart,
        IconVariant.TwoTone => Icons.Material.TwoTone.BubbleChart,
        _ => Icons.Material.Filled.BubbleChart,
    };

    public static string ShareLink(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.Share,
        IconVariant.Rounded => Icons.Material.Rounded.Share,
        IconVariant.Sharp => Icons.Material.Sharp.Share,
        IconVariant.TwoTone => Icons.Material.TwoTone.Share,
        _ => Icons.Material.Filled.Share,
    };

    public static string Journal(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.History,
        IconVariant.Rounded => Icons.Material.Rounded.History,
        IconVariant.Sharp => Icons.Material.Sharp.History,
        IconVariant.TwoTone => Icons.Material.TwoTone.History,
        _ => Icons.Material.Filled.History,
    };

    public static string Attachment(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.AttachFile,
        IconVariant.Rounded => Icons.Material.Rounded.AttachFile,
        IconVariant.Sharp => Icons.Material.Sharp.AttachFile,
        IconVariant.TwoTone => Icons.Material.TwoTone.AttachFile,
        _ => Icons.Material.Filled.AttachFile,
    };

    public static string AssetRelationship(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.AccountTree,
        IconVariant.Rounded => Icons.Material.Rounded.AccountTree,
        IconVariant.Sharp => Icons.Material.Sharp.AccountTree,
        IconVariant.TwoTone => Icons.Material.TwoTone.AccountTree,
        _ => Icons.Material.Filled.AccountTree,
    };

    public static string Template(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.ContentCopy,
        IconVariant.Rounded => Icons.Material.Rounded.ContentCopy,
        IconVariant.Sharp => Icons.Material.Sharp.ContentCopy,
        IconVariant.TwoTone => Icons.Material.TwoTone.ContentCopy,
        _ => Icons.Material.Filled.ContentCopy,
    };

    public static string AiLink(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.Link,
        IconVariant.Rounded => Icons.Material.Rounded.Link,
        IconVariant.Sharp => Icons.Material.Sharp.Link,
        IconVariant.TwoTone => Icons.Material.TwoTone.Link,
        _ => Icons.Material.Filled.Link,
    };

    public static string McpHub(IconVariant variant = IconVariant.Filled) => variant switch
    {
        IconVariant.Outlined => Icons.Material.Outlined.Hub,
        IconVariant.Rounded => Icons.Material.Rounded.Hub,
        IconVariant.Sharp => Icons.Material.Sharp.Hub,
        IconVariant.TwoTone => Icons.Material.TwoTone.Hub,
        _ => Icons.Material.Filled.Hub,
    };
}
