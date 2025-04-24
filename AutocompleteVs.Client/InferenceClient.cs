using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutocompleteVs.Client
{
    /// <summary>
    /// A client for the inference service
    /// </summary>
    public class InferenceClient : IInferenceHub, IAsyncDisposable
    {
        private readonly HubConnection _connection;

        public InferenceClient(string url = "http://localhost:5118/InferenceHub")
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();
            _connection.Closed += _connection_Closed;
        }

        async public Task ConnectAsync()
        {
            await _connection.StartAsync();
        }

        async public Task<string> PingAsync()
        {
            return await _connection.InvokeAsync<string>(nameof(PingAsync));
        }

        async public Task<string> StartInferenceAsync(string modelId, InferenceRequest prompt, string[] validWords)
        {
            return await _connection.InvokeAsync<string>(nameof(StartInferenceAsync), modelId, prompt, validWords);
        }

        async public Task<string> ContinueInferenceAsync(string[] validWords)
        {
            return await _connection.InvokeAsync<string>(nameof(ContinueInferenceAsync), validWords);
        }

        private Task _connection_Closed(Exception error)
        {
            Debug.WriteLine(error?.ToString() ?? "Connection closed without exception");
            return Task.CompletedTask;
        }

		async public ValueTask DisposeAsync()
		{
            await _connection.DisposeAsync();
        }
	}
}
