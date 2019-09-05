using DataAccess.Persistence.Log;
using DataAccess.Persistence.UnitOfWork;
using DataAccess.Services;
using Microsoft.Extensions.Options;
using Model.Model;
using Model.Model.SynapseItemGenerator;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Business.SynapseItemGenerator
{
    public class SynapseItemGeneratorService : ISynapseItemGeneratorService
    {
        private readonly Config _config;
        private readonly ILogRepository logRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;

        public SynapseItemGeneratorService(IOptions<Config> config,
                                           ILogRepository logRepository,
                                           IUnitOfWork unitOfWork,
                                           IUserService userService)
        {
            _config = config.Value;
            this.logRepository = logRepository;
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }

        public async Task<SynapseItemGeneratorResponse> GenerateItem(IFormFile file, string userName)
        {
            var synapseItemGeneratorResponse = new SynapseItemGeneratorResponse();
            var fileUplaoded = false;
            var originalFileName = "";
            var filePath = "";

            if (!userService.isUserMemberOfGroup(_config.UserServiceConfig.UserRoutes[3].group, userName))
            {
                synapseItemGeneratorResponse.isUploaded = false;
                synapseItemGeneratorResponse.message.Add(String.Format(_config.UserServiceConfig.accountMessages[2], userName, _config.UserServiceConfig.UserRoutes[3].header));
            }                       

            if (synapseItemGeneratorResponse.isUploaded)
            {
                // Check if null or empty file is uploaded
                if (file == null || file.Length == 0)
                {
                    synapseItemGeneratorResponse.isUploaded = false;
                    synapseItemGeneratorResponse.message.Add(_config.SynapseItemGeneratorConfig.messages[0]);
                }

                // Check if max file size is exceeded
                if (file.Length > _config.SynapseItemGeneratorConfig.MaxBytes)
                {
                    synapseItemGeneratorResponse.isUploaded = false;
                    synapseItemGeneratorResponse.message.Add(String.Format(_config.SynapseItemGeneratorConfig.messages[1], file.FileName));
                }

                if (synapseItemGeneratorResponse.isUploaded)
                {
                    // Check if file type is supported
                    if (!_config.SynapseItemGeneratorConfig.IsSupported(file.FileName))
                    {
                        synapseItemGeneratorResponse.isUploaded = false;
                        synapseItemGeneratorResponse.message.Add(_config.SynapseItemGeneratorConfig.messages[2]);
                    }

                    if (synapseItemGeneratorResponse.isUploaded)
                    {
                        if (!Directory.Exists(_config.SynapseItemGeneratorConfig.dropLocation))
                            Directory.CreateDirectory(_config.SynapseItemGeneratorConfig.dropLocation);

                        originalFileName = file.FileName;
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        filePath = Path.Combine(_config.SynapseItemGeneratorConfig.dropLocation, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            fileUplaoded = true;                            
                        }
                    }
                }

                if (fileUplaoded)
                {
                    // start splitting the file
                    if (SplitFile(filePath))
                    {
                        // Splitted successfully
                        synapseItemGeneratorResponse.isUploaded = true;
                        synapseItemGeneratorResponse.message.Add(String.Format(_config.SynapseItemGeneratorConfig.messages[4], originalFileName));
                    }
                    else
                    {
                        // Issue occurred during the split
                        synapseItemGeneratorResponse.isUploaded = false;
                        synapseItemGeneratorResponse.message.Add(String.Format(_config.SynapseItemGeneratorConfig.messages[3], originalFileName));
                    }
                }
            }

            return synapseItemGeneratorResponse;
        }

        public bool SplitFile(string filePath)
        {
            var splitted = false;
            var dropFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString(_config.SynapseItemGeneratorConfig.dateFormat) + _config.SynapseItemGeneratorConfig.fileExtension;
            List<Alias> aliases = new List<Alias>();
            List<BaseUom> baseuoms = new List<BaseUom>();
            List<ItemName> itemNames = new List<ItemName>();
            List<ItemSpecs> itemSpecss = new List<ItemSpecs>();
            List<UOMSeq> uOMSeqs = new List<UOMSeq>();
            List<string> lines = File.ReadAllLines(filePath).ToList();
            lines = lines.Skip(1).ToList();

            try
            {             
                foreach (string line in lines)
                {
                    //ItemImports
                    #region variables
                    string[] entries = line.Split(',');
                    string custid = entries[0];
                    string item = entries[1];
                    string descr = entries[2].ToString().Replace(",", "_");
                    string abbrev = entries[3];
                    string rategroup = entries[4];
                    string status = entries[5];
                    //string review = "Y";
                    string itmpercase = entries[7];
                    string eapercase = entries[8];
                    string itmprodgroup = entries[9];
                    string ti = entries[10];
                    string hi = entries[11];
                    string nmfc = entries[12];
                    string ltlclass = entries[13];
                    string itmpassthruchar03 = entries[14];
                    string itmpassthruchar0 = entries[15];
                    string baseuom = entries[16];
                    string cubeft = entries[17];
                    string pcubeft = entries[18];
                    string pltheight = entries[19];
                    string velocity = entries[20];
                    string picktotype = entries[21];
                    string contype = entries[22];
                    string length = entries[23];
                    string width = entries[24];
                    string height = entries[25];
                    string weight = entries[26];
                    string seq1 = entries[27];
                    string seq1qty = entries[28];
                    string seq1frmuom = entries[29];
                    string seq1inuom = entries[30];
                    string seq1picktotype = entries[31];
                    string seq1velocity = entries[32];
                    string seq2 = entries[33];
                    string seq2qty = entries[34];
                    string seq2frmuom = entries[35];
                    string seq2inuom = entries[36];
                    string seq2picktotype = entries[37];
                    string seq2velocity = entries[38];
                    string walmart = entries[39];
                    string gtin = entries[40];
                    string upc = entries[41];
                    string shelflife = entries[42];
                    #endregion
                    #region BaseUom
                    if (custid.Length > 0 & item.Length > 0 & baseuom.Length > 0 & weight.Length > 0 & velocity.Length > 0 & picktotype.Length > 0)
                    {
                        baseuoms.Add(new BaseUom { Ibu = "IBU", Custid = custid, Item = item, Baseuom = baseuom, Weight = weight, Velocity = velocity, Picktotype = picktotype, ContainerType = contype, Length = length, Width = width, Height = height });
                    }
                    List<string> strbaseuoms = new List<string>();
                    foreach (var baseuomm in baseuoms)
                    {
                        strbaseuoms.Add($"{baseuomm.Ibu},{baseuomm.Custid},{baseuomm.Item},{baseuomm.Baseuom},{baseuomm.Weight},{baseuomm.Velocity},{baseuomm.Picktotype},{baseuomm.ContainerType},{baseuomm.Length},{baseuomm.Width},{baseuomm.Height}");
                    }
                    File.WriteAllLines(_config.SynapseItemGeneratorConfig.splitLocations[1] + dropFileName, strbaseuoms);
                    /*
                    try
                    {
                        using (StreamWriter baseuomfile = new StreamWriter(@baseUomPath, true))
                        {
                            baseuomfile.WriteLine(strbaseuoms.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }*/
                    #endregion
                    #region Create Walmart Alias
                    if (walmart.Length > 6)
                    {
                        aliases.Add(new Alias { Qualifer = "IAL", Custid = entries[0], Item = entries[1], Aliass = walmart, AliasDescr = "WalMart Item #" });
                    }
                    if (gtin.Length > 6)
                    {
                        aliases.Add(new Alias { Qualifer = "IAL", Custid = entries[0], Item = entries[1], Aliass = gtin, AliasDescr = "GTIN #" });
                    }
                    if (upc.Length > 6)
                    {
                        aliases.Add(new Alias { Qualifer = "IAL", Custid = entries[0], Item = entries[1], Aliass = upc, AliasDescr = "UPC #" });
                    }
                    List<string> straliases = new List<string>();
                    foreach (var alias in aliases)
                    {
                        straliases.Add($"{alias.Qualifer},{alias.Custid},{alias.Item},{alias.Aliass},{alias.AliasDescr}");
                    }
                    File.WriteAllLines(_config.SynapseItemGeneratorConfig.splitLocations[0] + dropFileName, straliases);
                    /*
                    try
                    {
                        using (StreamWriter aliasfile = new StreamWriter(@aliasPath, true))
                        {
                            aliasfile.WriteLine(straliases.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    */
                    #endregion
                    #region ItemName
                    if (custid.Length > 0 & item.Length > 0 & descr.Length > 0 & abbrev.Length > 0 & rategroup.Length > 0 & status.Length > 0)
                    {
                        itemNames.Add(new ItemName { INM = "INM", Custid = custid, Item = item, Descr = descr.ToString().Replace(",", ""), Abbrev = abbrev, Rategroup = rategroup, Status = "ACTV" });
                    }
                    List<string> stritemNames = new List<string>();
                    foreach (var itmname in itemNames)
                    {
                        stritemNames.Add(itmname.INM + "," + itmname.Custid + "," + itmname.Item + "," + itmname.Descr.ToString().Replace(",", "").Replace(",", "") + "," + itmname.Abbrev + "," + itmname.Rategroup + "," + itmname.Status);
                    }
                    //stritemNames = stritemNames.ToString().Replace("\"", "");
                    //stritemNames = stritemNames.ToString().Replace("\"", "");
                    File.WriteAllLines(_config.SynapseItemGeneratorConfig.splitLocations[2] + dropFileName, stritemNames);//.ToString().Replace("\"", ""));
                                                                                                                          /*
                                                                                                                          try
                                                                                                                          {
                                                                                                                              using (StreamWriter itemnamefile = new StreamWriter(@itemNamePath, true))
                                                                                                                              {
                                                                                                                                  itemnamefile.WriteLine(stritemNames.ToString().Replace("\"", ""));
                                                                                                                              }
                                                                                                                          }
                                                                                                                          catch (Exception)
                                                                                                                          {
                                                                                                                              throw;
                                                                                                                          }
                                                                                                                          */
                    #endregion
                    #region ItemSpecs
                    if (custid.Length > 0 & item.Length > 0 & itmprodgroup.Length > 0)
                    {
                        itemSpecss.Add(new ItemSpecs { ISP = "ISP", Custid = custid, Item = item, Shelflife = shelflife, Itmpercase = itmpercase, Eapercase = eapercase, Itmprodgroup = itmprodgroup, Ti = ti, Hi = hi, Nmfc = nmfc, Ltlclass = ltlclass });
                    }
                    List<string> stritemspecs = new List<string>();
                    foreach (var itmspec in itemSpecss)
                    {
                        stritemspecs.Add($"{itmspec.ISP},{itmspec.Custid},{itmspec.Item},{itmspec.Shelflife},,,{itmspec.Itmprodgroup},{itmspec.Nmfc},,,,{itmspec.Ltlclass},,,,{itmspec.Itmpercase},{itmspec.Ti},{itmspec.Hi},,,,,");
                    }
                    File.WriteAllLines(_config.SynapseItemGeneratorConfig.splitLocations[3] + dropFileName, stritemspecs);
                    #endregion
                    #region UomSeq
                    if (custid.Length > 0 & item.Length > 0 & seq1.Length > 0 & seq1qty.Length > 0 & seq1frmuom.Length > 0 & seq1inuom.Length > 0 & seq1picktotype.Length > 0 & seq1velocity.Length > 0)
                    {
                        uOMSeqs.Add(new UOMSeq { Ius = "IUS", Custid = custid, Item = item, Sequence = seq1, Qty = seq1qty, Fromuom = seq1frmuom, Inuom = seq1inuom, Picktotype = seq1picktotype, Velocity = seq1velocity });
                    }
                    if (custid.Length > 0 & item.Length > 0 & seq2.Length > 0 & seq2qty.Length > 0 & seq2frmuom.Length > 0 & seq2inuom.Length > 0 & seq2picktotype.Length > 0 & seq2velocity.Length > 0)
                    {
                        uOMSeqs.Add(new UOMSeq { Ius = "IUS", Custid = custid, Item = item, Sequence = seq2, Qty = seq2qty, Fromuom = seq2frmuom, Inuom = seq2inuom, Picktotype = seq2picktotype, Velocity = seq2velocity });
                    }
                    List<string> struomseq = new List<string>();
                    foreach (var uomseqq in uOMSeqs)
                    {
                        struomseq.Add($"{uomseqq.Ius},{uomseqq.Custid},{uomseqq.Item},{uomseqq.Sequence},{uomseqq.Qty},{uomseqq.Fromuom},{uomseqq.Inuom},,,,,,,,,");
                    }
                    File.WriteAllLines(_config.SynapseItemGeneratorConfig.splitLocations[4] + dropFileName, struomseq);
                    #endregion
                }


                splitted = true;
                File.Move(filePath, _config.SynapseItemGeneratorConfig.archiveLocation + Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {

            }

            
            
            return splitted;
        }
    }
}
