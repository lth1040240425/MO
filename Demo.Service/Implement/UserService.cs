using Demo.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Models;
using MO.Core.Data;

namespace Demo.Service.Implement
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : IUserService
    {
        public IRepository<Member, Guid> _memberRepository { protected get; set; }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool Register(Member member)
        {
            return _memberRepository.Insert(member) > 0;
        }
    }
}
