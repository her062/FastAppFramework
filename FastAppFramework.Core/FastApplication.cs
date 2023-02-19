using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

namespace FastAppFramework.Core
{
    public abstract class FastApplication : PrismApplication
    {
#region Properties
        public static new FastApplication Current
        {
            get => (FastApplication)PrismApplication.Current;
        }

        public IApplicationEnvironment Environment
        {
            get => this._env;
        }
        public ILogger Logger
        {
            get => this._logger;
        }
        public IApplicationSettingProvider Settings
        {
            get => this._settingContainer!;
        }

        public virtual string SettingVersion
        {
            get => "1.0";
        }
#endregion

#region Fields
        private IApplicationEnvironment _env;
        private ILogger _logger;
        private ApplicationSettingContainer? _settingContainer;
#endregion

#region Constructor/Destructor
        protected FastApplication()
        {
            // Setup Fields.
            {
                this._env = SetupEnvironment();
                this._logger = CreateLogger();
            }
        }
#endregion

#region Protected Functions
        protected virtual IApplicationEnvironment SetupEnvironment() => new ApplicationEnvironment();
        protected virtual ILogger CreateLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            {
                config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Off, new NLog.Targets.FileTarget(){
                    FileName = System.IO.Path.Combine(this.Environment.LogFolder, string.Format("{0}.log", DateTime.Today.ToString("yyyyMMdd"))),
                    Layout = "${longdate}|${level}|${callsite}|${message}"
                });
                config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Off, new NLog.Targets.ConsoleTarget(){
                    Layout = "${longdate}|${level}|${callsite}|${message}"
                });
            }
            NLog.LogManager.Configuration = config;

            ILogger? logger = null;
            using (var factory = new NLog.Extensions.Logging.NLogLoggerFactory())
                logger = factory.CreateLogger(string.Empty);
            return logger;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var catalog = this.Container.Resolve<IModuleCatalog>();
            // Register setting types in modules.
            {
                foreach (var item in catalog.Modules)
                {
                    var module = item as IFastAppModule;
                    module?.RegisterSettingTypes(this._settingContainer!);
                }
                // Load application settings from a file.
                this.Settings.Load();
            }

            this.Logger.LogDebug("");
        }
        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);

            this._settingContainer = new ApplicationSettingContainer(containerRegistry);
            containerRegistry.RegisterInstance<IApplicationSettingProvider>(this._settingContainer);
            // Register setting types.
            RegisterSettingTypes(this._settingContainer);

            this.Logger.LogDebug("");
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
        protected virtual void RegisterSettingTypes(IApplicationSettingRegistry settingRegistry)
        {
        }
#endregion
    }
}