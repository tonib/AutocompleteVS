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
using System.Diagnostics;

namespace AutoocompleteVs.Client.Example.Tests
{
    class BatchedExeTests : IDisposable
    {
        private ModelParams parameters;
        private LLamaWeights model;
        private BatchedExecutor executor;
        private List<ConversationData> conversations = new List<ConversationData>();

        public async Task SetupAsync()
        {
            parameters = new ModelParams(Program.CODEQWEN_PATH);
            parameters.ContextSize = 2048;
            parameters.GpuLayerCount = 37;
            // parameters.Threads = 8;

            model = await LLamaWeights.LoadFromFileAsync(parameters);
            // Create an executor that can evaluate a batch of conversations together
            executor = new BatchedExecutor(model, parameters);
        }

        public void Dispose()
        {
            model?.Dispose();
            executor?.Dispose();
        }

        public async Task TestBatchedInference()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            // Add conversation prompts
            string[] prompts = { Program.CODE_PROMPT1, Program.CODE_PROMPT2 , Program.CODE_PROMPT1
            , Program.CODE_PROMPT1, Program.CODE_PROMPT1, Program.CODE_PROMPT1 };
            List<ConversationData> conversations = new List<ConversationData>();
            foreach (string prompt in prompts)
            {
                conversations.Add(new ConversationData(executor, prompt));
            }

            // Start to infer. It looks like only a single inference can be done at a time
            int nInference = 0;
            bool first = true;
            using var sampler = new GreedySamplingPipeline();
            while (true)
            {
                // Run inference for all conversations in the batch which have pending tokens.
                DecodeResult decodeResult = await executor.Infer();
                if(first)
                {
                    first = false;
                    watch.Stop();
                    Console.WriteLine($"First token: {watch.ElapsedMilliseconds} ms");
                    watch.Start();
                }

                // Inference can fail, always check the return value!
                // NoKvSlot is not a fatal error, it just means that there's not enough memory available in the KV cache to process everything. You can force
                // this to happen by setting a small value for ContextSize in the ModelParams at the top of this file (e.g. 512).
                // In this case it's handled by ending a conversation (which will free up some space) and trying again. You could also handle this by
                // saving the conversation to disk and loading it up again later once some other conversations have finished.
                if (decodeResult == DecodeResult.NoKvSlot)
                {
                    //  TODO: This is weird. Can you slide the kv cache BEFORE getting this error?
                    //  TODO: It happens always when you reach the context size? ???
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

                // NO diference:
                //var toRemove = conversations.Where(c => c.IsComplete).ToList();
                //foreach(var c in toRemove)
                //{
                //    c.Dispose();
                //    conversations.Remove(c);
                //}

                nInference++;
                //if (nInference == 5)
                //{
                //    var conversation = conversations[0];
                //    conversation.Dispose();
                //    conversations.RemoveAt(0);
                //}
            }

            watch.Stop();
            Console.WriteLine($"Total, {nInference} inferences: {watch.ElapsedMilliseconds} ms, mean: {watch.ElapsedMilliseconds / nInference} ms");

            Console.WriteLine("-----------------");
            foreach (ConversationData conversation in conversations)
            {
                Console.WriteLine(conversation.GeneratedText);
                Console.WriteLine("-----------------");
                conversation.Dispose();
            }
            conversations.Clear();
            Console.WriteLine("-----------------");

        }
    }
}
