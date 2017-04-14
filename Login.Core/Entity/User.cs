using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Common.PetaPoco;
using PetaPoco;

namespace Login.Core { 
    [Serializable]
    [TableName("Users")]
    [PrimaryKey("id")]
    public partial class User : ContextDB.Record<User>
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Source { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime? LastDirectoryUpdate { get; set; }
        public string UserImage { get; set; }
        public DateTime InsertDate { get; set; }
        public int InsertUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int UpdateUserId { get; set; }
        public int IsActive { get; set; }

        public string OpenIDClaimedIdentifier { get; set; }

        public string OpenIDFriendlyIdentifier { get; set; }
    }
}
