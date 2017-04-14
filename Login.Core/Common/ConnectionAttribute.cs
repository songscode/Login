using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConnectionAttribute : Attribute
    {
      
        public string Value { get; private set; }

        /// <summary>
        ///     数据库链接
        /// </summary>
        /// <param name="databaseKey">数据库链接的主键值</param>
        public ConnectionAttribute(string databaseKey)
        {
            Value = databaseKey;
        }
    }
}
