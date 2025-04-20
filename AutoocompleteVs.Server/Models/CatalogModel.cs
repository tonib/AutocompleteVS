using LLama.Common;

namespace AutoocompleteVs.Server.Models
{
    /// <summary>
    /// Catalog of available models
    /// TODO: Load this from appsettings.json
    /// </summary>
    public class CatalogModel
    {
        static private Dictionary<string, ModelParams>? Catalog;
        
        static public ModelParams GetModelParms(string modelFileName)
        {
            SetupCatalog();

            if(!Catalog!.TryGetValue(modelFileName, out ModelParams? modelParams))
			{
				throw new Exception($"Model {modelFileName} not found in catalog");
			}
            return modelParams;
		}

        static private void SetupCatalog()
        {
			// TODO: Load this from appsettings.json
			if (Catalog != null)
                return;

			Catalog = new Dictionary<string, ModelParams>();

			ModelParams modelParams = new ModelParams(@"..\Models\qwen2.5-coder-1.5b-q8_0.gguf");
            modelParams.ContextSize = 2048;
			modelParams.GpuLayerCount = 32;
            Catalog["qwen2.5-coder-1.5b-q8_0.gguf"] = modelParams;
		}
	}
}
