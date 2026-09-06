namespace LIT.TopSpecs.WebApp.States;

public sealed class ThemeState
{
    public bool IsDarkMode { get; private set; }

    public event Action? Changed;

    public void Toggle()
    {
        IsDarkMode = !IsDarkMode;
        Changed?.Invoke();
    }
}
