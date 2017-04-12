using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Common
{
    /// <summary>
    /// 节流器
    /// </summary>
    public class Throttler
    {
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="key">Cache key for throttler. Include the resource name, e.g. username, you are throttling</param>
        /// <param name="duration">Check period</param>
        /// <param name="limit">How many times are allowed</param>
        public Throttler(string key, TimeSpan duration, int limit)
        {
            Key = key;
            Duration = duration;
            Limit = limit;
            CacheKey = "Throttling:" + key + ":" + duration.Ticks;
        }

        /// <summary>
        /// 节流器key
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// Duration
        /// </summary>
        public TimeSpan Duration { get; private set; }
        /// <summary>
        /// 限制次数
        /// </summary>
        public int Limit { get; private set; }
        /// <summary>
        /// 缓存key
        /// </summary>
        public string CacheKey { get; private set; }

        private class HitInfo
        {
            public int Counter;
        }

        /// <summary>
        ///没超过限制的次数返回true,否则返回fals
        /// </summary>
        /// <returns>没超过限制的次数返回true,否则返回fals</returns>
        public bool Check()
        {
            var hit = LocalCache.TryGet<HitInfo>(this.CacheKey);
            if (hit == null)
            {
                hit = new HitInfo { Counter = 1 };
                LocalCache.Add(this.CacheKey, hit, this.Duration);
            }
            else
            {
                if (hit.Counter++ >= this.Limit)
                    return false;
            }

            return true;
        }

        /// <summary>
        ///重置节流器
        /// </summary>
        public void Reset()
        {
            LocalCache.Remove(CacheKey);
        }
    }
}
