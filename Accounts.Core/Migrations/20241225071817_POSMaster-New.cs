using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class POSMasterNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "POSMasterId",
                table: "UserMaster",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "POSMaster",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TIDBankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeriesMaster",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesMaster", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMaster_POSMaster_POSMasterId",
                table: "UserMaster");

            migrationBuilder.DropTable(
                name: "POSMaster");

            migrationBuilder.DropTable(
                name: "SeriesMaster");

            migrationBuilder.DropIndex(
                name: "IX_UserMaster_POSMasterId",
                table: "UserMaster");

            migrationBuilder.DropColumn(
                name: "POSMasterId",
                table: "UserMaster");
        }
    }
}
