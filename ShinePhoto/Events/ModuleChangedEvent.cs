using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;

namespace ShinePhoto.Events
{
    public class ModuleChangedEvent
    {
        public ModuleChangedEvent(IShellView shellView, string moduleName, Type moduleType)
        {
            ShellView = shellView;
            ModuleName = moduleName;
            ModuleType = moduleType;
        }

        public IShellView ShellView { get; private set; }
        public string ModuleName { get; private set; }
        public Type ModuleType { get; private set; }
    }
}
