using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(AdViewModel))]
    public class AdViewModel : Screen
    {
        //[ImportingConstructor]
        public AdViewModel()
        {

        }

    }
}
