using System;


namespace MO.Core.Dependency
{
    /// <summary>
    /// 对象创建委托
    /// </summary>
    /// <param name="provider">服务提供者</param>
    /// <param name="args">构造函数的参数</param>
    /// <returns></returns>
    public delegate object ObjectFactory(IServiceProvider provider, object[] args);
}