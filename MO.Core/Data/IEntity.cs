using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Core.Data
{
    /// <summary>
    /// 数据模型接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<out TKey>
    {
        /// <summary>
        /// 获取 实体唯一标识，主键
        /// </summary>
        TKey Id { get; }
    }
}
