using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ShinePhoto.Interfaces;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(IntroductionViewModel))]
    public class IntroductionViewModel : Screen, IShellView
    {
    }
}
