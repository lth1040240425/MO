using MO.Core.Data;
using MO.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Data.Entity
{
    /// <summary>
    /// 定义数据上下文实例创建器
    /// </summary>
    public interface IDbContextTypeResolver: ISingletonDependency
    {
        /// <summary>
        /// 由实体类型获取关联的上下文类型
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns></returns>
        IUnitOfWork Resolve<TEntity, TKey>() where TEntity:IEntity<TKey>;
        /// <summary>
        /// 由实体类型获取关联的上下文类型     
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        IUnitOfWork Resolve(Type entityType);
    }
}
