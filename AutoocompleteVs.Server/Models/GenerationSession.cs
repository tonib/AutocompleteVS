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
        private StatelessExecutor Executor;

        private InferenceParams InferenceParams;

        private IAsyncEnumerator<string>? InferenceProcess;

        private CancellationToken CancellationToken;

        public GenerationSession(LoadedModel model)
        {
            // TODO: This is really slow
            Executor = new StatelessExecutor(model.Model, model.Params);

            // TODO: Make this a config
            InferenceParams = new InferenceParams
            {
                SamplingPipeline = new DefaultSamplingPipeline
                {
                    Temperature = 0.6f
                },

                MaxTokens = -1
            };
        }

        public async Task<string?> StartGenerateAsync(string prompt, CancellationToken cancellationToken = default)
        {
            CancellationToken = cancellationToken;
            InferenceProcess = Executor.InferAsync(prompt, InferenceParams, CancellationToken).GetAsyncEnumerator();
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
            if (InferenceProcess == null)
                return;
            await InferenceProcess.DisposeAsync();
        }
    }
}
