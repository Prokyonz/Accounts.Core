using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class AddedIsDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseSlipNo",
                table: "PurchaseReports");

            migrationBuilder.RenameColumn(
                name: "TotalItems",
                table: "PurchaseReports",
                newName: "InvoiceNo");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNo",
                table: "UserMaster",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "UserMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Stock",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "SeriesMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "SalesMasters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "SalesDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "SaleReport",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "DealerName",
                table: "PurchaseReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "PurchaseMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "PurchaseDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "POSMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "ItemMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "CustomerMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "BrokerMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "AmountReceived",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CustomerReport",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<long>(type: "bigint", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharImageFrontData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadhbarImageBackData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanImageData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureImageData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMaster_MobileNo",
                table: "UserMaster",
                column: "MobileNo",
                unique: true,
                filter: "[MobileNo] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerReport");

            migrationBuilder.DropTable(
                name: "UserReport");

            migrationBuilder.DropIndex(
                name: "IX_UserMaster_MobileNo",
                table: "UserMaster");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "UserMaster");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "SeriesMaster");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "SalesMasters");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SaleReport");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "PurchaseMaster");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "PurchaseDetails");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "POSMaster");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "ItemMaster");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "CustomerMaster");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "BrokerMaster");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "AmountReceived");

            migrationBuilder.RenameColumn(
                name: "InvoiceNo",
                table: "PurchaseReports",
                newName: "TotalItems");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNo",
                table: "UserMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DealerName",
                table: "PurchaseReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PurchaseSlipNo",
                table: "PurchaseReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
