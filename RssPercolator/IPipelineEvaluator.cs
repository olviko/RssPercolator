using System;
using System.Collections.Generic;

namespace RssPercolator
{
    public interface IPipelineEvaluator
    {
        void Execute(IList<IFilter> filters, PipelineSettings pipelineSettings);
    }
}
