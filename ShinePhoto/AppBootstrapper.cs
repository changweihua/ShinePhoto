﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShinePhoto.ViewModels;
using Caliburn.Micro;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using ShinePhoto.Extensions;

namespace ShinePhoto
{
    class AppBootstrapper : Bootstrapper<LoginViewModel>
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
            base.OnStartup(sender, e);
        }

    }
}
