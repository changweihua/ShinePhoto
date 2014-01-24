using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShinePhoto.Models
{
    public class OSVersionInfoModel
    {
        public string Name { get; set; }
        public string Edition { get; set; }
        public string ServicePack { get; set; }
        public string Version { get; set; }
        public string ProcessorBits { get; set; }
        public string OSBits { get; set; }
        public string ProgramBits { get; set; }
    }
}
