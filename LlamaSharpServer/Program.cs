
using LLama;
using LLama.Batched;
using LLama.Common;
using LLama.Sampling;
using LLama.Transformers;

// await TestChatAsync();
await TestAutocompletion();

async static Task TestAutocompletion()
{
    // Codequen
    var parameters = new ModelParams(@"C:\Users\Toni Bennasar\Documents\Models\Qwen2.5-Coder-1.5B.Q8_0.gguf");
    parameters.ContextSize = 4096;
    parameters.GpuLayerCount = 32;
    using LLamaWeights model = await LLamaWeights.LoadFromFileAsync(parameters);
    using LLamaContext context = model.CreateContext(parameters);

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

    string prompt =
@"
<|fim_prefix|>
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
            // Write the lorem ipsum text to the console
            <|fim_suffix|>
        }
    }
}
<|fim_middle|>";

    await foreach (var text in executor.InferAsync(prompt, inferenceParams))
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
