using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Common.Caching;

namespace Login.Common
{
    public static class LocalCache
    {
        /// <summary>
        /// DIContainer.Resolve实例化，IOC原理
        /// </summary>
        public static ICache StaticProvider;

        public static ICache Provider
        {
            get
            {
                return StaticProvider ?? DIContainer.Resolve<ICache>();
            }
        }

        /// <summary>
        /// 缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">Expire time (Use TimeSpan.Zero to hold value with no expiration)</param>
        public static void Add(string key, object value, TimeSpan expiration)
        {
            Provider.Add(key, value, expiration);
        }

        /// <summary>
        ///从本地缓存读取具有指定键的值。如果它不存在于缓存中，调用加载器loader;
        /// loader读取数据库值，并存入缓存中
        /// </summary>
        /// <typeparam name="TItem">Data type</typeparam>
        /// <param name="cacheKey">Key</param>
        /// <param name="expiration">Expiration (TimeSpan.Zero means no expiration)</param>
        /// <param name="loader">Loader function that will be called if item doesn't exist in the cache.</param>
        public static TItem Get<TItem>(string cacheKey, TimeSpan expiration, Func<TItem> loader)
            where TItem : class
        {
            var cachedObj = Provider.Get(cacheKey);

            if (cachedObj == DBNull.Value)
            {
                return null;
            }

            if (cachedObj == null)
            {
                var item = loader();
                Add(cacheKey, (object)item ?? DBNull.Value, expiration);
                return item;
            }

            return (TItem)cachedObj;
        }

        /// <summary>
        /// 指定类型获取缓存，没有则返回null
        /// </summary>
        /// <typeparam name="TItem">Expected type</typeparam>
        /// <param name="cacheKey">Key</param>
        public static TItem TryGet<TItem>(string cacheKey)
            where TItem : class
        {
            return Provider.Get(cacheKey) as TItem;
        }

        /// <summary>
        /// 删除指定的缓存
        /// </summary>
        /// <param name="cacheKey">Key</param>
        public static void Remove(string cacheKey)
        {
             Provider.Remove(cacheKey);
        }

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public static void RemoveAll()
        {
            Provider.Clear();
        }
    }
}
