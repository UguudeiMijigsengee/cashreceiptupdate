using Model.Model.Consolidate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Persistence.Consolidate
{
    public interface IConsolidateRepository
    {
        Task runConsolidateMergeMain(string mainSP);
        Task<List<ConsolidateErrorLogTable>> runConsolidateErrorTb(string errorQuery);
        Task<List<ConsolidateSuccessLogTable>> runConsolidateSuccessTb(string successQuery);
    }
}
