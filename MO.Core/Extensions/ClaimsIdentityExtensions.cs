using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MO.Core.Extensions
{
    /// <summary>
    /// <see cref="ClaimsIdentity"/>扩展操作类
    /// </summary>
    public static class ClaimsIdentityExtensions
    {
        /// <summary>
        /// 获取指定类型的Claim值
        /// </summary>
        public static string GetClaimValue(this ClaimsIdentity identity, string type)
        {
            Claim claim = identity.Claims.FirstOrDefault(m => m.Type == type);
            return claim == null ? null : claim.Value;
        }
    }
}
