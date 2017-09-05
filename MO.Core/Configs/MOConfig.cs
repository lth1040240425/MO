using MO.Core.Configs.ConfigFile;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Core.Configs
{
    /// <summary>
    /// OA配置类
    /// </summary>
    public sealed class MOConfig
    {
        private const string MOSectionName = "MO";
        private static readonly Lazy<MOConfig> InstanceLazy
            = new Lazy<MOConfig>(() => new MOConfig());

        /// <summary>
        /// 初始化一个新的<see cref="MOConfig"/>实例
        /// </summary>
        private MOConfig()
        {
            MOFrameworkSection section = (MOFrameworkSection)ConfigurationManager.GetSection(MOSectionName);
            if (section == null)
            {
                DataConfig = new DataConfig();
                LoggingConfig = new LoggingConfig();
                return;
            }
            DataConfig = new DataConfig(section.Data);
            LoggingConfig = new LoggingConfig(section.Logging);
        }

        /// <summary>
        /// 获取 配置类的单一实例
        /// </summary>
        public static MOConfig Instance
        {
            get
            {
                MOConfig config = InstanceLazy.Value;
                if (DataConfigReseter != null)
                {
                    config.DataConfig = DataConfigReseter.Reset(config.DataConfig);
                }
                if (LoggingConfigReseter != null)
                {
                    config.LoggingConfig = LoggingConfigReseter.Reset(config.LoggingConfig);
                }
                return config;
            }
        }

        /// <summary>
        /// 获取或设置 数据配置重置信息
        /// </summary>
        public static IDataConfigReseter DataConfigReseter { get; set; }

        /// <summary>
        /// 获取或设置 日志配置重置信息
        /// </summary>
        public static ILoggingConfigReseter LoggingConfigReseter { get; set; }

        /// <summary>
        /// 获取或设置 数据配置信息
        /// </summary>
        public DataConfig DataConfig { get; set; }

        /// <summary>
        /// 获取或设置 日志配置信息
        /// </summary>
        public LoggingConfig LoggingConfig { get; set; }
    }
}
