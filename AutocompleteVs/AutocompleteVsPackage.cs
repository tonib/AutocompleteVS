using AutocompleteVs.Config;
using AutocompleteVs.Logging;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace AutocompleteVs
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    // Check ProvideAutoLoad: Needed to load the package at start (package settings are needed for suggestions generation).
    // TODO: Package could wait to load until a text/code editor is open, is not needed at VS startup time
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[Guid(AutocompleteVsPackage.PackageGuidString)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(Settings), Settings.PageCategory, Settings.PageCategory, 0, 0, true)]
    public sealed class AutocompleteVsPackage : AsyncPackage
	{
		/// <summary>
		/// AutocompleteVsPackage GUID string.
		/// </summary>
		public const string PackageGuidString = "05fd51b7-1fda-4eda-b9f9-d12f95255b5b";

		/// <summary>
		/// The package instance. It will be null until extension is initialized.
		/// </summary>
		static public AutocompleteVsPackage Instance { get; private set; }

		/// <summary>
		/// This instance, casted as IServiceProvider. TODO: I don't know why this instance does not implement IServiceProvider. Because is async?
		/// </summary>
		public IServiceProvider ServiceProvider { get; private set; }

		/// <summary>
		/// Visual Studio instance
		/// </summary>
		public DTE DTE { get; private set; }

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
		/// <param name="progress">A provider for progress updates.</param>
		/// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			// When initialized asynchronously, the current thread may be a background thread at this point.
			// Do any initialization that requires the UI thread after switching to the UI thread.
			await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
		    await AutocompleteCommand.InitializeAsync(this);

			Instance = this;
			ServiceProvider = (IServiceProvider)this;
			DTE = await GetServiceAsync(typeof(DTE)) as DTE;

			OutputPaneHandler.Instance.LogLevel = Settings.LogLevel;
		}

		/// <summary>
		/// Package settings
		/// </summary>
        internal Settings Settings => (Settings)GetDialogPage(typeof(Settings));

        /// <summary>
        /// Shows a message box
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="title">Window title</param>
        /// <param name="icon">Icon to show</param>
        public void MessageBox(string message, string title, Microsoft.VisualStudio.Shell.Interop.OLEMSGICON icon)
        {
            VsShellUtilities.ShowMessageBox(ServiceProvider, message, title, icon,
                Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK,
                Microsoft.VisualStudio.Shell.Interop.OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        /// <summary>
        /// Get the VS status bar (only from the UI thread)
        /// </summary>
        public IVsStatusbar GetStatusBar()
		{
            ThreadHelper.ThrowIfNotOnUIThread();
            return (IVsStatusbar)ServiceProvider.GetService(typeof(SVsStatusbar));
		}

        /// <summary>
        /// Get the VS status bar
        /// </summary>
        public async Task<IVsStatusbar> GetStatusBarAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
			return GetStatusBar();
        }
    }
}
