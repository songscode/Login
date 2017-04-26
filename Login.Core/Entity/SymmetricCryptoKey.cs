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
    [PrimaryKey("Id",AutoIncrement = false)]
    public class SymmetricCryptoKey : OperationData<SymmetricCryptoKey>
    {
        public int Id { get; set; }
        public string Bucket { get; set; }
        public string Handle { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string Secret { get; set; }
    }
}
