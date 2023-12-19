using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessManagement.Migrations
{
    /// <inheritdoc />
    public partial class GrantPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // grant permissions to all tables in database dev_business_management to user seand in postgres
            migrationBuilder.Sql(@"GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO seand;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"REVOKE SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public FROM seand;");
        }
    }
}
