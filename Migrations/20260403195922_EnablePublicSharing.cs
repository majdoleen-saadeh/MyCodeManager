using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCodeManager.Migrations
{
    /// <inheritdoc />
    public partial class EnablePublicSharing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Snippet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShareToken",
                table: "Snippet",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Snippet");

            migrationBuilder.DropColumn(
                name: "ShareToken",
                table: "Snippet");
        }
    }
}
