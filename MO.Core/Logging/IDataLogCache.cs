using MO.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Core.Logging
{
    /// <summary>
    /// 数据日志缓存接口
    /// </summary>
    public interface IDataLogCache : IScopeDependency
    {
        /// <summary>
        /// 获取 数据日志集合
        /// </summary>
        IEnumerable<DataLog> DataLogs { get; }

        /// <summary>
        /// 向缓存中添加数据日志信息
        /// </summary>
        /// <param name="dataLog">数据日志信息</param>
        void AddDataLog(DataLog dataLog);
    }
}
