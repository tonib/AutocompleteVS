﻿using System.Threading.Tasks;

namespace AutocompleteVs.Client
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
        Task<string> PingAsync();

        Task<string> StartInferenceAsync(string modelId, InferenceRequest prompt, string[] validWords);

        Task<string> ContinueInferenceAsync(string[] validWords);


    }
}
