using System;
using FluentMigrator;

namespace Login.Core.Migrations.DefaultDB
{
    [Migration(20141123155100)]
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
                 .WithColumn("IsActive").AsInt16().NotNullable().WithDefaultValue(1));

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
        }

        public override void Down()
        {
        }
    }
}
