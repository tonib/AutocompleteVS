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

        public required CatalogModel CatalogModel { get; set; }

        public required ModelParams Params { get; set; }

        public required LLamaWeights Model { get; set; }

        async static public Task<LoadedModel> LoadOrGetModelAsync(string modelId)
        {
            // TODO: Avoid reloading the same model twice due to concurrent calls to load the same model
            if (_models.TryGetValue(modelId, out var loaded))
                return loaded;

            loaded = await LoadModelAsync(modelId);
            _models[modelId] = loaded;
            return loaded;
        }

        private static async Task<LoadedModel> LoadModelAsync(string modelId)
        {
            CatalogModel catalogModel = CatalogModel.Catalog[modelId];
            // TODO: Configure this
            var parameters = new ModelParams(catalogModel.Path);
            parameters.ContextSize = 2048;
            parameters.GpuLayerCount = 32;
            LLamaWeights weights = await LLamaWeights.LoadFromFileAsync(parameters);
            return new LoadedModel { CatalogModel = catalogModel, Params = parameters, Model = weights };
        }

        public void Dispose()
        {
            Model?.Dispose();
        }
    }
}
