using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.Interfaces;
using ShinePhoto.Interface;

namespace ShinePhoto.Events
{
    /// <summary>
    /// 单击左侧导航按钮事件
    /// </summary>
    public class ModuleChangedEvent
    {
        public ModuleChangedEvent(IShellView shellView, string moduleName, Type moduleType)
        {
            ShellView = shellView;
            ModuleName = moduleName;
            ModuleType = moduleType;
        }

        /// <summary>
        /// 目标视图
        /// </summary>
        public IShellView ShellView { get; private set; }

        /// <summary>
        /// 目标视图名
        /// </summary>
        public string ModuleName { get; private set; }

        /// <summary>
        /// 目标视图真实类型
        /// </summary>
        public Type ModuleType { get; private set; }
    }
}
