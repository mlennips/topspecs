using MudBlazor;

namespace LIT.TopSpecs.WebApp.States
{
    public sealed class DrawerState
    {
        public bool IsOpen { get; set; }
        public bool IsEnabled { get; private set; }

        public event Action? OnChange;

        public void Toggle()
        {
            IsOpen = !IsOpen;
            OnChange?.Invoke();
        }

        public void Enable()
        {
            IsEnabled = true;
            OnChange?.Invoke();
        }

        public void Disable()
        {
            IsEnabled = false;
            IsOpen = false;
            OnChange?.Invoke();
        }
    }
}
