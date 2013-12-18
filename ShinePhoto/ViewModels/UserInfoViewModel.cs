using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace ShinePhoto.ViewModels
{
    [Export(typeof(UserInfoViewModel))]
    public class UserInfoViewModel : Screen
    {
    }
}
