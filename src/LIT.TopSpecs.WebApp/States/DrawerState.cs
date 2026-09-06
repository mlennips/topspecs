using MudBlazor;

namespace LIT.TopSpecs.WebApp.States
{
    public sealed class DrawerState
    {
        public bool IsOpen { get; set; }
        public bool IsEnabled { get; set; } = true;
        public DrawerClipMode DrawerClipMode { get; internal set; } = DrawerClipMode.Always;
        public DrawerVariant DrawerVariant { get; internal set; } = DrawerVariant.Mini;

        public DrawerState()
        {

        }

        public void Toggle()
        {
            IsOpen = !IsOpen;
        }

        private void Enable() 
        { 
            IsEnabled = true;
        }

        private void Disable()
        {
            IsEnabled = false;
        }
    }
}
