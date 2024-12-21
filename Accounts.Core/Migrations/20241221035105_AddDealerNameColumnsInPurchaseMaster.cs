using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class AddDealerNameColumnsInPurchaseMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "PurchaseMaster");

            migrationBuilder.AddColumn<string>(
                name: "DealerName",
                table: "PurchaseMaster",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DealerName",
                table: "PurchaseMaster");

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "PurchaseMaster",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
