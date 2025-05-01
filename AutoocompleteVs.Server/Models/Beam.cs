using AutoocompleteVs.Server.Models.BatchExecution;
using LLama;
using LLama.Native;
using System.Text;

namespace AutoocompleteVs.Server.Models
{
	public class Beam : IDisposable
	{
		private ConversationData Conversation;

		public Beam(ConversationData conversation)
		{
			Conversation = conversation;
		}

		public List<Beam> Sample(int nBeams, ValidWords validWords)
		{
			// Apply softmax, this calculates probabilities and sorts tokens into descending order
			LLamaTokenDataArray logitsArr = LLamaTokenDataArray.Create(Conversation.Conversation.Sample());
			logitsArr.Softmax();

			var newBeams = new List<Beam>();
			for(int i=0; i<logitsArr.Data.Length; i++)
			{
				// Check if the token matche a valid word
				LLamaTokenData token = logitsArr.Data.Span[i];
				bool acceptToken = false;
				if(token.ID.IsControl(Conversation.Conversation.Executor.Context.Vocab))
					acceptToken = true;
				else
				{
					// A renderable token. validWords only contains identifier tokens. So,
					// spaces, punctuation, numbers, etc can be accepted

				}
			}
			return newBeams;
		}

		private bool AceptToken(LLamaTokenData token, ValidWords validWords)
		{
			if (token.ID.IsControl(Conversation.Conversation.Executor.Context.Vocab))
			{
				if(Conversation.GeneratedTokens.Count == 0)
				{
					// This is the first token, so we can just add it to the conversation
					return true;
				}

				string tokenText = Conversation.TokenToString(token.ID);
				if (validWords.IsValidWord(tokenText))
				{
					// Current word is a valid word, so we can add it to the conversation
					return true;
				}
				return false;
			}

			// Token is a text token.
			string newTokenText = Conversation.TokenToString(token.ID);
			if (Conversation.GeneratedTokens.Count == 0 && !Char.IsLetter(newTokenText[0]))
			{
				// Token is not a identifier: Accept it
				return true;
			}
			
			// If new tokens contain spaces, add text just to the 
			if (validWords.IsValidPrefix(newTokenText))
			{
				return true;
			}

			return false;
		}

		public void Dispose()
		{
			
		}
	}
}
