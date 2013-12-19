using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;
using System.ComponentModel.Composition;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(CaptureViewModel))]
    public class CaptureViewModel : IShellView
    {
    }
}
