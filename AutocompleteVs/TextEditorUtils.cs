using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
	class TextEditorUtils
	{
		// For code to get current editor, see:
		// https://stackoverflow.com/questions/37469869/get-roslyn-syntaxtoken-from-visual-studio-text-selection-caret-position
		// https://vlasovstudio.com/visual-commander/commands.html#CreateTypedVariable

		static public Microsoft.VisualStudio.Text.Editor.IWpfTextView GetTextView(AsyncPackage package)
		{
			var textManager = package.GetService<
					Microsoft.VisualStudio.TextManager.Interop.SVsTextManager,
					Microsoft.VisualStudio.TextManager.Interop.IVsTextManager>();

			Microsoft.VisualStudio.TextManager.Interop.IVsTextView textView;
			textManager.GetActiveView(1, null, out textView);
			return GetEditorAdaptersFactoryService(package).GetWpfTextView(textView);
}

		static private Microsoft.VisualStudio.Editor.IVsEditorAdaptersFactoryService GetEditorAdaptersFactoryService(AsyncPackage package)
		{
			var componentModel = package.GetService<
					Microsoft.VisualStudio.ComponentModelHost.SComponentModel,
					Microsoft.VisualStudio.ComponentModelHost.IComponentModel>();
			return componentModel.GetService<Microsoft.VisualStudio.Editor.IVsEditorAdaptersFactoryService>();
		}
	}
}
