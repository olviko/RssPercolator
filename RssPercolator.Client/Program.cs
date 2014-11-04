using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssPercolator.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string settingsFile = "";

            //GetSamplePipelineSettings().SaveToFile(settingsFile);

            PercolatorSettings config = PercolatorSettings.LoadFromFile(settingsFile);

            IFilter[] filters = config.Filters.Select(filterSettings => Filter.Create(filterSettings)).ToArray();

            IPipelineEvaluator pipeline = PipelineEvaluator.Create();

            foreach (var pipelineSettings in config.Pipelines)
            {
                if (!Path.IsPathRooted(pipelineSettings.Output))
                {
                    pipelineSettings.Output = Path.Combine(Path.GetDirectoryName(settingsFile), pipelineSettings.Output);
                }

                pipeline.Execute(filters, pipelineSettings);
            }
        }
    }
}
