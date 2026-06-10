using System;

namespace AgriSmart.Web.Services
{
    public class AppState
    {
        public bool IsSidebarOpen { get; private set; }
        
        public string CurrentLanguage { get; set; } = "en";

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

        public void SetLanguage(string lang)
        {
            if (lang == "en" || lang == "ur")
            {
                CurrentLanguage = lang;
                NotifyStateChanged();
            }
        }

        public string T(string en, string ur)
        {
            return CurrentLanguage == "ur" ? ur : en;
        }

        public bool IsUrdu => CurrentLanguage == "ur";

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
