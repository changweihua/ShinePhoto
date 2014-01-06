using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.ViewModels;
using Caliburn.Micro;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using ShinePhoto.Extensions;
using ShinePhoto.Helpers;

namespace ShinePhoto
{
    class AppBootstrapper : Bootstrapper<MainViewModel>
    {
        /// <summary>
        /// 聚合容器，可以考虑使用 MEF
        /// </summary>
        private CompositionContainer _container;

        /// <summary>
        /// 重载配置方法
        /// </summary>
        protected override void Configure()
        {
            _container = new CompositionContainer(new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue<IWindowManager>(new FlatWindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);
            //模型验证
            ConventionManager.ApplyValidation = (binding, viewModelType, property) =>
            {
                //binding.ValidatesOnDataErrors = true;
                //binding.NotifyOnValidationError = true;
                binding.ValidatesOnExceptions = true;
            };

            LogManager.GetLog = (type) => new Log4netLogger(type);

            _container.Compose(batch);
        }

        protected override object GetInstance(Type service, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;

            var exports = _container.GetExportedValues<object>(contract);

            if (exports.Count() > 0)
            {
                return exports.First();
                
            }

            throw new Exception(string.Format("无法加载契约 {0} 的任何实例.", contract));

        }

        /// <summary>
        /// 改变启动方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            //Application
            LogManager.GetLog(typeof(AppBootstrapper)).Info("\r\n操作系统信息\r\n序列号 {0}\r\nCPU 编号 {1}\r\n硬盘编号 {2}\r\n主板编号 {3}\r\n网卡编号 {4}\r\n用户组 {5}\r\n驱动器情况 {6}", SystemInfoHelper.GetSerialNumber(), SystemInfoHelper.GetCpuID(), SystemInfoHelper.GetMainHardDiskId(), SystemInfoHelper.GetMainBoardId(), SystemInfoHelper.GetNetworkAdapterId(), SystemInfoHelper.GetGroupName(), SystemInfoHelper.GetDriverInfo());

            #region 多语言

            System.Globalization.CultureInfo currentCultureInfo = System.Globalization.CultureInfo.CurrentCulture;

            System.Windows.ResourceDictionary langRd = null;

            try
            {
                langRd =System.Windows.Application.LoadComponent(new Uri(@"Lang\" + currentCultureInfo.Name + ".xaml", UriKind.Relative)) as System.Windows.ResourceDictionary;
            }
            catch
            {
            }

            if (langRd != null)
            {
                if (this.Application.Resources.MergedDictionaries.Count > 0)
                {
                    this.Application.Resources.MergedDictionaries.RemoveAt(0);
                }
                this.Application.Resources.MergedDictionaries.Insert(0, langRd);
            }

            #endregion

            base.OnStartup(sender, e);
        }

    }
}
