using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutocompleteVs.TestIntraTextAdorments
{
	/// <summary>
	/// A text label adorment
	/// </summary>
	internal sealed class LabelAdornment : Label
	{
		internal LabelAdornment(LabelTag labelTag) { Content = labelTag.Text; }

		internal void Update(LabelTag labelTag)
		{
			Content = labelTag.Text;
		}
	}
}
