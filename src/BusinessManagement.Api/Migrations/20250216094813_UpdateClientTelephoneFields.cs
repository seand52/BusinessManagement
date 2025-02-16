using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientTelephoneFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "Clients",
                newName: "Telephone2");

            migrationBuilder.AddColumn<string>(
                name: "Telephone1",
                table: "Clients",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telephone1",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "Telephone2",
                table: "Clients",
                newName: "Telephone");
        }
    }
}
