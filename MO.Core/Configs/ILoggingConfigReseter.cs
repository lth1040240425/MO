using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Core.Configs
{
    /// <summary>
    /// 定义日志配置信息重置功能
    /// </summary>
    public interface ILoggingConfigReseter
    {
        /// <summary>
        /// 日志配置信息重置
        /// </summary>
        /// <param name="config">待重置的日志配置信息</param>
        /// <returns>重置后的日志配置信息</returns>
        LoggingConfig Reset(LoggingConfig config);
    }
}
