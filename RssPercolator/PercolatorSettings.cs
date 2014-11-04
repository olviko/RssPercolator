using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace RssPercolator
{
    /// <summary>
    /// Pipeline settings.
    /// </summary>
    public sealed class PercolatorSettings
    {
        public string Version { get; private set; }

        public IList<PipelineSettings> Pipelines { get; set; }

        public IList<FilterSettings> Filters { get; set; }

        public PercolatorSettings()
        {
            Version = "1.0";
        }

        /// <summary>
        /// Saves settings to a file.
        /// </summary>
        public void SaveToFile(string path)
        {
            File.WriteAllText(path, this.ToString());
        }

        /// <summary>
        /// Loads settings from a file.
        /// </summary>
        public static PercolatorSettings LoadFromFile(string path)
        {
            return LoadFromString(File.ReadAllText(path));
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, serializerSettings);
        }

        /// <summary>
        /// Loads settings from a string.
        /// </summary>
        public static PercolatorSettings LoadFromString(string source)
        {
            return JsonConvert.DeserializeObject<PercolatorSettings>(source, serializerSettings);
        }

        private static readonly JsonSerializerSettings serializerSettings =
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] 
                    { 
                        new StringEnumConverter { CamelCaseText = true }
                    }
            };
    }
}
