using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
	// https://github.com/madskristensen/MultiEdit/blob/master/src/Editor/MultiPointEditCommandFilter.cs

	class AutocompleteCommandFilter : IOleCommandTarget
	{
		private IOleCommandTarget NextTarget;

		private IWpfTextView View;

		private AutocompleteCommandFilter(IVsTextView viewAdapter, IWpfTextView textView)
		{
			int result = viewAdapter.AddCommandFilter(this, out NextTarget);
			if (result != VSConstants.S_OK)
				throw new Exception($"Error adding command filter, code {result}");

			View = textView;
		}

		static public void AttachCommandFiter(IVsTextView viewAdapter, IWpfTextView textView) =>
			textView.Properties.GetOrCreateSingletonProperty(() => new AutocompleteCommandFilter(viewAdapter, textView));

		public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			// Debug.WriteLine(nCmdID);

			if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
			{
				switch ((VSConstants.VSStd2KCmdID)nCmdID)
				{
					case VSConstants.VSStd2KCmdID.TAB:
						Debug.WriteLine("Tab");
						//return VSConstants.S_OK;
						break;

					case VSConstants.VSStd2KCmdID.OPENLINEABOVE:
						// Ctrl + Enter
						Debug.WriteLine("OPENLINEABOVE");
						if(InsertCurrentSuggestion())
							return VSConstants.S_OK;
						break;
				}
			}

			return NextTarget?.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut) ?? VSConstants.S_OK;
		}

		private bool InsertCurrentSuggestion()
		{
			ViewAutocompleteHandler view = ViewAutocompleteHandler.AttachedHandler(View);
			return view.AddCurrentSuggestionToView();
		}

		public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
		{
			return NextTarget?.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText) ?? VSConstants.S_OK;
		}
	}
}
