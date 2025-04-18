namespace AutoocompleteVs.Server.Models
{
    /// <summary>
    /// Catalog of available models
    /// TODO: Load this from appsettings.json
    /// </summary>
    public class CatalogModel
    {
        static public readonly Dictionary<string, CatalogModel> Catalog = new()
        {
            {"Qwen2.5-Coder-1.5B", new CatalogModel("Qwen2.5-Coder-1.5B", @"..\..\..\..\..\Models\Qwen2.5-Coder-1.5B.Q8_0.gguf")}
        };

        /// <summary>
        /// The name of the model
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The path to the model file
        /// </summary>
        public string Path { get; set; }

        private CatalogModel(string id, string path)
        {
            Id = id;
            Path = path;
        }

    }
}
