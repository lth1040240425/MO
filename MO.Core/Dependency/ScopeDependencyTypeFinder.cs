﻿using System;
using System.Linq;
using System.Reflection;

using MO.Core.Reflection;
using MO.Utility.Extensions;


namespace MO.Core.Dependency
{
    /// <summary>
    /// <see cref="IScopeDependency"/>接口实现类查找器
    /// </summary>
    public class ScopeDependencyTypeFinder : ITypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="ScopeDependencyTypeFinder"/>类型的新实例
        /// </summary>
        public ScopeDependencyTypeFinder()
        {
            AssemblyFinder = new DirectoryAssemblyFinder();
        }

        /// <summary>
        /// 获取或设置 程序集查找器
        /// </summary>
        public IAllAssemblyFinder AssemblyFinder { get; set; }

        /// <summary>
        /// 查找指定条件的项
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns></returns>
        public Type[] Find(Func<Type, bool> predicate)
        {
            return FindAll().Where(predicate).ToArray();
        }

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <returns></returns>
        public Type[] FindAll()
        {
            try
            {
                Assembly[] assemblies = AssemblyFinder.FindAll();
                return assemblies.SelectMany(assembly =>
                    assembly.GetTypes().Where(type =>
                        typeof(IScopeDependency).IsAssignableFrom(type) && !type.IsAbstract))
                    .Distinct().ToArray();
            }
            catch (ReflectionTypeLoadException e)
            {
                string msg = e.Message;
                Exception[] exs = e.LoaderExceptions;
                msg = msg + "\r\n详情：" + exs.Select(m => m.Message).ExpandAndToString("---");
                throw new Exception(msg, e);
            }
        }
    }
}