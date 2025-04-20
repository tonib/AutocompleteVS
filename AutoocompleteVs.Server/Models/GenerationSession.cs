using AutoocompleteVs.Server.Models.BatchExecution;
using LLama;
using LLama.Batched;
using LLama.Common;
using LLama.Native;
using LLama.Sampling;
using System.Runtime.CompilerServices;

namespace AutoocompleteVs.Server.Models
{
    /// <summary>
    /// Stores state of the current generation session
    /// </summary>
    public class GenerationSession
    {
		// TODO: Handle cancellation token

		private LoadedModel Model;

        private InferenceParams InferenceParams;

        private ConversationData? Conversation;

        private GreedySamplingPipeline Sampler = new GreedySamplingPipeline();

        public GenerationSession(LoadedModel model)
        {
            Model = model;

            // TODO: Make this a config
            InferenceParams = new InferenceParams
            {
                SamplingPipeline = new DefaultSamplingPipeline
                {
                    Temperature = 0.6f,
                    TopK = 1,
                    Seed = 1
                },

                MaxTokens = -1
            };
        }

        public async Task<string?> StartGenerateAsync(string prompt, string[]? validWords)
        {
            Conversation = Model.Executor.CreateConversation(prompt);

            if(validWords != null)
                throw new NotImplementedException("Valid words not implemented yet");

            return await SimpleGenerationAsync();
		}

        public async Task<string?> ContinueGenerateAsync(string[]? validWords)
        {
            if(Conversation == null)
                throw new InvalidOperationException("Inference process not started");

			if (validWords != null)
				throw new NotImplementedException("Valid words not implemented yet");

			return await SimpleGenerationAsync();
		}

        private async Task<string?> SimpleGenerationAsync()
        {
			var decodeResult = await Model.Executor.BatchedExecutor.Infer();

			// Inference can fail, always check the return value!
			// NoKvSlot is not a fatal error, it just means that there's not enough memory available in the KV cache to process everything. You can force
			// this to happen by setting a small value for ContextSize in the ModelParams at the top of this file (e.g. 512).
			// In this case it's handled by ending a conversation (which will free up some space) and trying again. You could also handle this by
			// saving the conversation to disk and loading it up again later once some other conversations have finished.
			if (decodeResult == DecodeResult.NoKvSlot)
			{
                Conversation!.Dispose();
                Conversation = null;
                throw new Exception("No KV slot available");
			}

			// A generic error, this is fatal and the batch can no longer be used. This should never occur and generally indicates
			// a bug in LLamaSharp, llama.cpp or a hardware error.
			if (decodeResult == DecodeResult.Error)
            {
				Conversation!.Dispose();
				Conversation = null;
				throw new Exception($"Unknown error occurred while inferring, result: {decodeResult}");
			}

			// Use the sampling pipeline to choose a single token for this conversation.
			LLamaToken token = Conversation!.Conversation.Sample(Sampler);
            string tokenText = Conversation.Add(token);
			if (tokenText == null)
			{
				Conversation!.Dispose();
				Conversation = null;
			}
			return tokenText;
		}

        public void Dispose()
        {
            // TODO: Should we dispose the IAsyncEnumerable? right now only the IAsyncEnumerator
            if (Conversation == null)
                return;
            Conversation.Dispose();
        }
    }
}
