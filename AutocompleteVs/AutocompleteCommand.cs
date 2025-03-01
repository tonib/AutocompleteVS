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

		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0100;

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
		private AutocompleteCommand(AsyncPackage package, OleMenuCommandService commandService)
		{
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			var menuCommandID = new CommandID(CommandSet, CommandId);
			var menuItem = new MenuCommand(this.Execute, menuCommandID);
			
			commandService.AddCommand(menuItem);
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static AutocompleteCommand Instance
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
		{
			get
			{
				return this.package;
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static async Task InitializeAsync(AsyncPackage package)
		{
			// Switch to the main thread - the call to AddCommand in AutocompleteCommand's constructor requires
			// the UI thread.
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			Instance = new AutocompleteCommand(package, commandService);
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
			ThreadHelper.ThrowIfNotOnUIThread();
			//string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
			//string title = "AutocompleteCommand";

			//// Show a message box to prove we were here
			//VsShellUtilities.ShowMessageBox(
			//	this.package,
			//	message,
			//	title,
			//	OLEMSGICON.OLEMSGICON_INFO,
			//	OLEMSGBUTTON.OLEMSGBUTTON_OK,
			//	OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

			Microsoft.VisualStudio.Text.Editor.IWpfTextView view = TextEditorUtils.GetTextView(package);
			if (view == null)
				return;

			// TODO: This only for models allowing fill in the middle
			// Get prefix / suffix text
			int caretIdx = view.Caret.Position.BufferPosition;
			string prefixText = view.TextBuffer.CurrentSnapshot.GetText(0, caretIdx);
			string suffixText;
			int textLength = view.TextBuffer.CurrentSnapshot.Length;
			if (caretIdx >= textLength)
				suffixText = "";
			else
				suffixText = view.TextBuffer.CurrentSnapshot.GetText(caretIdx, view.TextBuffer.CurrentSnapshot.Length - caretIdx);

			IChatClient x = null;
			// _ = Generation.TestAsync();

			// Call the model, do not wait
			_ = DoRequestAsync(view, prefixText, suffixText);
		}

		/*async private Task TestAsync()
		{
			IChatClient x = null;
			if(x != null)
				var y = await x.GetStreamingResponseAsync(null);

			//IAsyncEnumerable<int> tuputamadre = null;
			//IAsyncEnumerator<int> x = tuputamadre?.GetAsyncEnumerator();
			//if(x != null)
			//	_ = await x.MoveNextAsync();
		}*/

		async private static Task DoRequestAsync(Microsoft.VisualStudio.Text.Editor.IWpfTextView view, string prefixText, string suffixText)
		{
			try
			{
				// set up the client
				var uri = new Uri("http://localhost:11434");
				var ollama = new OllamaApiClient(uri);

				// select a model which should be used for further operations
				ollama.SelectedModel = "qwen2.5-coder:1.5b-base";

				var request = new GenerateRequest();
				request.Prompt = prefixText;
				request.Suffix = suffixText;

				Debug.WriteLine("---------------");
				string autocompleteText = "";
				var enumerator = ollama.GenerateAsync(request).GetAsyncEnumerator();
				while (await enumerator.MoveNextAsync())
				{
					string newToken = enumerator.Current.Response;
					Debug.Write(newToken);
					autocompleteText += newToken;
				}
				Debug.WriteLine("---------------");

				ViewAutocompleteHandler handler = ViewAutocompleteHandler.AttachedHandler(view);
				handler.AddAutocompletionAdornment(autocompleteText);

			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}

		/*
			*/
	}
}
