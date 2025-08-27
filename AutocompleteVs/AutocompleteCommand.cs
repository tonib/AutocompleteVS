using AutocompleteVs.Logging;
using Microsoft.Extensions.AI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace AutocompleteVs
{

	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class AutocompleteCommand
	{
		// TODO: Enable command only in text editors
		// TODO: Add key binding to command (https://learn.microsoft.com/en-us/visualstudio/extensibility/binding-keyboard-shortcuts-to-menu-items?view=vs-2022)

		public const int AutcompleteConfig0CommandId = 0x0100,
            AutcompleteConfig1CommandId = 0x0101,
            AutcompleteConfig2CommandId = 0x0102;

        /// <summary>
        /// Command ID.
        /// </summary>
        private int CommandId;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("6f75977d-a6b8-446e-92ad-c986456617f5");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly AsyncPackage package;

		/// <summary>
		/// Initializes a new instance of the <see cref="AutocompleteCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		/// <param name="commandService">Command service to add command to, not null.</param>
		private AutocompleteCommand(AsyncPackage package, OleMenuCommandService commandService,
			int commandId)
		{
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            CommandId = commandId;
            var menuCommandID = new CommandID(CommandSet, CommandId);
			var menuItem = new MenuCommand(this.Execute, menuCommandID);
			
			commandService.AddCommand(menuItem);
		}

        /// <summary>
        /// Command instances. Index relates to the AutocompleteConfig index in 
        /// Settings.AutocompletionConfigurations
        /// </summary>
        public static List<AutocompleteCommand> Instances = new List<AutocompleteCommand>();

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static async Task InitializeAsync(AsyncPackage package, int commandId)
		{
			// Switch to the main thread - the call to AddCommand in AutocompleteCommand's constructor requires
			// the UI thread.
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			Instances.Add(new AutocompleteCommand(package, commandService, commandId));
		}

		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void Execute(object sender, EventArgs e)
		{
			try
			{
				ThreadHelper.ThrowIfNotOnUIThread();

				Microsoft.VisualStudio.Text.Editor.IWpfTextView view = TextEditorUtils.GetTextView(package);
				if (view == null)
					return;

				ViewAutocompleteHandler.AttachedHandler(view).StartGeneration();
			}
			catch(Exception ex)
			{
				OutputPaneHandler.Instance.Log(ex);
			}
		}

	}
}
