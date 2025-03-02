using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.Commanding;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutocompleteVs.Keyboard
{
	// https://learn.microsoft.com/en-us/visualstudio/extensibility/walkthrough-using-a-shortcut-key-with-an-editor-extension?view=vs-2022

	// TODO: Not working, remove this
	/*
	[Export(typeof(ICommandHandler))]
	[ContentType(VsTextViewListener.CONTENT_TYPE_ID)]
	[Name("AutocompleteCommandHandler")]
	internal class AutocompleteCommandHandler : ICommandHandler<DownKeyCommandArgs>
	{
		public string DisplayName => "AutocompleteVs";

		public CommandState GetCommandState(DownKeyCommandArgs args) => CommandState.Unspecified;

		public bool ExecuteCommand(DownKeyCommandArgs args, CommandExecutionContext executionContext)
		{
			//if (args.TypedChar == '+')
			//{
			//	bool alreadyAdorned = args.TextView.Properties.TryGetProperty(
			//		"KeyBindingTextAdorned", out bool adorned) && adorned;
			//	if (!alreadyAdorned)
			//	{
			//		new PurpleCornerBox((IWpfTextView)args.TextView);
			//		args.TextView.Properties.AddProperty("KeyBindingTextAdorned", true);
			//	}
			//}

			//ModifierKeys modifiers = System.Windows.Input.Keyboard.Modifiers;
			//Debug.WriteLine(modifiers + " " + args.TypedChar);
			Debug.WriteLine(args);

			return false;
		}
	}*/

}
