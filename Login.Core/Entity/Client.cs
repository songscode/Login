using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Common.PetaPoco;
using PetaPoco;

namespace Login.Core
{
    [Serializable]
    [TableName("Clients")]
    [PrimaryKey("ClientId")]
    public partial class Client: OperationData<Client>
    {
        public int ClientId { get; set; }
        public string ClientIdentifier { get; set; }
        public string ClientSecret { get; set; }
        public string Callback { get; set; }
        public string Name { get; set; }
        public int ClientType { get; set; }
    }
}
