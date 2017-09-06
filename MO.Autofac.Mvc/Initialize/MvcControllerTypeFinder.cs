
//using System;
//using System.Linq;
//using System.Reflection;
//using System.Web.Mvc;

//using OA.Core.Reflection;
//using OA.Core.Security;


//namespace OA.Web.Mvc.Initialize
//{
//    /// <summary>
//    /// MVC控制器类型查找器
//    /// </summary>
//    public class MvcControllerTypeFinder : IFunctionTypeFinder
//    {
//        /// <summary>
//        /// 获取或设置 程序集查找器
//        /// </summary>
//        public IAllAssemblyFinder AssemblyFinder { get; set; }

//        /// <summary>
//        /// 查找指定条件的项
//        /// </summary>
//        /// <param name="predicate">筛选条件</param>
//        /// <returns></returns>
//        public Type[] Find(Func<Type, bool> predicate)
//        {
//            return FindAll().Where(predicate).ToArray();
//        }

//        /// <summary>
//        /// 查找所有项
//        /// </summary>
//        /// <returns></returns>
//        public Type[] FindAll()
//        {
//            Assembly[] assemblies = AssemblyFinder.FindAll();
//            return assemblies.SelectMany(assembly => assembly.GetTypes()
//                .Where(type => typeof(Controller).IsAssignableFrom(type) && !type.IsAbstract))
//                .Distinct().ToArray();
//        }
//    }
//}