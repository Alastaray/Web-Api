using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspEndpoint.Migrations
{
    public partial class UpdateRefreshSessionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fingerprint",
                table: "RefreshSessions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fingerprint",
                table: "RefreshSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
