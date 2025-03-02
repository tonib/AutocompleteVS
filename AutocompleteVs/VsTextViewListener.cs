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
using System.Windows.Forms;

namespace AutocompleteVs
{
    /// <summary>
    /// Listens for editors creation
    /// </summary>
    [Export(typeof(IVsTextViewCreationListener))]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [ContentType(CONTENT_TYPE_ID)]
    internal sealed class VsTextViewListener : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService = null;

        /// <summary>
        /// Content type supported for autocompletion
        /// </summary>
        public const string CONTENT_TYPE_ID = "text";

        /// <summary>
        /// Adornments layer identifier for autocompletion
        /// </summary>
        public const string AUTOCOMPLETE_ADORNMENT_LAYER_ID = "AutocompleteLayer";

        // Disable "Field is never assigned to..." and "Field is never used" compiler's warnings. Justification: the field is used by MEF.
#pragma warning disable 649, 169
        /// <summary>
        /// Defines the adornment layer for the adornment. This layer is ordered
        /// after the selection layer in the Z-order
        /// </summary>
        [Export(typeof(AdornmentLayerDefinition))]
        [Name(AUTOCOMPLETE_ADORNMENT_LAYER_ID)]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        private AdornmentLayerDefinition editorAdornmentLayer;

#pragma warning restore 649, 169

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

            // Create the view autocompletion handler
            ViewAutocompleteHandler.AttachedHandler(WpfTextView);

            /*
             * See: https://stackoverflow.com/questions/18268685/visual-studio-extension-keyprocessor-alt-key
             *      https://stackoverflow.com/questions/60261450/how-to-handle-keyboard-events-in-vsix
             * search IWpfTextView key typed event
             */

        }
    }
}
