using LLama;
using LLama.Common;
using LLama.Sampling;
using System.Runtime.CompilerServices;

namespace AutoocompleteVs.Server.Models
{
    /// <summary>
    /// Stores state of the current generation session
    /// </summary>
    public class GenerationSession : IAsyncDisposable
    {
        private LoadedModel Model;

        private InferenceParams InferenceParams;

        private IAsyncEnumerator<string>? InferenceProcess;

        private CancellationToken CancellationToken;

        public GenerationSession(LoadedModel model)
        {
            Model = model;

            // TODO: Make this a config
            InferenceParams = new InferenceParams
            {
                SamplingPipeline = new DefaultSamplingPipeline
                {
                    Temperature = 0.6f,
                    TopK = 1,
                    Seed = 1
                },

                MaxTokens = -1
            };
        }

        public async Task<string?> StartGenerateAsync(string prompt, CancellationToken cancellationToken = default)
        {
            CancellationToken = cancellationToken;
            InferenceProcess = Model.Executor.InferAsync(prompt, InferenceParams, CancellationToken).GetAsyncEnumerator();
            if (!await InferenceProcess.MoveNextAsync())
                return null;
            return InferenceProcess.Current;
        }

        public async Task<string?> ContinueGenerateAsync()
        {
            if(InferenceProcess == null)
                throw new InvalidOperationException("Inference process not started");

            if (!await InferenceProcess.MoveNextAsync())
                return null;
            return InferenceProcess.Current;
        }

        public async ValueTask DisposeAsync()
        {
            // TODO: Should we dispose the IAsyncEnumerable? right now only the IAsyncEnumerator
            if (InferenceProcess == null)
                return;
            await InferenceProcess.DisposeAsync();
        }
    }
}
