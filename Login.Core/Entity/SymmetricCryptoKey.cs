using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Common.PetaPoco;
using PetaPoco;

namespace Login.Core.Entity
{
    [Serializable]
    [TableName("SymmetricCryptoKey")]
    [PrimaryKey("Bucket",AutoIncrement = false)]
    public class SymmetricCryptoKey : ContextDB.Record<SymmetricCryptoKey>
    {
        public string Bucket { get; set; }
        public string Handle { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public byte[] Secret { get; set; }
    }
}
