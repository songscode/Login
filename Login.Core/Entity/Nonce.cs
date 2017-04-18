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
    [TableName("Nonce")]
    [PrimaryKey("Id")]
    public class Nonce : OperationData<Nonce>
    {
        public int Id { get; set; }
        public string Context { get; set; }
        public string Code { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
