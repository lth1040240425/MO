﻿// -----------------------------------------------------------------------
//  <copyright file="ServiceBuildOptions.cs" company="MO开源团队">
//      Copyright (c) 2014-2015 MO. All rights reserved.
//  </copyright>
//  <site>http://www.MO.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-10-10 15:49</last-date>
// -----------------------------------------------------------------------

using MO.Core.Reflection;


namespace MO.Core.Dependency
{
    /// <summary>
    /// 服务创建配置信息
    /// </summary>
    public class ServiceBuildOptions
    {
        /// <summary>
        /// 初始化一个<see cref="ServiceBuildOptions"/>类型的新实例
        /// </summary>
        public ServiceBuildOptions()
        {
            AssemblyFinder = new DirectoryAssemblyFinder();
            TransientTypeFinder = new TransientDependencyTypeFinder();
            ScopeTypeFinder = new ScopeDependencyTypeFinder();
            SingletonTypeFinder = new SingletonDependencyTypeFinder();
        }

        /// <summary>
        /// 获取或设置 程序集查找器
        /// </summary>
        public IAllAssemblyFinder AssemblyFinder { get; set; }

        /// <summary>
        /// 获取或设置 即时生命周期依赖类型查找器
        /// </summary>
        public ITypeFinder TransientTypeFinder { get; set; }

        /// <summary>
        /// 获取或设置 范围生命周期依赖类型查找器
        /// </summary>
        public ITypeFinder ScopeTypeFinder { get; set; }

        /// <summary>
        /// 获取或设置 单例生命周期依赖类型查找器
        /// </summary>
        public ITypeFinder SingletonTypeFinder { get; set; }
    }
}