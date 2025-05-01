using AutoocompleteVs.Server.Models.BatchExecution;
using LLama.Batched;

namespace AutoocompleteVs.Server.Models
{
	public class WordBeamSearch
	{
		private BatchedExecutor Executor;

		private ValidWords ValidWords;

		public WordBeamSearch(BatchedExecutor executor, ValidWords validWords)
		{
			Executor = executor;
			ValidWords = validWords;
		}

		/// <summary>
		/// Generates the next word in the conversation using a beam search algorithm.
		/// Word can be a non C# token (ex. "." or "!"), or some of the valid specified words 
		/// </summary>
		/// <param name="conversation">The initial prompt</param>
		/// <returns>The next word. null for EOS</returns>
		public string InferNextWord(ConversationData conversation)
		{
			return null;
		}
	}
}
