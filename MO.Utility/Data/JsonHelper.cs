
using System;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MO.Utility.Data
{
    /// <summary>
    /// JSON辅助操作类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 处理Json的时间格式为正常格式
        /// </summary>
        public static string JsonDateTimeFormat(string json)
        {
            json.CheckNotNullOrEmpty("json");
            json = Regex.Replace(json,
                @"\\/Date\((\d+)\)\\/",
                match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                });
            return json;
        }

        /// <summary>
        /// 把对象序列化成Json字符串格式
        /// </summary>
        /// <param name="object">Json 对象</param>
        /// <returns>Json 字符串</returns>
        public static string ToJson(object @object)
        {
            @object.CheckNotNull("@object");
            string json = JsonConvert.SerializeObject(@object);
            return JsonDateTimeFormat(json);
        }

        /// <summary>
        /// 把Json字符串转换为强类型对象
        /// </summary>
        public static T FromJson<T>(string json)
        {
            json.CheckNotNullOrEmpty("json");
            json = JsonDateTimeFormat(json);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 获取json序列化字符串
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public static string GetJsonString(object Ob)
        {
            string Jstring = Newtonsoft.Json.JsonConvert.SerializeObject(Ob);
            return Jstring;
        }


        /// <summary>
        /// 获取json反序列化对象(T类型参数)
        /// </summary>
        /// <param name="JsonStr"></param>
        /// <returns></returns>
        public static T GetJsonObject<T>(string JsonStr)
        {
            object ob = Newtonsoft.Json.JsonConvert.DeserializeObject(JsonStr, typeof(T));
            if (ob != null)
            {
                T jobject = (T)ob;
                return jobject;
            }
            return default(T);
        }
        /// <summary>
        /// 获取json反序列化对象
        /// </summary>
        /// <param name="JsonStr"></param>
        /// <returns></returns>
        public static JObject GetJsonObject(string JsonStr)
        {
            Newtonsoft.Json.Linq.JObject jobject = Newtonsoft.Json.JsonConvert.DeserializeObject(JsonStr) as Newtonsoft.Json.Linq.JObject;
            return jobject;
        }

        /// <summary>
        /// 返回一个json数组
        /// </summary>
        /// <param name="jobject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static JArray GetArray(this Newtonsoft.Json.Linq.JObject jobject, string key)
        {
            Newtonsoft.Json.Linq.JArray jv = jobject[key] as Newtonsoft.Json.Linq.JArray;
            if (jv != null && jv.Count > 0)
            {
                return jv;
            }
            else
            {
                return null;
            }
        }
        public static object GetObject(this Newtonsoft.Json.Linq.JObject jobject, string key)
        {
            return jobject[key];
            //Newtonsoft.Json.Linq.JArray jv = jobject[key] as Newtonsoft.Json.Linq.JArray;
            //if (jv != null && jv.Count > 0)
            //{
            //    return jv;
            //}
            //else
            //{
            //    return null;
            //}
        }

        /// <summary>
        /// 获取Decimal不存在放回null
        /// </summary>
        /// <param name="jobject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Nullable<Decimal> GetDecimal(this Newtonsoft.Json.Linq.JObject jobject, string key)
        {
            Newtonsoft.Json.Linq.JValue jv = jobject[key] as Newtonsoft.Json.Linq.JValue;
            if (jv != null && jv.Value != null)
            {
                return Convert.ToDecimal(jobject[key]);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取String不存在放回null
        /// </summary>
        /// <param name="jobject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(this Newtonsoft.Json.Linq.JObject jobject, string key)
        {
            Newtonsoft.Json.Linq.JValue jv = jobject[key] as Newtonsoft.Json.Linq.JValue;
            if (jv != null && jv.Value != null)
            {
                return Convert.ToString(jobject[key]);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取Guid不存在放回null
        /// </summary>
        /// <param name="jobject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Nullable<Guid> GetGuid(this Newtonsoft.Json.Linq.JObject jobject, string key)
        {
            Newtonsoft.Json.Linq.JValue jv = jobject[key] as Newtonsoft.Json.Linq.JValue;
            if (jv != null && jv.Value != null)
            {
                return Guid.Parse(Convert.ToString(jobject[key]));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取Bool不存在放回null
        /// </summary>
        /// <param name="jobject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Nullable<Boolean> GetBool(this Newtonsoft.Json.Linq.JObject jobject, string key)
        {
            Newtonsoft.Json.Linq.JValue jv = jobject[key] as Newtonsoft.Json.Linq.JValue;
            if (jv != null && jv.Value != null)
            {
                return jobject.Value<bool>(key);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取DateTime不存在放回null
        /// </summary>
        /// <param name="jobject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Nullable<DateTime> GetDateTime(this Newtonsoft.Json.Linq.JObject jobject, string key)
        {
            Newtonsoft.Json.Linq.JValue jv = jobject[key] as Newtonsoft.Json.Linq.JValue;
            if (jv != null && jv.Value != null)
            {
                return jobject.Value<DateTime>(key);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取Int不存在放回null
        /// </summary>
        /// <param name="jobject"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Nullable<Int32> GetInt32(this Newtonsoft.Json.Linq.JObject jobject, string key)
        {
            Newtonsoft.Json.Linq.JValue jv = jobject[key] as Newtonsoft.Json.Linq.JValue;
            if (jv != null && jv.Value != null)
            {
                return jobject.Value<Int32>(key);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 强制转换为guid(null返回empty)
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static Guid ToGuid(this Nullable<Guid> gid)
        {
            try
            {
                Guid ReGuid = Guid.Parse(Convert.ToString(gid));
                return ReGuid;
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }
}