using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;

namespace Login.Core.Data
{
    public static class DBConnections
    {
        private static Dictionary<string, Connection> dic = null;

        public static string DefaultKey = "Default";
#if SAMPLESONLY
        public static int Initial()
        {
            string[] databaseKeys = new[] { DefaultKey };
            if (dic == null)
            {
                dic=new Dictionary<string, Connection>();
                foreach (var databaseKey in databaseKeys)
                {
                    var entry = ConfigurationManager.ConnectionStrings[databaseKey];
                    dic.Add(databaseKey,new Connection
                    {
                        ConnectionString = entry.ConnectionString
                        ,ProviderName = entry.ProviderName
                        ,DatabaseKey = databaseKey
                    });
                }
            }
            return dic.Count;
        }
#else
		[Obsolete("DBConnections初始化，这里需要自己定义", true)]
		public static int Initial()
        {
        //todo 定义链接字符串
           throw new NotImplementedException();
        }
#endif
        public static string[] DatabaseKeys
        {
            get
            {
                if (Initial() > 0)
                {
                    return dic.Keys.ToArray();
                }
                return null;
            }
        }

        public static Connection GetConnectionByKey(string key)
        {
            if (Initial() > 0)
            {
                if (dic.ContainsKey(key))
                {
                    return dic[key];
                }
            }
            throw new SqlNullValueException("No Connection");
        }
    }

    public class Connection
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string DatabaseKey { get; set; }
    }
}
