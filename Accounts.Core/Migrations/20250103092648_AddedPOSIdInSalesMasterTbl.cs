using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class AddedPOSIdInSalesMasterTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleSlipNo",
                table: "SaleReport");

            migrationBuilder.DropColumn(
                name: "TotalItems",
                table: "SaleReport");

            migrationBuilder.AddColumn<long>(
                name: "POSId",
                table: "SalesMasters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "CardNo",
                table: "SaleReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNo",
                table: "SaleReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "SaleReport",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMode",
                table: "SaleReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentNo",
                table: "SaleReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TIDNumber",
                table: "POSMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TIDBankName",
                table: "POSMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "POSResponceModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TIDBankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POSResponceModel");

            migrationBuilder.DropColumn(
                name: "POSId",
                table: "SalesMasters");

            migrationBuilder.DropColumn(
                name: "CardNo",
                table: "SaleReport");

            migrationBuilder.DropColumn(
                name: "InvoiceNo",
                table: "SaleReport");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "SaleReport");

            migrationBuilder.DropColumn(
                name: "PaymentMode",
                table: "SaleReport");

            migrationBuilder.DropColumn(
                name: "PaymentNo",
                table: "SaleReport");

            migrationBuilder.AddColumn<long>(
                name: "SaleSlipNo",
                table: "SaleReport",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalItems",
                table: "SaleReport",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "TIDNumber",
                table: "POSMaster",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TIDBankName",
                table: "POSMaster",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
