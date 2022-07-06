using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vesta.Banks.EntityFrameworkCore.SqlServer.Migrations
{
    public partial class Add_BankTransfer_History : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "BankAccounts",
                type: "decimal(16,4)",
                precision: 16,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldPrecision: 16,
                oldScale: 4,
                oldDefaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BankTransferHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankAccountFromNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccountToNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(16,4)", precision: 16, scale: 4, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransferHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransferHistory");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "BankAccounts",
                type: "decimal(16,4)",
                precision: 16,
                scale: 4,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldPrecision: 16,
                oldScale: 4);
        }
    }
}
