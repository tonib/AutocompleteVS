using AutocompleteVs.Client;
using LLama;
using LLama.Batched;
using LLama.Native;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoocompleteVs.Server.Models.BatchExecution
{
    public class ConversationData : IDisposable
    {
        public string Prompt = "";

        public string GeneratedText { get; private set; } = "";

        public List<LLamaToken> GeneratedTokens { get; private set; } = new List<LLamaToken>();

		public bool IsComplete { get; private set; }
        public bool IsFailed { get; private set; }

        public Conversation Conversation;

        public InferenceRequest Request { get; private set; }

        private StreamingTokenDecoder Decoder { get; init; }

        public ConversationData(BatchedExecutor executor, InferenceRequest request)
        {
            Request = request;
            Prompt = request.Prompt;
            Conversation = executor.Create();
            Decoder = new StreamingTokenDecoder(executor.Context);

            LLamaToken[] tokenizedPrompt = executor.Context.Tokenize(Prompt, addBos: true, special: true);
            Conversation.Prompt(tokenizedPrompt);
        }

        public void Dispose()
        {
            if (Conversation.IsDisposed)
                return;
            Conversation.Dispose();
        }

        public string? Add(LLamaToken token)
        {
            // Some special tokens indicate that this sequence has ended. Check if that's what has been chosen by the sampling pipeline.
            if (token.IsEndOfGeneration(Conversation.Executor.Context.NativeHandle.ModelHandle.Vocab))
            {
                MarkComplete();
                return null;
            }
            else
            {
                // It isn't the end of generation, so add this token to the decoder and then add that to our tracked data
                string addedText = TokenToString(token);
				GeneratedText += addedText;
                GeneratedTokens.Add(token);

                // Prompt the conversation with this token, ready for the next round of inference to generate another token
                Conversation.Prompt(token);
                return addedText;

			}
        }

        public string? Add(LLamaToken token, string[]? validWords)
        {
            return Add(token);

            // Not finished:
           /* int nValidWords = validWords?.Length ?? 0;
            if(nValidWords == 0 || token.IsControl(Conversation.Executor.Context.NativeHandle.ModelHandle))
                return Add(token);

            // Current prompt state:
            string? openWord = GetIdentifierAtEnd();
            if(openWord != null && !validWords!.Any(w => w.StartsWith(openWord)))
            {
                // Invalid open word. We are done
                MarkComplete();
                return null;
            }

            // Get new token text:
            Decoder.Add(token);
            string newText = Decoder.Read();

            return Add(token);*/
        }

        /// <summary>
        /// Checks what text of a new token can be safelly added to the current prompt state, and returns it.
        /// </summary>
        /// <param name="newToken">Thew new token purposed by the model</param>
        /// <param name="openWord">The current open identifier. null if cursor is not at the end of one identifier</param>
        /// <returns>The text to add. null if we must to stop the current conversation</returns>
        /*private string? ProcessNewToken(string newToken, string? openWord, string[]? validWords)
        {
            if (string.IsNullOrWhiteSpace(newToken))
                return newToken;

            // Get spaces at beggining of new token:
            int idx = 0;
            while (idx < newToken.Length && char.IsWhiteSpace(newToken[idx]))
                idx++;
            bool spacesAtStart = idx > 0;

            if(idx < newToken.Length && char.IsWhiteSpace(newToken[newToken.Length - 1]))

            if(idx > 0 && openWord != null)
            {
                // We are closing an identifier. Check is valid
                if(validWords != null && !validWords.Any(w => w.StartsWith(openWord)))
                {
                    // Invalid open word. We are done
                    MarkComplete();
                    return null;
                }

            }

            return null;
        }*/

        public void MarkComplete(bool failed = false)
        {
            IsComplete = true;
            IsFailed = failed;
            if (Conversation.IsDisposed == false)
            {
                // clean up the conversation and sampler to release more memory for inference. 
                // real life usage would protect against these two being referenced after being disposed.
                Conversation.Dispose();
            }
        }

        public string TokenToString(LLamaToken token)
		{
			Decoder.Add(token);
			return Decoder.Read();
		}
	}
}
