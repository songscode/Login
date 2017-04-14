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
    public class SymmetricCryptoKeyDB : BaseDB
    {
        private ContextDB db = NewDB();
        private SymmetricCryptoKeyDB()
        {

        }
        public static SymmetricCryptoKeyDB New()
        {
            return new SymmetricCryptoKeyDB();
        }

        public SymmetricCryptoKey Get(string bucket, string handle)
        {
            var sql=new Sql();
            sql.Where("Bucket=@0 and Handle=@1 ", bucket, handle);
            return db.FirstOrDefault<SymmetricCryptoKey>(sql);
        }

        public IEnumerable<SymmetricCryptoKey> Gets(string bucket)
        {
            var sql=new Sql();
            sql.Where("Bucket=@0",bucket)
                .OrderBy("ExpiresUtc Desc");
            return db.Query<SymmetricCryptoKey>(sql);
        }

        public void Deleted(string bucket, string handle)
        {
            var sql = new Sql();
            sql.Where("Bucket=@0 and Handle=@1 ", bucket, handle);
            db.Delete<SymmetricCryptoKey>(sql);
        }
    }
}
