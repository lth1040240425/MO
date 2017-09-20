using MO.Core.Configs;
using MO.Core.Data;
using MO.Core.Dependency;
using MO.Data.Entity.Properties;
using MO.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Data.Entity
{
    /// <summary>
    /// 服务映射集合扩展操作
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加数据层服务映射信息
        /// </summary>
        /// <param name="services">服务映射信息集合</param>
        public static void AddDataServices(this IServiceCollection services)
        {
            //添加上下文类型
            if (MOConfig.DataConfigReseter == null)
            {
                MOConfig.DataConfigReseter = new DataConfigReseter();
            }
            DataConfig config = MOConfig.Instance.DataConfig;
            Type[] contextTypes = config.ContextConfigs.Where(m => m.Enabled).Select(m => m.ContextType).ToArray();
            Type baseType = typeof(IUnitOfWork);
            foreach (var contextType in contextTypes)
            {
                if (!baseType.IsAssignableFrom(contextType))
                {
                    throw new InvalidOperationException(Resources.ContextTypeNotIUnitOfWorkType.FormatWith(contextType));
                }
                services.AddScoped(baseType, contextType);
                services.AddScoped(contextType);
            }

            //其他数据层初始化类型
            services.AddSingleton<IEntityMapperAssemblyFinder, EntityMapperAssemblyFinder>();
        }
    }

}
