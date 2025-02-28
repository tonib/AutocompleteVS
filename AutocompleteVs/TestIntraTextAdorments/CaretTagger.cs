using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace AutocompleteVs.TestIntraTextAdorments
{
	class CaretTagger : ITagger<LabelTag>
	{
		private readonly IWpfTextView TextView;

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

		public CaretTagger(IWpfTextView textView)
		{
			TextView = textView;
			TextView.Caret.PositionChanged += Caret_PositionChanged;
		}

		private void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
		{
			int oldIdx = e.OldPosition.BufferPosition.Position;
			int newIdx = e.NewPosition.BufferPosition.Position;
			int min = Math.Min(oldIdx, newIdx);
			int max = Math.Max(oldIdx, newIdx);
			int length = max - min;
			var span = new SnapshotSpan(TextView.TextSnapshot, min, length);

			TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(span));
		}

		public IEnumerable<ITagSpan<LabelTag>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			// TODO: Check caret is inside spans ??? NFI
			SnapshotSpan span = new SnapshotSpan(TextView.TextSnapshot, TextView.Caret.Position.BufferPosition, 0);
			return new ITagSpan<LabelTag>[] { new TagSpan<LabelTag>(span, new LabelTag("Achilipu arriquitaun")) };
		}
	}
}
