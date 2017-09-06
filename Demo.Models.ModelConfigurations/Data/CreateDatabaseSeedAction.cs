using MO.Data.Entity.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models.ModelConfigurations.Data
{
    public class CreateDatabaseSeedAction : ISeedAction
    { /// <summary>
      /// 定义种子数据初始化过程
      /// </summary>
      /// <param name="context">数据上下文</param>
        public void Action(DbContext context)
        {
            //context.Set<Users>()
            //    .Add(new Users()
            //    {
            //        UserName = "米客",
            //        Mobile = "15860288200",
            //        AddTime = System.DateTime.Now,
            //        OPENID = "1000",
            //        Tag = "Mike",
            //        UserType = "1",
            //        State = 1
            //    });
        }

        /// <summary>
        /// 获取 操作排序，数值越小越先执行
        /// </summary>
        public int Order
        {
            get { return 1; }
        }
    }
}
