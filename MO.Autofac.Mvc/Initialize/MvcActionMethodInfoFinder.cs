﻿
//using System;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using System.Web.Mvc;

//using MO.Core.Security;
//using MO.Utility.Extensions;
//using MO.Autofac.Mvc.Properties;


//namespace MO.Autofac.Mvc.Initialize
//{
//    /// <summary>
//    /// MVC功能方法查找器
//    /// </summary>
//    public class MvcActionMethodInfoFinder : IFunctionMethodInfoFinder
//    {
//        /// <summary>
//        /// 查找指定条件的功能方法信息
//        /// </summary>
//        /// <param name="type">控制器类型</param>
//        /// <param name="predicate">筛选条件</param>
//        /// <returns></returns>
//        public MethodInfo[] Find(Type type, Func<MethodInfo, bool> predicate)
//        {
//            return FindAll(type).Where(predicate).ToArray();
//        }

//        /// <summary>
//        /// 从指定类型查找功能方法信息
//        /// </summary>
//        /// <param name="type">控制器类型</param>
//        /// <returns></returns>
//        public MethodInfo[] FindAll(Type type)
//        {
//            if (!typeof(Controller).IsAssignableFrom(type))
//            {
//                throw new InvalidOperationException(Resources.ActionMethodInfoFinder_TypeNotMvcControllerType.FormatWith(type.FullName));
//            }
//            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
//                .Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType)
//                    || m.ReturnType.IsGenericType
//                        && m.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
//                        && typeof(ActionResult).IsAssignableFrom(m.ReturnType.GetGenericArguments()[0]))
//                    .ToArray();
//            return methods;
//        }
//    }
//}