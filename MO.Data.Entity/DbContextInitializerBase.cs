﻿using MO.Core.Data;
using MO.Data.Entity.Defaults;
using MO.Data.Entity.Migrations;
using MO.Data.Entity.Properties;
using MO.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MO.Data.Entity
{
    /// <summary>
    /// 数据上下文初始化基类
    /// </summary>
    public abstract class DbContextInitializerBase<TDbContext> : DbContextInitializerBase
        where TDbContext:DbContext,IUnitOfWork,new()
    {
        protected DbContextInitializerBase()
        {
            CreateDatabaseInitializer = new CreateDatabaseIfNotExists<TDbContext>();
            MigrateInitializer = new MigrateDatabaseToLatestVersion<TDbContext, AutoMigrationsConfiguration<TDbContext>>();
        }
        /// <summary>
        /// 获取或设置 设置数据库创建初始化，默认为<see cref="CreateDatabaseIfNotExists{TDbContext}"/>
        /// </summary>
        public IDatabaseInitializer<TDbContext> CreateDatabaseInitializer { get; set; }

        /// <summary>
        /// 获取或设置 数据迁移策略，默认为自动迁移
        /// </summary>
        public IDatabaseInitializer<TDbContext> MigrateInitializer { get; set; }

        /// <summary>
        /// 重写以筛选出当前上下文的实体映射信息
        /// </summary>
        protected override IEnumerable<IEntityMapper> EntityMappersFilter(IEnumerable<IEntityMapper> entityMappers)
        {
            Type contextType = typeof(TDbContext);
            Expression<Func<IEntityMapper, bool>> predicate = m => m.DbContextType == contextType;
            if (contextType == typeof(DefaultDbContext))
            {
                predicate = predicate.Or(m => m.DbContextType == null);
            }
            return entityMappers.Where(predicate.Compile());
        }

        /// <summary>
        /// 数据库初始化实现，设置数据库初始化策略，并进行EntityFramework的预热
        /// </summary>
        protected override void ContextInitialize()
        {
            TDbContext context = new TDbContext();
            IDatabaseInitializer<TDbContext> initializer;
            if (!context.Database.Exists())
            {
                initializer = CreateDatabaseInitializer;
            }
            else
            {
                initializer = MigrateInitializer;
            }
            Database.SetInitializer(initializer);

            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            StorageMappingItemCollection mappingItemCollection = (StorageMappingItemCollection)objectContext.ObjectStateManager
                .MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            mappingItemCollection.GenerateViews(new List<EdmSchemaError>());
            context.Dispose();
        }
    }

    /// <summary>
    /// 数据上下文初始化基类
    /// </summary>
    public abstract class DbContextInitializerBase
    {
        public DbContextInitializerBase()
        {
            MapperAssemblies = new List<Assembly>();
            EntityMappers = new ReadOnlyDictionary<Type, IEntityMapper>(new Dictionary<Type, IEntityMapper>());
        }
        /// <summary>
        /// 获取或设置 实体映射程序集
        /// </summary>
        public ICollection<Assembly> MapperAssemblies { get; private set; }

        /// <summary>
        /// 获取 当前上下文的实体映射信息集合
        /// </summary>
        public IReadOnlyDictionary<Type, IEntityMapper> EntityMappers { get; private set; }

        /// <summary>
        /// 执行数据上下文初始化
        /// </summary>
        public void Initialize()
        {
            EntityMappersInitialize();
            ContextInitialize();
        }

        /// <summary>
        /// 初始化实体映射类型
        /// </summary>
        private void EntityMappersInitialize()
        {
            if (MapperAssemblies.Count == 0)
            {
                throw new InvalidOperationException(Resources.DbContextInitializerBase_MapperAssembliesIsEmpty.FormatWith(this.GetType().FullName));
            }
            Type baseType = typeof(IEntityMapper);
            Type[] mapperTypes = MapperAssemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && type != baseType && !type.IsAbstract).ToArray();
            IEnumerable<IEntityMapper> entityMappers = mapperTypes.Select(type => Activator.CreateInstance(type) as IEntityMapper).ToList();
            entityMappers = EntityMappersFilter(entityMappers);
            IDictionary<Type, IEntityMapper> dict = new Dictionary<Type, IEntityMapper>();
            foreach (IEntityMapper mapper in entityMappers)
            {
                Type baseMapperType = mapper.GetType().BaseType;
                if (baseMapperType == null)
                {
                    continue;
                }
                Type entityType = baseMapperType.GetGenericArguments().FirstOrDefault();
                if (entityType == null || dict.ContainsKey(entityType))
                {
                    continue;
                }
                dict[entityType] = mapper;
            }
            EntityMappers = new ReadOnlyDictionary<Type, IEntityMapper>(dict);
        }

        /// <summary>
        /// 重写以筛选出当前上下文的实体映射信息
        /// </summary>
        protected abstract IEnumerable<IEntityMapper> EntityMappersFilter(IEnumerable<IEntityMapper> entityMappers);

        /// <summary>
        /// 数据库初始化实现，设置数据库初始化策略，并进行EntityFramework的预热
        /// </summary>
        protected abstract void ContextInitialize();
    }
}
