
using System.Collections.Generic;


namespace MO.Core.Mapping
{
    /// <summary>
    /// 对象映射构造器
    /// </summary>
    public class MappersBuilder : IMappersBuilder
    {
        /// <summary>
        /// 执行对象映射构造
        /// </summary>
        /// <param name="mapTuples">对象映射源-目标查找器配对信息集合</param>
        public void Build(IEnumerable<IMapTuple> mapTuples)
        {
            foreach (IMapTuple mapTuple in mapTuples)
            {
                mapTuple.Build();
            }
        }
    }
}