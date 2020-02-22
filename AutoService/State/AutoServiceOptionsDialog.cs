using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoService.State
{
    [Guid("BE905985-26BB-492B-9453-743E26F4E8BB")]
    public class AutoServiceOptionsDialog : DialogPage
    {
        public const string Category = "AutoService";
        public const string SubCategory = "General";

        private const string ServicesSubCategory = "Services";

        [Category(SubCategory)]
        [DisplayName("Show start page")]
        [Description("Show start pages Visual Studio starts.")]
        public bool ShowStartPage { get; set; }

        [Category(ServicesSubCategory)]
        [DisplayName("Services")]
        [Description("Specify services to start or stop when Visual Studio starts.")]
        public ServiceEntry[] Services { get; set; }

        public override void LoadSettingsFromStorage()
        {
            var settings = Settings.Load();
            Services = settings.Services;
            ShowStartPage = settings.ShowStartPage;
        }

        public override void SaveSettingsToStorage()
        {
            var settings = new Settings
            {
                Services = Services,
                ShowStartPage = ShowStartPage
            };
            settings.Save();
        }
    }
}
