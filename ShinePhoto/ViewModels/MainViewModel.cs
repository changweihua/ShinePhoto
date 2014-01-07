using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using ShinePhoto.Events;
using System.Diagnostics;
using ShinePhoto.Interfaces;

namespace ShinePhoto.ViewModels
{
    /// <summary>
    /// 主视图
    /// </summary>
    [Export(typeof(MainViewModel))]
    public class MainViewModel : Screen, IHandle<ModuleChangedEvent>
    {
        private readonly IEventAggregator _eventAggregator;

        public MainViewModel()
        {

        }

        [ImportingConstructor]
        public MainViewModel(LeftViewModel leftViewModel,IntroductionViewModel viewModel, IEventAggregator eventAggregator)
        {
            LeftViewModel = leftViewModel;
            ShellViewModel = viewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        /// <summary>
        /// 左侧视图
        /// </summary>
        public LeftViewModel LeftViewModel { get; private set; }

        /// <summary>
        /// 右侧视图
        /// </summary>
        private IShellView _shellViewModel;
        public IShellView ShellViewModel
        {
            get
            {
                return _shellViewModel;
            }
            set
            {
                _shellViewModel = value;
                NotifyOfPropertyChange(() => ShellViewModel);
            }
        }

        /// <summary>
        /// 监听并处理左侧导航事件
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ModuleChangedEvent message)
        {
            //ShellViewModel = message.ShellView;
            var type = message.ModuleType;
            ShellViewModel = IoC.GetInstance(type, "") as IShellView;
            Debug.WriteLine("新模块编号" + message.ModuleName);
        }
    }
}
