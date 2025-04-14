namespace AutoocompleteVs.Server.Hubs
{
    /// <summary>
    /// Defines the contract for inference services
    /// </summary>
    public interface IInferenceHub
    {

        /// <summary>
        /// Just a ping to check if the server is alive
        /// </summary>
        /// <returns>"Pong" string</returns>
        Task<string> Ping();

    }
}
