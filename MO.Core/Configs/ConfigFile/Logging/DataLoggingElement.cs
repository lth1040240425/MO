

using System.Configuration;

using MO.Utility.Logging;


namespace MO.Core.Configs.ConfigFile
{
    /// <summary>
    /// 数据日志配置节点
    /// </summary>
    internal class DataLoggingElement : ConfigurationElement
    {
        private const string EnabledKey = "enabled";
        private const string OutLogLevelKey = "level";
        private const string TypeKey = "type";

        /// <summary>
        /// 获取或设置 是否允许数据日志输出
        /// </summary>
        [ConfigurationProperty(EnabledKey, DefaultValue = true)]
        public virtual bool Enabled
        {
            get { return (bool)this[EnabledKey]; }
            set { this[EnabledKey] = value; }
        }

        /// <summary>
        /// 获取或设置 数据日志输出级别
        /// </summary>
        [ConfigurationProperty(OutLogLevelKey, DefaultValue = LogLevel.All)]
        public virtual LogLevel OutLogLevel
        {
            get { return (LogLevel)this[OutLogLevelKey]; }
            set { this[OutLogLevelKey] = value; }
        }

        /// <summary>
        /// 获取或设置 数据日志输出适配器类型名称
        /// </summary>
        [ConfigurationProperty(TypeKey, DefaultValue = "OA.Core.Logging.DatabaseLoggerAdapter, OA.Core")]
        public virtual string AdapterTypeName
        {
            get { return (string)this[TypeKey]; }
            set { this[TypeKey] = value; }
        }
    }
}