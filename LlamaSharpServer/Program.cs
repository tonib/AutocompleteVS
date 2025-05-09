﻿
using AutocompleteVs.Client;
using AutoocompleteVs.Client.Example.Tests;
using LLama;
using LLama.Batched;
using LLama.Common;
using LLama.Native;
using LLama.Sampling;
using LLama.Transformers;
using System.Diagnostics;
using System.Text;

namespace AutoocompleteVs.Client.Example
{
    class Program
    {
        // public const string CODEQWEN_PATH = @"C:\Users\Toni Bennasar\Documents\Models\Qwen2.5-Coder-1.5B.Q8_0.gguf";
        public const string CODEQWEN_PATH = @"..\..\..\..\Models\qwen2.5-coder-1.5b-q8_0.gguf";

        public const string PREFIX1 =
@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoremIpsum
{
    class Program
    {
        static void Main(string[] args)
        {
            // Write a VERY short version of the ""lorem ipsum"" text to the console
            const string lorenImpsum = ";

        public const string SUFFIX1 =
@"
        }
    }
}

/*
    Loren ipsum text:
        Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore 
        magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo 
        consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. 
        Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
        ...
*/

";

        public const string CODE_PROMPT1 =
        $@"
<|fim_prefix|>{PREFIX1}<|fim_suffix|>
{SUFFIX1}<|fim_middle|>";

        public const string CODE_PROMPT2 =
            @"
<|fim_prefix|>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    /// <summary>
    /// A simple hello world program
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            <|fim_suffix|>
        }
    }
}
<|fim_middle|>";

        static void Main(string[] args)
        {
            try
            {
                // TestBatched().Wait();

                TestServer().Wait();
                // await TestChatAsync();
                // await TestAutocompletion();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadKey();
        }

        private static async Task TestBatched()
        {
            var test = new BatchedExeTests();
            await test.SetupAsync();
            for(int i=0; i<100; i++)
                await test.TestBatchedInference();
        }

        static async Task TestServer()
        {
            try
            {
                var inferenceClient = new InferenceClient();
                await inferenceClient.ConnectAsync();
                Console.WriteLine(await inferenceClient.PingAsync());
                await RunGeneration(inferenceClient);
                await RunGeneration(inferenceClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            // Console.ReadLine();
        }
        
        static async Task RunGeneration(InferenceClient inferenceClient)
        {
            StringBuilder sb = new StringBuilder();
            var request = new InferenceRequest
            {
                Prompt = PREFIX1,
                Suffix = SUFFIX1,
            };
            string token = await inferenceClient.StartInferenceAsync("qwen2.5-coder-1.5b-q8_0.gguf", request, null);
            while (token != null)
            {
                sb.Append(token);
                token = await inferenceClient.ContinueInferenceAsync(null);
            }
            Console.WriteLine(sb.ToString());
        }

        async static Task TestAutocompletion()
        {
            var parameters = new ModelParams(CODEQWEN_PATH);
            parameters.ContextSize = 4096;
            parameters.GpuLayerCount = 32;
            using LLamaWeights model = await LLamaWeights.LoadFromFileAsync(parameters);

            // https://github.com/QwenLM/Qwen2.5-Coder
            // prompt = '<|fim_prefix|>' + prefix_code + '<|fim_suffix|>' + suffix_code + '<|fim_middle|>'

            var executor = new StatelessExecutor(model, parameters);

            // Print some info
            var name = model.Metadata.GetValueOrDefault("general.name", "unknown model name");
            Console.WriteLine($"Created executor with model: {name}");

            var inferenceParams = new InferenceParams
            {
                SamplingPipeline = new DefaultSamplingPipeline
                {
                    Temperature = 0.6f
                },

                MaxTokens = -1
            };

            await foreach (var text in executor.InferAsync(CODE_PROMPT1, inferenceParams))
            {
                Console.Write(text);
            }
            Console.Read();
        }


        async static Task TestChatAsync()
        {
            var parameters = new ModelParams(@"C:\Users\Toni Bennasar\source\repos\LLamaSharp\LLama.Unittest\Models\Llama-3.2-1B-Instruct-Q4_0.gguf");
            parameters.ContextSize = 2048;
            using LLamaWeights model = await LLamaWeights.LoadFromFileAsync(parameters);
            using LLamaContext context = model.CreateContext(parameters);
            var executor = new InteractiveExecutor(context);

            var chatHistory = new ChatHistory();
            chatHistory.AddMessage(AuthorRole.System, "You are a helpful assistant");

            // add the default templator. If llama.cpp doesn't support the template by default, 
            // you'll need to write your own transformer to format the prompt correctly
            ChatSession session = new(executor, chatHistory);
            session.WithHistoryTransform(new PromptTemplateTransformer(model, withAssistant: true));

            // Add a transformer to eliminate printing the end of turn tokens, llama 3 specifically has an odd LF that gets printed sometimes
            session.WithOutputTransform(new LLamaTransforms.KeywordTextOutputStreamTransform(
                ["User:", "�"],
                redundancyLength: 5));

            var inferenceParams = new InferenceParams
            {
                SamplingPipeline = new DefaultSamplingPipeline
                {
                    Temperature = 0.6f
                },

                MaxTokens = -1, // keep generating tokens until the anti prompt is encountered
                AntiPrompts = ["User:"] // model specific end of turn string (or default)
            };

            string userInput = "What's your name?";

            // as each token (partial or whole word is streamed back) print it to the console, stream to web client, etc
            await foreach (
                var text
                in session.ChatAsync(
                    new ChatHistory.Message(AuthorRole.User, userInput),
                    inferenceParams))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(text);
            }
            Console.WriteLine();
            Console.ReadLine();
        }

    }
}
