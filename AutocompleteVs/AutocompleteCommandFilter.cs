using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
	// https://github.com/madskristensen/MultiEdit/blob/master/src/Editor/MultiPointEditCommandFilter.cs

	/// <summary>
	/// Handles / intercepts commands in text editor
	/// </summary>
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
			try
			{
				// Debug.WriteLine(nCmdID);

				if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
				{
					switch ((VSConstants.VSStd2KCmdID)nCmdID)
					{
						case VSConstants.VSStd2KCmdID.TYPECHAR:
                            // Character typed in editor: Cancel running generations, remove current suggestion
                            // Now handled by ViewAutocompleteHandler.TextBuffer_Changed
                            //_ = AutocompletionGeneration.Instance.CancelCurrentGenerationAsync();
                            //ViewAutocompleteHandler.AttachedHandler(View).RemoveAdornment();
       //                     char typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
							//ViewAutocompleteHandler.AttachedHandler(View).CharTyped(typedChar);
                            break;

						case VSConstants.VSStd2KCmdID.OPENLINEABOVE:
							// Ctrl + Enter: Add suggestion
							// Debug.WriteLine("OPENLINEABOVE");
							if (ViewAutocompleteHandler.AddCurrentSuggestionToView(false))
							{
								// Suggestion added: Command has been consumed
								return VSConstants.S_OK;
							}
							break;

						case VSConstants.VSStd2KCmdID.WORDNEXT:
							// Ctrl + >: Autocomplete next word only
							if (ViewAutocompleteHandler.AddCurrentSuggestionToView(true))
							{
								// Suggestion added: Command has been consumed
								return VSConstants.S_OK;
							}
							break;

						case VSConstants.VSStd2KCmdID.CANCEL:
							// Esc: Cancel current suggestion / generation
							var handler = ViewAutocompleteHandler.AttachedHandler(View);
							// If there is a VS intellisense session active, do nothing. Esc will cancel that session
							// Do not cancell the suggestion
							if (!handler.CompletionBroker.IsCompletionActive(View))
								handler.CancelCurrentAutocompletion();
							break;

					}
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
			return NextTarget?.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut) ?? VSConstants.S_OK;
		}

		private ViewAutocompleteHandler ViewAutocompleteHandler => ViewAutocompleteHandler.AttachedHandler(View);

		public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
		{
			return NextTarget?.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText) ?? VSConstants.S_OK;
		}
	}
}
