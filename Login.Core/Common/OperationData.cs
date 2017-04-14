using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Common.PetaPoco;
using Login.Core.Data;
using PetaPoco;

namespace Login.Core
{
    public class OperationData<T> where T : new()
    {
        public ContextDB repo
        {
            get
            {
                var conn =this.GetType().GetCustomAttributes(typeof(ConnectionAttribute), true);
                var databaseKey = conn.Length == 0 ? DBConnections.DefaultKey : (conn[0] as ConnectionAttribute).Value;
                return new ContextDB(databaseKey);
            }
        }

        public bool IsNew() { return repo.IsNew(this); }
        public object Insert() { return repo.Insert(this); }

        public void Save() { repo.Save(this); }
        public int Update() { return repo.Update(this); }

        public int Update(IEnumerable<string> columns) { return repo.Update(this, columns); }
    }
}
