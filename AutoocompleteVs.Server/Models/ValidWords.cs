using LLama.Batched;
using LLama.Native;

namespace AutoocompleteVs.Server.Models
{
	/// <summary>
	/// Stores tokenized valid words to generate
	/// </summary>
	public class ValidWords
	{
		string[]? ValidWordsText;
		// LLamaToken[][]? TokenizedValidWords;

		public ValidWords(BatchedExecutor executor, string[] validWords)
		{
			ValidWordsText = validWords;
			if (ValidWordsText == null)
				return;

			//TokenizedValidWords = new LLamaToken[validWords.Length][];
			//for (int i = 0; i < validWords.Length; i++)
			//{
			//	TokenizedValidWords[i] = executor.Context.Tokenize(validWords[i], addBos: false, special: false);
			//}
		}

		public bool IsValidWord(string word)
		{
			if (ValidWordsText == null)
				return true;
			for (int i = 0; i < ValidWordsText.Length; i++)
			{
				if (ValidWordsText[i] == word)
					return true;
			}
			return false;
		}

		public bool IsValidPrefix(string prefix)
		{
			if (ValidWordsText == null)
				return true;
			for (int i = 0; i < ValidWordsText.Length; i++)
			{
				if (ValidWordsText[i].StartsWith(prefix))
					return true;
			}
			return false;
		}
	}
}
