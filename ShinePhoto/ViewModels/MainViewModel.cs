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
    [Export(typeof(MainViewModel))]
    public class MainViewModel : PropertyChangedBase, IHandle<ModuleChangedEvent>
    {
        private readonly IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public MainViewModel(LeftViewModel leftViewModel, CaptureViewModel captureViewModel, IEventAggregator eventAggregator)
        {
            LeftViewModel = leftViewModel;
            ShellViewModel = captureViewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public LeftViewModel LeftViewModel { get; private set; }

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

        public void Handle(ModuleChangedEvent message)
        {
            //ShellViewModel = message.ShellView;
            var type = message.ModuleType;
            ShellViewModel = IoC.GetInstance(type, "") as IShellView;
            Debug.WriteLine("新模块编号" + message.ModuleName);
        }
    }
}
