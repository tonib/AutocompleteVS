using AutocompleteVs.Client;
using LLama;
using LLama.Batched;
using LLama.Common;

namespace AutoocompleteVs.Server.Models.BatchExecution
{
	public class Executor : IDisposable
	{
		public BatchedExecutor BatchedExecutor;

		public List<ConversationData> Conversations = new();

		public Executor(LLamaWeights weights, ModelParams parameters)
		{
			BatchedExecutor = new BatchedExecutor(weights, parameters);
		}

		public ConversationData CreateConversation(InferenceRequest request)
		{
			var conversation = new ConversationData(BatchedExecutor, request);
			Conversations.Add(conversation);
			return conversation;
		}

		public void RemoveConversation(ConversationData conversation)
		{
			if (Conversations.Contains(conversation))
			{
				conversation.Dispose();
				Conversations.Remove(conversation);
			}
		}

		public void ClearConversations()
		{
			foreach (var conversation in Conversations)
			{
				conversation.Dispose();
			}
			Conversations.Clear();
		}

		public void Dispose()
		{
			ClearConversations();
			BatchedExecutor?.Dispose();
		}

	}
}
