﻿// -----------------------------------------------------------------------
//  <copyright file="OAResult.cs" company="MO开源团队">
//      Copyright (c) 2014-2015 MO. All rights reserved.
//  </copyright>
//  <site>http://www.MO.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-08-03 18:29</last-date>
// -----------------------------------------------------------------------

namespace MO.Utility.Data
{
    /// <summary>
    /// MO结果基类
    /// </summary>
    /// <typeparam name="TResultType"></typeparam>
    public abstract class OAResult<TResultType> : OAResult<TResultType, object>, IOAResult<TResultType>
    {
        /// <summary>
        /// 初始化一个<see cref="OAResult{TResultType}"/>类型的新实例
        /// </summary>
        protected OAResult()
            : this(default(TResultType))
        { }

        /// <summary>
        /// 初始化一个<see cref="OAResult{TResultType}"/>类型的新实例
        /// </summary>
        protected OAResult(TResultType type)
            : this(type, null, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OAResult{TResultType}"/>类型的新实例
        /// </summary>
        protected OAResult(TResultType type, string message)
            : this(type, message, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OAResult{TResultType}"/>类型的新实例
        /// </summary>
        protected OAResult(TResultType type, string message, object data)
            : base(type, message, data)
        { }
    }


    /// <summary>
    /// MO结果基类
    /// </summary>
    /// <typeparam name="TResultType">结果类型</typeparam>
    /// <typeparam name="TData">结果数据类型</typeparam>
    public abstract class OAResult<TResultType, TData> : IOAResult<TResultType, TData>
    {
        protected string _message;

        /// <summary>
        /// 初始化一个<see cref="OAResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected OAResult()
            : this(default(TResultType))
        { }

        /// <summary>
        /// 初始化一个<see cref="OAResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected OAResult(TResultType type)
            : this(type, null, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OAResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected OAResult(TResultType type, string message)
            : this(type, message, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OAResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected OAResult(TResultType type, string message, TData data)
        {
            ResultType = type;
            _message = message;
            Data = data;
        }

        /// <summary>
        /// 获取或设置 结果类型
        /// </summary>
        public TResultType ResultType { get; set; }

        /// <summary>
        /// 获取或设置 返回消息
        /// </summary>
        public virtual string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 获取或设置 结果数据
        /// </summary>
        public TData Data { get; set; }
    }
}