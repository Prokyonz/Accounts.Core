using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class UpdateMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_PurchaseMasterId",
                table: "PurchaseDetails",
                column: "PurchaseMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_PurchaseMaster_PurchaseMasterId",
                table: "PurchaseDetails",
                column: "PurchaseMasterId",
                principalTable: "PurchaseMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_PurchaseMaster_PurchaseMasterId",
                table: "PurchaseDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseDetails_PurchaseMasterId",
                table: "PurchaseDetails");
        }
    }
}
