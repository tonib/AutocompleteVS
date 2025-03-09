using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace AutocompleteVs.LIneTransforms
{
	class MultiLineCompletionTransformSource : ILineTransformSource
	{
		static private readonly LineTransform Identity = new LineTransform(1.0);
		private IWpfTextView View;

		/// <summary>
		/// Line index number where transform has been applied. -1 == no transform applied
		/// </summary>
		private int LineNumber = -1;

		private int BottomSize = 0;

		/// <summary>
		/// Buffer position for start line where transform has been applied
		/// </summary>
		private int LineStart;

		private MultiLineCompletionTransformSource(IWpfTextView view)
		{
			View = view;
		}

		static public MultiLineCompletionTransformSource Attached(IWpfTextView view)
			=> view.Properties.GetOrCreateSingletonProperty(() => new MultiLineCompletionTransformSource(view));

		public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
		{
			try
			{
				if (LineNumber < 0)
				{
					// No transform currently applied
					return Identity;
				}

				// TODO: Performance. Just check if line contains the buffer position LineStart ???
				int currentLineNumber = View.TextSnapshot.GetLineNumberFromPosition(line.Start);
				if (LineNumber == currentLineNumber)
					return new LineTransform(0, BottomSize, 1.0);

				return Identity;
			}
			catch(Exception ex)
			{
				// TODO: Log exception
				Debug.WriteLine(ex.ToString());
				return Identity;
			}
		}

		public void AddTransform(ITextViewLine line, int bottomSize)
		{
			LineStart = line.Start.Position;
			LineNumber = View.TextSnapshot.GetLineNumberFromPosition(line.Start);
			BottomSize = bottomSize;

			// Force to redraw the line
			ForceRedrawView();
		}

		private void ForceRedrawView()
		{
			// Get line where transform was applied
			var lineStart = new SnapshotPoint(View.TextSnapshot, LineStart);
			if (!View.TryGetTextViewLineContainingBufferPosition(lineStart, out ITextViewLine line))
				return;

			// Line has been formated by the view?
			if (line.VisibilityState != VisibilityState.Unattached)
			{
				// Yes. Force the view to redraw so that (top of) the line has exactly the same position.
				View.DisplayTextLineContainingBufferPosition(line.Start, line.Top - View.ViewportTop, ViewRelativePosition.Top);
			}
		}

		public void RemoveCurrentTransform()
		{
			if (LineNumber < 0)
				return;

			LineNumber = -1;
			ForceRedrawView();
		}
	}
}
