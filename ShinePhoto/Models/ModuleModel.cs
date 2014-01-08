using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace ShinePhoto.Models
{
    public class ModuleModel : PropertyChangedBase
    {
        public string Icon { get; set; }
        public string ModuleName { get; set; }
        public int Tag { get; set; }
    }
}
