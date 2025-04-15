﻿
using AutocompleteVs.Client;
using AutoocompleteVs.Server.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace AutoocompleteVs.Server.Hubs
{
    public class InferenceHub : Hub<IInferenceHub>
    {
        // TODO: Handle disconnects

        static private ConcurrentDictionary<string, GenerationSession> _sessions = new ConcurrentDictionary<string, GenerationSession>();

        public Task<string> PingAsync() => Task.FromResult("Pong");

        async public Task<string?> StartInferenceAsync(string modelId, string prompt)
        {
            LoadedModel model = await LoadedModel.LoadOrGetModelAsync(modelId);
            GenerationSession session = new GenerationSession(model);
            _sessions[this.Context.ConnectionId] = session;

            return await session.StartGenerateAsync(prompt);
        }

        async public Task<string?> ContinueInferenceAsync()
        {
            if(!_sessions.TryGetValue(Context.ConnectionId, out GenerationSession? session))
                throw new InvalidOperationException("No active inference session");

            return await session.ContinueGenerateAsync();
        }

        async public override Task OnDisconnectedAsync(Exception? exception)
        {
            if(_sessions.TryRemove(Context.ConnectionId, out GenerationSession? session))
            {
                await session.DisposeAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
