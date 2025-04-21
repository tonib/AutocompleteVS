using System;
using System.Collections.Generic;
using System.Text;

namespace AutocompleteVs.Client
{
	public class InferenceRequest
	{
		/// <summary>
		/// Prompt
		/// </summary>
		public string Prompt { get; set; }

		/// <summary>
		/// If model supports infill, tihs is the text after the cursor. 
		/// null if does not support infill, or should not be used
		/// </summary>
		public string Suffix { get; set; }

		public string PromptString()
		{
			// TODO: Check if we can build the tokenized version here, adding manually the prefix / suffix
			// TODO: tokens
			StringBuilder sb = new StringBuilder();
			sb.Append("<|fim_prefix|>");
			sb.Append(Prompt);
			sb.Append("<|fim_suffix|>");
			sb.Append(Suffix);
			sb.Append("<|fim_middle|>");
			return sb.ToString();
		}
	}
}
