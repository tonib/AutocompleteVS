using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutocompleteVs.Keyboard
{
	/// <summary>
	/// Handles keyboard events in a view
	/// </summary>
	internal class AutocompleteKeyProcesor : KeyProcessor
	{
		public delegate void OnPreviewKeyDownDelegate(KeyEventArgs args);

		public event OnPreviewKeyDownDelegate OnPreviewKeyDown;

		private AutocompleteKeyProcesor() 
		{
		}

		public override bool IsInterestedInHandledEvents => true; 

		static public AutocompleteKeyProcesor AttachedProcessor(IWpfTextView view) =>
			view.Properties.GetOrCreateSingletonProperty(() => new AutocompleteKeyProcesor());

		public override void PreviewKeyDown(KeyEventArgs args)
		{
			try
			{
				OnPreviewKeyDown?.Invoke(args);
			}
			catch(Exception ex)
			{
				// TODO: Log exceptions
				Debug.WriteLine(ex.ToString());
			}
			base.PreviewKeyDown(args);
		}
	}
}
