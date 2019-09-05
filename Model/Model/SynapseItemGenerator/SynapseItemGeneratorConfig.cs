using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace Model.Model.SynapseItemGenerator
{
    public class SynapseItemGeneratorConfig
    {
        public string fileExtension { get; set; }
        public string dateFormat { get; set; }
        public string dropLocation { get; set; }
        public string archiveLocation { get; set; }
        public List<string> splitLocations { get; set; }
        public int MaxBytes { get; set; }
        public string[] AcceptedFileTypes { get; set; }
        public List<string> messages { get; set; }
        public bool IsSupported(string fileName)
        {
            return AcceptedFileTypes.Any(s => s == Path.GetExtension(fileName).ToLower());
        }
    }
}
