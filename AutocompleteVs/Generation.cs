using Microsoft.Extensions.AI;
using OllamaSharp.Models;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
	class Generation
	{
		/*static async public Task TestAsync()
		{
			IChatClient x = null;
			if (x != null)
			{
				IAsyncEnumerable<ChatResponseUpdate> r = x.GetStreamingResponseAsync(null);
			}

			//IAsyncEnumerable<int> tuputamadre = null;
			//IAsyncEnumerator<int> x = tuputamadre?.GetAsyncEnumerator();
			//if(x != null)
			//	_ = await x.MoveNextAsync();
		}*/

		async public static Task DoRequestAsync(string prefixText, string suffixText)
		{

			// set up the client
			var uri = new Uri("http://localhost:11434");
			var ollama = new OllamaApiClient(uri);

			// select a model which should be used for further operations
			ollama.SelectedModel = "Qwen2.5-Coder 1.5B";

			var request = new GenerateRequest();
			request.Prompt = prefixText;
			request.Suffix = suffixText;
			/*
			IAsyncEnumerator<GenerateResponseStream> enumerator = ollama.GenerateAsync(request).GetAsyncEnumerator();
			while(await enumerator.MoveNextAsync())
			{
				Debug.WriteLine(enumerator.Current.Response);
			}*/
		}
	}
}
