using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class moveThumbToFilesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Files");

            migrationBuilder.AddColumn<byte[]>(
                name: "Thumbnail",
                table: "FilesData",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "FilesData");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
