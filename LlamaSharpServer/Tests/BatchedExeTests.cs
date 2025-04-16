using LLama.Batched;
using LLama.Common;
using LLama.Native;
using LLama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLama.Sampling;

namespace AutoocompleteVs.Client.Example.Tests
{
    class BatchedExeTests
    {

        
        static public async Task TestBatchedInference()
        {
            var parameters = new ModelParams(Program.CODEQWEN_PATH);
            parameters.ContextSize = 4096;
            parameters.GpuLayerCount = 32;
            using LLamaWeights model = await LLamaWeights.LoadFromFileAsync(parameters);

            // Create an executor that can evaluate a batch of conversations together
            using var executor = new BatchedExecutor(model, parameters);

            // TODO: Try to remove a conversation

            // Add conversation prompts
            string[] prompts = { Program.CODE_PROMPT1, Program.CODE_PROMPT2 };
            List<ConversationData> conversations = new List<ConversationData>();
            foreach (string prompt in prompts)
            {
                conversations.Add(new ConversationData(executor, prompt));
            }

            // Start to infer. It looks like only a single inference can be done at a time
            using var sampler = new GreedySamplingPipeline();
            while (true)
            {
                // Run inference for all conversations in the batch which have pending tokens.
                DecodeResult decodeResult = await executor.Infer();

                // Inference can fail, always check the return value!
                // NoKvSlot is not a fatal error, it just means that there's not enough memory available in the KV cache to process everything. You can force
                // this to happen by setting a small value for ContextSize in the ModelParams at the top of this file (e.g. 512).
                // In this case it's handled by ending a conversation (which will free up some space) and trying again. You could also handle this by
                // saving the conversation to disk and loading it up again later once some other conversations have finished.
                if (decodeResult == DecodeResult.NoKvSlot)
                {
                    conversations.FirstOrDefault(a => !a.IsComplete)?.MarkComplete(failed: true);
                    continue;
                }

                if (decodeResult != DecodeResult.Ok)
                {
                    // TODO: Check what to do with DecodeResult.NoKvSlot: Truncate prompt?
                    throw new Exception($"Failed to infer batch, error: {decodeResult}");
                }

                // Feed conversations with new generated tokens
                foreach (ConversationData conversation in conversations)
                {
                    // Completed conversations don't need sampling.
                    if (conversation.IsComplete)
                        continue;

                    // If the conversation wasn't prompted before the last call to Infer then it won't need sampling.
                    // TODO: Is not this equal to IsComplete == true?
                    // TODO: When could you have IsComplete == false and RequireSampling == true?
                    if (!conversation.Conversation.RequiresSampling)
                        continue;

                    // Use the sampling pipeline to choose a single token for this conversation.
                    LLamaToken token = conversation.Conversation.Sample(sampler);
                    conversation.Add(token);
                }

                if (conversations.All(c => c.IsComplete))
                    break;

            }

            Console.WriteLine("-----------------");
            foreach (ConversationData conversation in conversations)
            {
                Console.WriteLine(conversation.GeneratedText);
                Console.WriteLine("-----------------");
            }
            Console.WriteLine("-----------------");

        }
    }
}
