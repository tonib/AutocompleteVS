using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
	/// <summary>
	/// Text tag info
	/// </summary>
	internal class LabelTag : ITag
	{
		internal readonly string Text;

		internal LabelTag(string text)
		{
			Text = text;
		}
	}
}
