using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalesOrderNumber",
                table: "SalesOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesOrderNumber",
                table: "SalesOrders");
        }
    }
}
