using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Utility.ConfigInfo
{
    public class ConfigInfoHelper
    {
        #region 获取Config内容

        /// <summary>
        /// 获取ConnectionString的配置
        /// </summary>
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// 获取AppSettings的配置
        /// </summary>
        public static string GetAppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        #endregion 获取Config内容
    }
}
