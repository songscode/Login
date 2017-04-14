using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Login.Core.Migrations.DefaultDB
{
    [Migration(201704131609)]
    public class DefaultDB_201704131609_Clients : Migration
    {
        public override void Up()
        {
            this.CreateTableWithId32("Clients", "ClientId", s => s
                .WithColumn("ClientIdentifier").AsString(100).NotNullable()
                .WithColumn("ClientSecret").AsString(100).NotNullable()
                .WithColumn("Callback").AsString(100).Nullable()
                .WithColumn("Name").AsString(4).NotNullable()
                .WithColumn("ClientType").AsString(86).NotNullable());

            this.CreateTableWithId32("ClientAuthorization", "AuthorizationId", s => s
                .WithColumn("CreatedOnUtc").AsDateTime().NotNullable()
                .WithColumn("ClientId").AsInt32().NotNullable()
                    .ForeignKey("FK_ClientId_Clients", "Clients", "ClientId")
                .WithColumn("UserId").AsInt32().NotNullable()
                    .ForeignKey("FK_UserId_Users", "Users","UserId")
                .WithColumn("Scope").AsString(50).WithDefaultValue("")
                .WithColumn("ExpirationDateUtc").AsDateTime());

            this.Create.Table("Nonce")
                .WithColumn("Context").AsString(100).PrimaryKey()
                .WithColumn("Code").AsString(100).PrimaryKey()
                .WithColumn("Timestamp").AsDateTime().PrimaryKey();

            this.Create.Table("SymmetricCryptoKey")
                .WithColumn("Bucket").AsString(100).PrimaryKey()
                .WithColumn("Handle").AsString(100).PrimaryKey()
                .WithColumn("ExpiresUtc").AsDateTime()
                .WithColumn("Secret").AsByte();

            this.Alter.Table("Users")
                .AddColumn("OpenIDClaimedIdentifier").AsString(100).Nullable()
                .AddColumn("OpenIDFriendlyIdentifier").AsString(100).Nullable();

        }

        public override void Down()
        {
        }
    }
}
