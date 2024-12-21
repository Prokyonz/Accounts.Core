using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class AddGSTColumnsInPurchaseDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GSTAmount",
                table: "PurchaseDetails",
                newName: "SGST");

            migrationBuilder.AddColumn<long>(
                name: "UserMasterId",
                table: "UserPermissionChild",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CGST",
                table: "PurchaseDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GSTPer",
                table: "PurchaseDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionChild_UserMasterId",
                table: "UserPermissionChild",
                column: "UserMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissionChild_UserMaster_UserMasterId",
                table: "UserPermissionChild",
                column: "UserMasterId",
                principalTable: "UserMaster",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissionChild_UserMaster_UserMasterId",
                table: "UserPermissionChild");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissionChild_UserMasterId",
                table: "UserPermissionChild");

            migrationBuilder.DropColumn(
                name: "UserMasterId",
                table: "UserPermissionChild");

            migrationBuilder.DropColumn(
                name: "CGST",
                table: "PurchaseDetails");

            migrationBuilder.DropColumn(
                name: "GSTPer",
                table: "PurchaseDetails");

            migrationBuilder.RenameColumn(
                name: "SGST",
                table: "PurchaseDetails",
                newName: "GSTAmount");
        }
    }
}
