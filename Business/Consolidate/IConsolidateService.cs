using Model.Model.Consolidate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Consolidate
{
    public interface IConsolidateService
    {
        Task<ConsolidateResponse> ConsolidateLoad(ConsolidateRequest request, string userName);
        Task<ConsolidateMergeResponse> MergeLoads(ConsolidateMergeRequest request, string userName);
    }
}
