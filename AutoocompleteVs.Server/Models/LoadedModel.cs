using AutoocompleteVs.Server.Models.BatchExecution;
using LLama;
using LLama.Common;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;

namespace AutoocompleteVs.Server.Models
{
    /// <summary>
    /// Llama models loaded
    /// </summary>
    public class LoadedModel : IDisposable
    {
        static private ConcurrentDictionary<string, LoadedModel> _models = new();

        private ModelParams Params;

        private LLamaWeights Model;

        public Executor Executor { get; private set; }

        private LoadedModel(ModelParams @params, LLamaWeights model)
        {
            Params = @params;
            Model = model;
            Executor = new Executor(Model, Params);
        }
        
        async static public Task<LoadedModel> LoadOrGetModelAsync(string modelId)
        {
            // TODO: Avoid reloading the same model twice due to concurrent calls to load the same model
            if (_models.TryGetValue(modelId, out var loaded))
                return loaded;

            loaded = await LoadModelAsync(modelId);
            _models[modelId] = loaded;
            return loaded;
        }

        private static async Task<LoadedModel> LoadModelAsync(string modelFileName)
        {
            var parameters = CatalogModel.GetModelParms(modelFileName);
            LLamaWeights weights = await LLamaWeights.LoadFromFileAsync(parameters);
            
            return new LoadedModel(parameters, weights);
        }

        public void Dispose()
        {
            Model?.Dispose();
        }
    }
}
