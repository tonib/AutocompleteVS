
using LLama;
using LLama.Common;
using LLama.Sampling;
using LLama.Transformers;

await TestChatAsync();

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
