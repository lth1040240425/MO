using MO.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
    /// <summary>
    /// 会员信息
    /// </summary>
    public class Member : EntityBase<Guid>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNum { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 省份Id
        /// </summary>
        public Guid ProvinceId { get; set; }
        /// <summary>
        /// 城市Id
        /// </summary>
        public Guid CityId { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
    }
}
