using MO.Utility.Net;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Utility.HttpHelp
{
    /// <summary>
    /// Http请求帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 通过RestSharp 发起http请求
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <param name="methodName">方法名</param>
        /// <param name="paras">参数</param>
        /// <param name="mes">返回信息</param>
        /// <param name="me">请求方式  默认post</param>
        /// <returns>返回</returns>
        public static bool PostJsonData(string baseUrl, string methodName, Dictionary<string, object> paras, out string mes, Method me = Method.POST)
        {
            bool boolResult = false;
            try
            {
                var client = new RestClient(baseUrl);
                var request = new RestRequest(methodName, me);
                request.RequestFormat = DataFormat.Json;
                foreach (var item in paras)
                {
                    request.AddParameter(item.Key,item.Value);
                }
                //request.AddBody(paras);
                IRestResponse response = client.Execute(request);
                mes = response.Content;
                boolResult = true;
            }
            catch (Exception ex)
            {
                boolResult = false;
                mes = ex.Message + "," + baseUrl;
            }
            return boolResult;

        }

        public static bool PostJsonData(string fullUrl, Dictionary<string, object> paras, out string mes, Method me = Method.POST)
        {
            string baseUrl=UrlHelper.GetBaseUrl(fullUrl);
            string methodName=UrlHelper.GetAbsolutePath(fullUrl);
            return PostJsonData(baseUrl,methodName,paras,out mes,me);
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <param name="methodName">方法名</param>
        /// <param name="paras">参数</param>
        /// <param name="me">请求方式  默认post</param>
        /// <returns></returns>
        public static byte[] DownStream(string baseUrl, string methodName, Dictionary<string, object> paras, Method me = Method.POST)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(methodName, me);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(paras);
            var bytes = client.DownloadData(request);
            return bytes;

        }
    }
}
