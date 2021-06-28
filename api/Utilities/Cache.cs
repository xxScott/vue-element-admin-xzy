using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;

namespace Utilities
{
    public class Cache
    {
        const int cacheExpireMinute = 300000;

        /// <summary>
        /// 添加至缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void AddToCache(string key, object value)
        {
            HttpContext.Current.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(cacheExpireMinute), CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void AddToCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 根据“键”从缓存中读取“值”
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static object GetFromCache(string key)
        {
            return HttpContext.Current.Cache.Get(key);
        }

        /// <summary>
        /// 根据“键”从缓存中删除
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveCache(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }
    }
}
