using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class POSMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserPermissionChild");

            migrationBuilder.DropColumn(
                name: "PermissionMasterId",
                table: "UserPermissionChild");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserPermissionChild");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "PurchaseReports");

            migrationBuilder.DropColumn(
                name: "PartyName",
                table: "PurchaseReports");

            migrationBuilder.RenameColumn(
                name: "PurchaseSlipNo",
                table: "PurchaseMaster",
                newName: "InvoiceNo");

            migrationBuilder.AddColumn<long>(
                name: "POSId",
                table: "UserMaster",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNo",
                table: "SalesMasters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DealerName",
                table: "PurchaseReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "PurchaseMaster",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SaleReport",
                columns: table => new
                {
                    SaleSlipNo = table.Column<long>(type: "bigint", nullable: false),
                    PartyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalItems = table.Column<long>(type: "bigint", nullable: false),
                    BillAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleReport");

            migrationBuilder.DropTable(
                name: "StockReport");

            migrationBuilder.DropColumn(
                name: "POSId",
                table: "UserMaster");

            migrationBuilder.DropColumn(
                name: "InvoiceNo",
                table: "SalesMasters");

            migrationBuilder.DropColumn(
                name: "DealerName",
                table: "PurchaseReports");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "PurchaseMaster");

            migrationBuilder.RenameColumn(
                name: "InvoiceNo",
                table: "PurchaseMaster",
                newName: "PurchaseSlipNo");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "UserPermissionChild",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PermissionMasterId",
                table: "UserPermissionChild",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "UserPermissionChild",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "PurchaseReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PartyName",
                table: "PurchaseReports",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
