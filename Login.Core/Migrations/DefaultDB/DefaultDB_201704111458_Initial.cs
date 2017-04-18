using System;
using FluentMigrator;

namespace Login.Core.Migrations.DefaultDB
{
    [Migration(201704111458)]
    public class DefaultDB_201704111458_Initial : Migration
    {
        public override void Up()
        {
            this.CreateTableWithId32("Users", "UserId", s => s
                 .WithColumn("Username").AsString(100).NotNullable()
                 .WithColumn("DisplayName").AsString(100).NotNullable()
                 .WithColumn("Email").AsString(100).Nullable()
                 .WithColumn("Source").AsString(4).NotNullable()
                 .WithColumn("PasswordHash").AsString(86).NotNullable()
                 .WithColumn("PasswordSalt").AsString(10).NotNullable()
                 .WithColumn("LastDirectoryUpdate").AsDateTime().Nullable()
                 .WithColumn("UserImage").AsString(100).Nullable()
                 .WithColumn("InsertDate").AsDateTime().NotNullable()
                 .WithColumn("InsertUserId").AsInt32().NotNullable()
                 .WithColumn("UpdateDate").AsDateTime().Nullable()
                 .WithColumn("UpdateUserId").AsInt32().Nullable()
                 .WithColumn("IsActive").AsInt16().NotNullable().WithDefaultValue(1)
                 .WithColumn("OpenIDClaimedIdentifier").AsString(100).Nullable()
                 .WithColumn("OpenIDFriendlyIdentifier").AsString(100).Nullable());

            Insert.IntoTable("Users").Row(new
            {
                Username = "admin",
                DisplayName = "admin",
                Email = "admin@dummy.com",
                Source = "site",
                PasswordHash = "rfqpSPYs0ekFlPyvIRTXsdhE/qrTHFF+kKsAUla7pFkXL4BgLGlTe89GDX5DBysenMDj8AqbIZPybqvusyCjwQ",
                PasswordSalt = "hJf_F",
                InsertDate = new DateTime(2014, 1, 1),
                InsertUserId = 1,
                IsActive = 1
            });

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
                    .ForeignKey("FK_UserId_Users", "Users", "UserId")
                .WithColumn("Scope").AsString(50).WithDefaultValue("")
                .WithColumn("ExpirationDateUtc").AsDateTime());

            this.CreateTableWithId32("Nonce", "Id", s => s
                .WithColumn("Context").AsString(100).NotNullable()
                .WithColumn("Code").AsString(100).NotNullable()
                .WithColumn("Timestamp").AsDateTime().NotNullable());

            this.CreateTableWithId32("SymmetricCryptoKey", "Id", s => s
                .WithColumn("Bucket").AsString(100).NotNullable()
                .WithColumn("Handle").AsString(100).NotNullable()
                .WithColumn("ExpiresUtc").AsDateTime()
                .WithColumn("Secret").AsByte());
        }

        public override void Down()
        {
        }
    }
}
