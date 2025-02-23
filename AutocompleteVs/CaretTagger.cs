using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
	class CaretTagger<T> : ITagger<T> where T : ITag
	{
		private readonly IWpfTextView TextView;

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

		public CaretTagger(IWpfTextView textView)
		{
			this.TextView = textView;
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

		public IEnumerable<ITagSpan<T>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			//throw new NotImplementedException();
			return new List<ITagSpan<T>>();
		}
	}
}
