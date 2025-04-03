using AutocompleteVs.Logging;
using OllamaSharp;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration
{
	/// <summary>
	/// Suggestions generation with OpenAI API
	/// </summary>
	class OpenAiGenerator : IGenerator
	{
		// https://github.com/openai/openai-dotnet?tab=readme-ov-file


		private OpenAIClient OpenAiClient;
		private ChatClient ChatClient;

		public OpenAiGenerator(Settings settings)
		{
			OpenAiClient = new OpenAIClient(settings.OpenAiKey);
			// TODO: Configure model
			ChatClient = OpenAiClient.GetChatClient("gpt-4o-mini");
		}

		public void Dispose()
		{
			// Nothing disposable... Suspicious
		}

		async public Task GetAutocompletionInternalAsync(GenerationParameters parameters, CancellationToken cancellationToken)
		{
			// TODO: The catch is duplicated in Ollama generator, unify code
			try
			{
				await OutputPaneHandler.Instance.LogAsync("Starting new suggestion", LogLevel.Debug);

				// TODO: Check if any of the ollama generation parms is appliable to OpenAI API (topk, topp, etc)

				// TODO: Configure this:
				string systemPrompt = "You are an code autocompletion tool. User will give you a piece of C# code he/she " +
					"is editing, with a token <|CURSOR|> where the cursor is currently located in text editor. You must write " +
					"the most probable text expected in that cursor position (code, comments, string literals, etc). " +
					"You will not make any comment, nor use markdown, just write the code in plain text. " +
					"You will not repeat existing code, and respect " +
					"current indentations You can use classes not included in current using statements: If they are needed," +
					"user will add them later";

				string userPrompt = parameters.ModelPrompt.PrefixText + "<|CURSOR|>" + parameters.ModelPrompt.SuffixText;

				List<ChatMessage> messages = new List<ChatMessage>
				{
					new SystemChatMessage(systemPrompt),
					new UserChatMessage(userPrompt)
				};

				// Write prompt to debug:
				await OutputPaneHandler.Instance.LogAsync("System prompt:", LogLevel.Debug);
				await OutputPaneHandler.Instance.LogAsync(systemPrompt, LogLevel.Debug);
				await OutputPaneHandler.Instance.LogAsync("User prompt:", LogLevel.Debug);
				await OutputPaneHandler.Instance.LogAsync(userPrompt, LogLevel.Debug);

				ChatCompletion completion;
				using (var exeTime = new ExecutionTime($"Autocompletion generation, " +
					$"prefix chars: {parameters.ModelPrompt.PrefixText.Length}, " +
					$"suffix chars: {parameters.ModelPrompt.SuffixText.Length}", false))
				{
					completion = await ChatClient
						.CompleteChatAsync(messages, cancellationToken: cancellationToken)
						.ConfigureAwait(false);
					await exeTime.WriteElapsedTimeAsync();
				}

				string autocompleteText = completion.Content[0].Text;

				// Notify the view the autocompletion has finished.
				// Run it in the UI thread. Otherwise it will trhow an excepcion
				await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
				parameters.View.AutocompletionGenerationFinished(new Autocompletion(autocompleteText, parameters));
			}
			catch (TaskCanceledException)
			{
				await OutputPaneHandler.Instance.LogAsync("Suggestion cancelled");
			}
			catch (Exception ex)
			{
				// Somethimes is giving me IOException inside "await enumerator.MoveNextAsync()" instead a TaskCanceledException
				// So check if process was canceled
				bool isCanceled = cancellationToken.IsCancellationRequested;
				if (!isCanceled)
				{
					await OutputPaneHandler.Instance.LogAsync(ex);
				}
				else
				{
					await OutputPaneHandler.Instance.LogAsync("Suggestion cancelled");
				}
			}
		}
	}
}
