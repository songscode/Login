using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Login.Core.Migrations.DefaultDB
{
    [Migration(201704251458)]
    public class DefaultDB_201704251458_Initial : Migration
    {
        public override void Up()
        {
            this.Update.Table("Clients")
                .Set(new
            {
                ClientType=1
            }).Where(new { ClientId=1 });
            this.Alter.Table("Clients").AlterColumn("ClientType")
                .AsInt32();
            this.Alter.Table("SymmetricCryptoKey").AlterColumn("Secret")
                .AsString(200);
        }

        public override void Down()
        {
        }
    }
}
