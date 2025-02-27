using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutocompleteVs
{
	/// <summary>
	/// A text label adorment
	/// </summary>
	internal sealed class LabelAdornment : Label
	{
		internal LabelAdornment(LabelTag labelTag) { this.Content = labelTag.Text; }

		internal void Update(LabelTag labelTag)
		{
			this.Content = labelTag.Text;
		}
	}
}
