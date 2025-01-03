using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class RelationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionChild_UserId",
                table: "UserPermissionChild",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissionChild_UserMaster_UserId",
                table: "UserPermissionChild",
                column: "UserId",
                principalTable: "UserMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissionChild_UserMaster_UserId",
                table: "UserPermissionChild");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissionChild_UserId",
                table: "UserPermissionChild");
        }
    }
}
