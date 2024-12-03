using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class AddNewcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardAmount",
                table: "AmountReceived");

            migrationBuilder.RenameColumn(
                name: "GstAmount",
                table: "SalesDetails",
                newName: "SGST");

            migrationBuilder.RenameColumn(
                name: "CashAmount",
                table: "AmountReceived",
                newName: "Amount");

            migrationBuilder.AddColumn<decimal>(
                name: "CGST",
                table: "SalesDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IGST",
                table: "SalesDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMode",
                table: "AmountReceived",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CGST",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "IGST",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "PaymentMode",
                table: "AmountReceived");

            migrationBuilder.RenameColumn(
                name: "SGST",
                table: "SalesDetails",
                newName: "GstAmount");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "AmountReceived",
                newName: "CashAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "CardAmount",
                table: "AmountReceived",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
