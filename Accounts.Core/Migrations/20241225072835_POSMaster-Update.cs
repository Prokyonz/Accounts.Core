using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class POSMasterUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMaster_POSMaster_POSMasterId",
                table: "UserMaster");

            migrationBuilder.DropIndex(
                name: "IX_UserMaster_POSMasterId",
                table: "UserMaster");

            migrationBuilder.DropColumn(
                name: "POSMasterId",
                table: "UserMaster");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "POSMasterId",
                table: "UserMaster",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMaster_POSMasterId",
                table: "UserMaster",
                column: "POSMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMaster_POSMaster_POSMasterId",
                table: "UserMaster",
                column: "POSMasterId",
                principalTable: "POSMaster",
                principalColumn: "Id");
        }
    }
}
