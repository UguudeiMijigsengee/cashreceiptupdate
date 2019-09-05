using Microsoft.AspNetCore.Http;
using Model.Model.SynapseItemGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.SynapseItemGenerator
{
    public interface ISynapseItemGeneratorService
    {
        Task<SynapseItemGeneratorResponse> GenerateItem(IFormFile file, string userName);
        bool SplitFile(string filePath);
    }
}
