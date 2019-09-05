using DataAccess.Persistence.UnitOfWork;
using DataAccess.Persistence.Consolidate;
using DataAccess.Services;
using Microsoft.Extensions.Options;
using Model.Model;
using Model.Model.Consolidate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Consolidate
{
    public class ConsolidateService : IConsolidateService
    {
        private readonly Config _config;
        private readonly IConsolidateRepository consolidateRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;

        public ConsolidateService(IOptions<Config> config,
                                  IConsolidateRepository consolidateRepository,
                                  IUnitOfWork unitOfWork,
                                  IUserService userService)
        {
            _config = config.Value;
            this.consolidateRepository = consolidateRepository;
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }

        public async Task<ConsolidateResponse> ConsolidateLoad(ConsolidateRequest request, string userName)
        {
            var consolidateResponse = new ConsolidateResponse();

            if (!userService.isUserMemberOfGroup(_config.UserServiceConfig.UserRoutes[2].group, userName))
            {
                consolidateResponse.isConsolidated = false;
                consolidateResponse.message.Add(String.Format(_config.UserServiceConfig.accountMessages[2], userName, _config.UserServiceConfig.UserRoutes[2].header));
            }

            try
            {

            if (consolidateResponse.isConsolidated)
            {
                // Run main SP
                await consolidateRepository.runConsolidateMergeMain(String.Format(_config.ConsolidateConfig.mainStoredProcedures[0], request.load));

                // Check in ErrorLogTable
                var consolidateErrorLogTables = await consolidateRepository.runConsolidateErrorTb(String.Format(_config.ConsolidateConfig.errorLogTables[0], request.load));

                    // Check in SuccessLogTable
                if (consolidateErrorLogTables.Count == 0)
                {
                    var consolidateSuccessLogTables = await consolidateRepository.runConsolidateSuccessTb(String.Format(_config.ConsolidateConfig.successLogTables[0], request.load));
                    if (consolidateSuccessLogTables.Count > 0)
                    {
                        consolidateResponse.isConsolidated = true;
                        consolidateResponse.message.Add(String.Format(_config.ConsolidateConfig.successLogTableMessages[0], request.load, consolidateSuccessLogTables[0].dtstamp, consolidateSuccessLogTables[0].TMWMove));
                    }
                    else
                    {
                        consolidateResponse.isConsolidated = false;
                        consolidateResponse.message.Add(String.Format(_config.ConsolidateConfig.successLogTableMessages[1], request.load));
                    }
                }
                else
                {
                    consolidateResponse.isConsolidated = false;
                    consolidateResponse.message.Add(consolidateErrorLogTables[0].ErrorString);
                }
            }
            } catch (Exception ex)
            {

            }


            return consolidateResponse;
        }

        public async Task<ConsolidateMergeResponse> MergeLoads(ConsolidateMergeRequest request, string userName)
        {
            var mergeResponse = new ConsolidateMergeResponse();

            if (!userService.isUserMemberOfGroup(_config.UserServiceConfig.UserRoutes[2].group, userName))
            {
                mergeResponse.isMerged = false;
                mergeResponse.message.Add(String.Format(_config.UserServiceConfig.accountMessages[2], userName, _config.UserServiceConfig.UserRoutes[2].header));
            }

            if (mergeResponse.isMerged)
            {
                //load1,load2
                var loads_commaS = String.Join(_config.ConsolidateConfig.mergeHelper[0], request.consolidatedLoads);
                //load1','load2
                var loads_quote_comma = String.Join(_config.ConsolidateConfig.mergeHelper[1], request.consolidatedLoads);
                // Run main SP
                await consolidateRepository.runConsolidateMergeMain(String.Format(_config.ConsolidateConfig.mainStoredProcedures[1], loads_commaS, userName));

                // Check in ErrorLogTable
                var consolidateErrorLogTables = await consolidateRepository.runConsolidateErrorTb(String.Format(_config.ConsolidateConfig.errorLogTables[1], loads_commaS, loads_quote_comma));

                // Check in SuccessLogTable
                if (consolidateErrorLogTables.Count == 0)
                {
                    var consolidateSuccessLogTables = await consolidateRepository.runConsolidateSuccessTb(String.Format(_config.ConsolidateConfig.successLogTables[1], loads_quote_comma));
                    if (consolidateSuccessLogTables.Count > 0)
                    {
                        mergeResponse.isMerged = true;
                        mergeResponse.message.Add(String.Format(_config.ConsolidateConfig.successLogTableMessages[0], loads_commaS, consolidateSuccessLogTables[0].dtstamp, consolidateSuccessLogTables[0].TMWMove));
                    }
                    else
                    {
                        mergeResponse.isMerged = false;
                        mergeResponse.message.Add(String.Format(_config.ConsolidateConfig.successLogTableMessages[1], loads_commaS));
                    }
                }
                else
                {
                    mergeResponse.isMerged = false;
                    mergeResponse.message.Add(consolidateErrorLogTables[0].ErrorString);
                }
            }

            return mergeResponse;
        }
    }
}
