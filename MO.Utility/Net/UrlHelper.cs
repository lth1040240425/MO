using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Utility.Net
{
    /// <summary>
    /// UrlHelper
    /// </summary>
    public static class UrlHelper
    {

        /// <summary>
        /// 获取基地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetBaseUrl(string url)
        {
            Uri uri = new Uri(url);
            string Scheme = uri.Scheme;
            string baseUrl = uri.Authority;
            return Scheme+"://"+baseUrl;
        }

        /// <summary>
        /// 获取具体的调用方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetAbsolutePath(string url)
        {
            Uri uri = new Uri(url);
            string AbsolutePath = uri.AbsolutePath;
            return AbsolutePath;
        }
    }
}
