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
    [TableName("ClientAuthorization")]
    [PrimaryKey("AuthorizationId")]
    public class ClientAuthorization : OperationData<ClientAuthorization>
    {
        public int AuthorizationId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public string Scope { get; set; }
        public string ExpirationDateUtc { get; set; }
    }
}
