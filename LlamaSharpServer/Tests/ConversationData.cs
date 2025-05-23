﻿using LLama;
using LLama.Batched;
using LLama.Native;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoocompleteVs.Client.Example.Tests
{
    class ConversationData : IDisposable
    {
        public string Prompt = "";

        public string GeneratedText { get; private set; } = "";

        public bool IsComplete { get; private set; }
        public bool IsFailed { get; private set; }

        public Conversation Conversation;

        private StreamingTokenDecoder Decoder { get; init; }

        public ConversationData(BatchedExecutor executor, string prompt)
        {
            Prompt = prompt;
            Conversation = executor.Create();
            Decoder = new StreamingTokenDecoder(executor.Context);

            LLamaToken[] tokenizedPrompt = executor.Context.Tokenize(prompt, addBos: true, special: true);
            Conversation.Prompt(tokenizedPrompt);
        }

        public void Dispose()
        {
            if (Conversation.IsDisposed)
                return;
            Conversation.Dispose();
        }

        public void Add(LLamaToken token)
        {
            // Some special tokens indicate that this sequence has ended. Check if that's what has been chosen by the sampling pipeline.
            if (token.IsEndOfGeneration(Conversation.Executor.Context.NativeHandle.ModelHandle.Vocab))
            {
                MarkComplete();
            }
            else
            {
                // It isn't the end of generation, so add this token to the decoder and then add that to our tracked data
                Decoder.Add(token);
                GeneratedText += Decoder.Read();

                // Prompt the conversation with this token, ready for the next round of inference to generate another token
                Conversation.Prompt(token);
            }
        }

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
    }
}
