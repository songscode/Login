using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Core.Entity;
using PetaPoco;

namespace Login.Core.Data
{
    public class ClientDB:BaseDB
    {
        private ClientDB()
        {

        }
        public static ClientDB New()
        {
            return new ClientDB();
        }

        public Client GetByClientIdentifier(string clientIdentifier)
        {
            var db = NewDB();
            var sql = new Sql().Where("ClientIdentifier=@0", clientIdentifier);
            return db.SingleOrDefault<Client>(sql);
        }
    }
}
