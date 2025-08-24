using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.Config
{
    /// <summary>
    /// Autocomplete configuration
    /// </summary>
    public class AutocompleteConfig
    {
        /// <summary>
        /// Autocomplete configuration id
        /// </summary>
        public string Id { get; set; }

        [DisplayName("Max. number of characters in prompt")]
        [Description("Maximum number of characters to send as prompt. Empty == send all file")]
        public int? MaxPromptCharacters { get; set; } = 2048;

        [DisplayName("Prefix % when max. characters is reached")]
        [Description("Only applies the current file has a size lager than the max. number of characters in prompt. " +
            "As only that maximum number of characters will be sent, this is the % of characters to get from before the caret position." +
            "The remaining % will be characters after caret position. Between 0 and 100")]
        public float? InfillPrefixPercentage { get; set; } = 75.0f;

        [Browsable(false)]
        public double? InfillSuffixPercentage => 100.0 - InfillPrefixPercentage;

        /// <summary>
        /// Model id to use to autocomplete (references to IModelConfig)
        /// </summary>
        public string ModelConfigId { get; set; }

        /// <summary>
        /// Model id to use to autocomplete
        /// </summary>
        public IModelConfig ModelConfig =>
            AutocompleteVsPackage.Instance.Settings.Models.FirstOrDefault(m => m.Id == ModelConfigId);

        override public string ToString()
        {
            string txt = Id + " / " + ModelConfig?.ToString() ?? "???";
            if (MaxPromptCharacters != null)
                txt += $" / {MaxPromptCharacters}) char.";
            return txt;
        }

    }
}
