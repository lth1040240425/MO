//// -----------------------------------------------------------------------
////  <copyright file="AppBuilderExtensions.cs" company="OSharp开源团队">
////      Copyright (c) 2014-2015 OSharp. All rights reserved.

//using System.Web.Mvc;

//using MO.Core;
//using MO.Core.Dependency;
//using MO.Utility;
//using OA.Web.Mvc.Binders;

//using Owin;


//namespace OA.Web.Mvc.Initialize
//{
//    /// <summary>
//    /// <see cref="IAppBuilder"/>初始化扩展
//    /// </summary>
//    public static class AppBuilderExtensions
//    {
//        /// <summary>
//        /// 初始化Mvc框架
//        /// </summary>
//        public static IAppBuilder UseOsharpMvc(this IAppBuilder app, IIocBuilder iocBuilder)
//        {
//            iocBuilder.CheckNotNull("iocBuilder");

//            ModelBinders.Binders.Add(typeof(string), new StringTrimModelBinder());

//            IFrameworkInitializer initializer = new FrameworkInitializer();
//            initializer.Initialize(iocBuilder);
//            return app;
//        }
//    }
//}