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
		private int LineNumber = -1;
		private int BottomSize = 0;
		private int LineStart;
		double LineTop;

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
					return Identity;

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
			LineStart = line.Start;
			LineNumber = View.TextSnapshot.GetLineNumberFromPosition(line.Start);
			BottomSize = bottomSize;

			// Those are needed to force to redraw the line if the transform is removed:
			LineStart = line.Start.Position;
			LineTop = line.Top;

			// Force to redraw the line
			ForceRedrawLine();
		}

		private void ForceRedrawLine()
		{
			var lineStart = new SnapshotPoint(View.TextSnapshot, LineStart);
			View.DisplayTextLineContainingBufferPosition(lineStart, LineTop, ViewRelativePosition.Top);
		}

		public void RemoveCurrentTransform()
		{
			if (LineNumber < 0)
				return;

			LineNumber = -1;
			ForceRedrawLine();
		}
	}
}
