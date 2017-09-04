using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MO.Core.Data;
using MO.Core.Dependency;
using MO.Utility;

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
        public IUnitOfWork Resolve<TEntity, TKey>() where TEntity : IEntity<TKey>
        {
            throw new NotImplementedException();
        }

        public IUnitOfWork Resolve(Type entityType)
        {
            entityType.CheckNotNull("entityType");
            Type contextType= DbContextManager
        }
    }
}
