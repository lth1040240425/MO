using Demo.Models;
using MO.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Service.Interface
{
    public interface IUserService: IScopeDependency
    {
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        bool Register(Member member);
    }
}
