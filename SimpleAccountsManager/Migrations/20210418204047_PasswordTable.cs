using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleAccountsManager.Migrations
{
    public partial class PasswordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "StoredPasswords");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "StoredPasswords",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "StoredPasswords");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "StoredPasswords",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
