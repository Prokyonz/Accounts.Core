using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class AddedPinColumnInUserMasterTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Pin",
                table: "UserMaster",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "SaleReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "PurchaseDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "CustomerReport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SaleReportForAdmin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<long>(type: "bigint", nullable: false),
                    PanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarratQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SGST = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CGST = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BillAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Payment1_Mode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment1_CardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment1_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Payment2_Mode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment2_CardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment2_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Payment3_Mode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment3_CardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment3_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Payment4_Mode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment4_CardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment4_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleReportForAdmin");

            migrationBuilder.DropColumn(
                name: "Pin",
                table: "UserMaster");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "SaleReport");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "CustomerReport");

            migrationBuilder.AlterColumn<long>(
                name: "Quantity",
                table: "PurchaseDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
