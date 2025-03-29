using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutocompleteVs
{
	/// <summary>
	/// Handle output pane for logging
	/// </summary>
	class OutputPaneHandler
	{
		private static Guid PaneGuid = new Guid("7EAF8615-355D-4F1C-8B75-CE67360A4BB7");
		private const string PaneTitle = "AutompleteVS";

		private IVsOutputWindowPane Output;

		static private OutputPaneHandler _Instance;
		static public OutputPaneHandler Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new OutputPaneHandler();
				}
				return _Instance;
			}
		}

		private OutputPaneHandler()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			// Create pane for output messages
			// https://learn.microsoft.com/en-us/visualstudio/extensibility/extending-the-output-window?view=vs-2019
			IVsOutputWindow outputWindow = (IVsOutputWindow)AutocompleteVsPackage.Instance.ServiceProvider
				.GetService(typeof(SVsOutputWindow));

			outputWindow.GetPane(ref PaneGuid, out Output);
			if (Output == null)
			{
				// This should throw an exception, but it will not be logged anywhere
				return;
			}

			// Create a new pane.
			outputWindow.CreatePane(
				ref PaneGuid,
				PaneTitle,
				Convert.ToInt32(true),
				Convert.ToInt32(true));

			// Retrieve the new pane.
			outputWindow.GetPane(ref PaneGuid, out Output);
		}

		public void ShowOutputWindow()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			// Make output window visible (https://stackoverflow.com/questions/36680335/dock-toolwindowpane-at-the-same-location-as-the-output-and-error-list-windows)
			EnvDTE.Window win = AutocompleteVsPackage.Instance.DTE.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
			win.Visible = true;
		}

		public void Log(string line)
		{
			try
			{
				ThreadHelper.ThrowIfNotOnUIThread();
				Output.OutputStringThreadSafe($"[{DateTime.Now}] {line}\n");
			}
			catch (Exception ex)
			{
				// Not loggable
				Debug.WriteLine(ex.ToString());
			}
		}

		async public System.Threading.Tasks.Task LogAsync(string line)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
			Log(line);
		}

		public void Log(Exception ex)
		{
			Log(ex.ToString());
			// ShowOutputWindow();
		}

		async public System.Threading.Tasks.Task LogAsync(Exception ex)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
			Log(ex);
		}
	}
}
