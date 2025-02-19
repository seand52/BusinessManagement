using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalesOrderProductMakeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderProduct_Products_ProductId",
                table: "SalesOrderProduct");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SalesOrderProduct",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SalesOrderProduct",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderProduct_Products_ProductId",
                table: "SalesOrderProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderProduct_Products_ProductId",
                table: "SalesOrderProduct");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SalesOrderProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SalesOrderProduct",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderProduct_Products_ProductId",
                table: "SalesOrderProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
