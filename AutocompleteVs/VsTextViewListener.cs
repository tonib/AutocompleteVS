using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
    /// <summary>
    /// Listens for editors creation
    /// </summary>
    [Export(typeof(IVsTextViewCreationListener))]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [ContentType("text")]
    internal sealed class VsTextViewListener : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService = null;

        IWpfTextView WpfTextView;

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            //ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            //if (textView == null)
            //    return;

            //var adornment = textView.Properties.GetProperty<TypingSpeedMeter>(typeof(TypingSpeedMeter));

            //textView.Properties.GetOrCreateSingletonProperty(
            //    () => new TypeCharFilter(textViewAdapter, textView, adornment));

            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            if (textView == null)
                return;

            WpfTextView = (IWpfTextView)textView;

            /*
             * See: https://stackoverflow.com/questions/18268685/visual-studio-extension-keyprocessor-alt-key
             *      https://stackoverflow.com/questions/60261450/how-to-handle-keyboard-events-in-vsix
             * search IWpfTextView key typed event
             */

        }
    }
}
