namespace LIT.TopSpecs.WebApp;

public sealed class ThemeState
{
    public bool IsDarkMode { get; private set; } = true;

    public event Action? Changed;

    public void Toggle()
    {
        IsDarkMode = !IsDarkMode;
        Changed?.Invoke();
    }
}
