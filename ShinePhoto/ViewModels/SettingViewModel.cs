using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(SettingViewModel))]
    public class SettingViewModel : Screen
    {
        protected override void OnActivate()
        {
            System.Diagnostics.Debug.WriteLine("Page Two Activated");
            base.OnActivate();
        }
    }
}
