using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Samples.Banks.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Add_BankAccount_Active : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                table: "BankAccounts",
                newName: "Version");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BankAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BankAccounts");

            migrationBuilder.RenameColumn(
                name: "Version",
                table: "BankAccounts",
                newName: "ConcurrencyStamp");
        }
    }
}
