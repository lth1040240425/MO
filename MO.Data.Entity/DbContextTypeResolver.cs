using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MO.Core.Data;
using MO.Core.Dependency;
using MO.Utility;
using MO.Data.Entity.Properties;
using MO.Utility.Extensions;

namespace MO.Data.Entity
{
    /// <summary>
    /// 数据上下文类型获取器
    /// </summary>
    public class DbContextTypeResolver : IDbContextTypeResolver
    {
        private readonly IIocResolver _resolver;

        /// <summary>
        ///  初始化一个<see cref="DbContextTypeResolver"/>类型的新实例
        /// </summary>
        /// <param name="resolver"></param>
        public DbContextTypeResolver(IIocResolver resolver)
        {
            _resolver = resolver;
        }
        /// <summary>
        /// 由实体类型获取关联的上下文类型
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns></returns>
        public IUnitOfWork Resolve<TEntity, TKey>() where TEntity : IEntity<TKey>
        {
            return Resolve(typeof(TEntity));
        }

        /// <summary>
        ///  /// <summary>
        /// 由实体类型获取关联的上下文类型
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IUnitOfWork Resolve(Type entityType)
        {
            entityType.CheckNotNull("entityType");
            Type contextType = DbContextManager.Instance.GetDbContexType(entityType);
            IUnitOfWork unitOfWork = (IUnitOfWork)_resolver.Resolve(contextType);
            if (unitOfWork == null)
            {
                throw new InvalidOperationException(Resources.DbContextTypeResolver_DbContextResolveFailed.FormatWith(entityType, contextType));
            }
            return unitOfWork;
        }
    }
}
