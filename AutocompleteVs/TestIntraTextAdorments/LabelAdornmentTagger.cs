using AutocompleteVs.TestIntraTextAdorments.Support;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.TestIntraTextAdorments
{
	/// <summary>
	/// Provides autocompletion adorments at the caret position
	/// </summary>
	internal sealed class LabelAdornmentTagger : IntraTextAdornmentTagger<LabelTag, LabelAdornment>
	{

		private ITagAggregator<LabelTag> LabelTagger;

		internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, Lazy<ITagAggregator<LabelTag>> colorTagger)
		{
			return view.Properties.GetOrCreateSingletonProperty(
				() => new LabelAdornmentTagger(view, colorTagger.Value));
		}

		private LabelAdornmentTagger(IWpfTextView view, ITagAggregator<LabelTag> labelTagger)
			: base(view)
		{
			LabelTagger = labelTagger;
		}

		public void Dispose()
		{
			LabelTagger.Dispose();

			view.Properties.RemoveProperty(typeof(LabelAdornmentTagger));
		}

		// To produce adornments that don't obscure the text, the adornment tags
		// should have zero length spans. Overriding this method allows control
		// over the tag spans.
		protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, LabelTag>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
		{
			if (spans.Count == 0)
				yield break;

			ITextSnapshot snapshot = spans[0].Snapshot;

			var colorTags = LabelTagger.GetTags(spans);

			foreach (IMappingTagSpan<LabelTag> dataTagSpan in colorTags)
			{
				NormalizedSnapshotSpanCollection colorTagSpans = dataTagSpan.Span.GetSpans(snapshot);

				// Ignore data tags that are split by projection.
				// This is theoretically possible but unlikely in current scenarios.
				if (colorTagSpans.Count != 1)
					continue;

				SnapshotSpan adornmentSpan = new SnapshotSpan(colorTagSpans[0].Start, 0);

				yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, dataTagSpan.Tag);
			}
		}

		protected override LabelAdornment CreateAdornment(LabelTag data, SnapshotSpan span)
		{
			return new LabelAdornment(data);
		}

		protected override bool UpdateAdornment(LabelAdornment adornment, LabelTag data)
		{
			adornment.Update(data);
			return true;
		}
	}
}
