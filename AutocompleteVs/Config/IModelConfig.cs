using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.Config
{
    /// <summary>
    /// Settings for a model
    /// </summary>
    public interface IModelConfig
    {
        /// <summary>
        /// Model configuration id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Model type
        /// </summary>
        ModelType Type { get; }

        /// <summary>
        /// True if it's an infill base model
        /// </summary>
        bool IsInfillModel { get; }
    }
}
