using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finanacial_Transaction_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAddresses_Customer_CustomerId",
                table: "CustomerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerEmails_Customer_CustomerId",
                table: "CustomerEmails");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPhones_Customer_CustomerId",
                table: "CustomerPhones");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Customer_CustomerId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAddresses_Customers_CustomerId",
                table: "CustomerAddresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerEmails_Customers_CustomerId",
                table: "CustomerEmails",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPhones_Customers_CustomerId",
                table: "CustomerPhones",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Customers_CustomerId",
                table: "Transactions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAddresses_Customers_CustomerId",
                table: "CustomerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerEmails_Customers_CustomerId",
                table: "CustomerEmails");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPhones_Customers_CustomerId",
                table: "CustomerPhones");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Customers_CustomerId",
                table: "Transactions");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    TempId = table.Column<int>(type: "int", nullable: false),
                    TempId1 = table.Column<int>(type: "int", nullable: false),
                    TempId2 = table.Column<int>(type: "int", nullable: false),
                    TempId3 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_Customer_TempId", x => x.TempId);
                    table.UniqueConstraint("AK_Customer_TempId1", x => x.TempId1);
                    table.UniqueConstraint("AK_Customer_TempId2", x => x.TempId2);
                    table.UniqueConstraint("AK_Customer_TempId3", x => x.TempId3);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAddresses_Customer_CustomerId",
                table: "CustomerAddresses",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerEmails_Customer_CustomerId",
                table: "CustomerEmails",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPhones_Customer_CustomerId",
                table: "CustomerPhones",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "TempId2",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Customer_CustomerId",
                table: "Transactions",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "TempId3",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
