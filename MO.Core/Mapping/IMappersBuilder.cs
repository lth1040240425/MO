

using System.Collections.Generic;

using MO.Core.Dependency;


namespace MO.Core.Mapping
{
    /// <summary>
    /// 定义对象映射构造器
    /// </summary>
    public interface IMappersBuilder : ISingletonDependency
    {
        /// <summary>
        /// 执行对象映射构造
        /// </summary>
        /// <param name="mapTuples">对象映射源-目标查找器配对信息集合</param>
        void Build(IEnumerable<IMapTuple> mapTuples);
    }
}