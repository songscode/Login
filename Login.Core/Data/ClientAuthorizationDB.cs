using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Common.PetaPoco;
using Login.Core.Entity;
using PetaPoco;

namespace Login.Core.Data
{
    public class ClientAuthorizationDB:BaseDB
    {
        private static ContextDB db = NewDB();
        private ClientAuthorizationDB()
        {

        }
        public static ClientAuthorizationDB New()
        {
            return new ClientAuthorizationDB();
        }

        public IEnumerable<ClientAuthorization> Gets(string clientIdentifier, DateTime issuedUtc, string username)
        {
            var sql=new Sql();
            sql.Select("ca.*")
                .From("ClientAuthorization ca")
                .InnerJoin("Clients c").On("ca.ClientId=c.ClientId")
                .InnerJoin("Users u").On("ca.UserId=u.UserId")
                .Where("ca.CreatedOnUtc<=@0 and ca.ExpirationDateUtc>=@1 and c.ClientIdentifier=@2 and u.Username=@3", issuedUtc, DateTime.UtcNow, clientIdentifier, username);

            return db.Query<ClientAuthorization>(sql);
        }
    }
}
