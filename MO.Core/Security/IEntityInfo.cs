using MO.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Core.Security
{
    /// <summary>
    /// 实体数据接口
    /// </summary>
    public interface IEntityInfo : IRecyclable
    {
        /// <summary>
        /// 获取 实体数据类型名称
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// 获取 实体数据显示名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取 是否启用数据日志
        /// </summary>
        bool DataLogEnabled { get; }

        /// <summary>
        /// 获取 实体属性信息字典
        /// </summary>
        IDictionary<string, string> PropertyNames { get; }
    }
}
