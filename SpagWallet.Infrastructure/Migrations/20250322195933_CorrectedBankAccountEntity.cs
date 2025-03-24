using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpagWallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedBankAccountEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Wallets");

            migrationBuilder.AddColumn<Guid>(
                name: "WalletNumber",
                table: "Wallets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletNumber",
                table: "Wallets");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
