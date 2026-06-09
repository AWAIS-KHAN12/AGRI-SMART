using System;

namespace AgriSmart.Web.Services
{
    public class AppState
    {
        public bool IsSidebarOpen { get; private set; }

        public event Action OnChange;

        public void ToggleSidebar()
        {
            IsSidebarOpen = !IsSidebarOpen;
            NotifyStateChanged();
        }

        public void CloseSidebar()
        {
            if (IsSidebarOpen)
            {
                IsSidebarOpen = false;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
