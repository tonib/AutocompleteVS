
using Microsoft.AspNetCore.SignalR;

namespace AutoocompleteVs.Server.Hubs
{
    public class InferenceHub : Hub<IInferenceHub>
    {

        public Task<string> Ping() => Task.FromResult("Pong");

    }
}
