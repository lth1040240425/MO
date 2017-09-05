using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Core.Configs
{
    /// <summary>
    /// OA数据配置信息重置类
    /// </summary>
    public interface IDataConfigReseter
    {
        /// <summary>
        /// 重置数据配置信息
        /// </summary>
        /// <param name="config">原始数据配置信息</param>
        /// <returns>重置后的数据配置信息</returns>
        DataConfig Reset(DataConfig config);
    }
}
