using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using AutoService.State;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace AutoService
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string, PackageAutoLoadFlags.BackgroundLoad)]
    [Guid(PackageGuidString)]
    [ProvideOptionPage(typeof(AutoServiceOptionsDialog), AutoServiceOptionsDialog.Category, AutoServiceOptionsDialog.SubCategory, 1000, 1001, true)]
    [ProvideProfile(typeof(AutoServiceOptionsDialog), AutoServiceOptionsDialog.Category, AutoServiceOptionsDialog.SubCategory, 1000, 1001, true)]
    [InstalledProductRegistration(AssemblyInfo.Product, AssemblyInfo.Description, AssemblyInfo.Version)]
    public sealed class AutoServicePackage : AsyncPackage
    {
        /// <summary>
        /// AutoServicePackage GUID string.
        /// </summary>
        public const string PackageGuidString = "855bcce1-94f1-4098-a784-28b041ba9cfb";

        private Settings settings;

        public AutoServicePackage()
        {
            settings = Settings.Load();
            Settings.SettingsUpdated += OnSettingsUpdated;
        }

        private void OnSettingsUpdated(object sender, EventArgs e)
        {
            settings = (Settings)sender;
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            foreach (var entry in settings.Services)
            {
                _ = Task.Run(() =>
                  {
                      ServiceEntry service = entry;
                      ServiceController serviceController = new ServiceController(service.Name);
                      switch (service.Action)
                      {
                          case ServiceAction.Start:
                              if (serviceController.Status == ServiceControllerStatus.Stopped)
                                  serviceController.Start();
                              break;
                          case ServiceAction.Stop:
                              if (serviceController.Status == ServiceControllerStatus.Running)
                                  serviceController.Stop();
                              break;
                      }
                      serviceController.Close();
                  });
            }

            if (settings.ShowStartPage)
            _ = Task.Run(() =>
            {
                var dte = GetService(typeof(DTE)) as DTE2;
                dte.ExecuteCommand("File.StartWindow");
            });
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Settings.SettingsUpdated -= OnSettingsUpdated;

            foreach (var entry in settings.Services)
            {
                _ = Task.Run(() =>
                {
                    ServiceEntry service = entry;
                    ServiceController serviceController = new ServiceController(service.Name);
                    switch (service.Action)
                    {
                        case ServiceAction.Start:
                            if (serviceController.Status == ServiceControllerStatus.Running)
                                serviceController.Stop();
                            break;
                        case ServiceAction.Stop:
                            if (serviceController.Status == ServiceControllerStatus.Stopped)
                                serviceController.Start();
                            break;
                    }
                    serviceController.Close();
                });
            }
        }

        #endregion
    }
}
