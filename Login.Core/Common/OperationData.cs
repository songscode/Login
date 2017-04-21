using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Login.Common.PetaPoco;
using Login.Core.Data;
using PetaPoco;
using System.Web.Script.Serialization;

namespace Login.Core
{
    public class OperationData<T> where T : new()
    {
        private ContextDB DB()
        {
                var conn =this.GetType().GetCustomAttributes(typeof(ConnectionAttribute), true);
                var databaseKey = conn.Length == 0 ? DBConnections.DefaultKey : (conn[0] as ConnectionAttribute).Value;
                return new ContextDB(databaseKey);
        }

        public bool IsNew() { return DB().IsNew(this); }
        public object Insert() { return DB().Insert(this); }

        public void Save() { DB().Save(this); }
        public int Update() { return DB().Update(this); }

        public int Update(IEnumerable<string> columns) { return DB().Update(this, columns); }
    }
}
