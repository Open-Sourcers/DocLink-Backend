using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSpecialtyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Specialties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDoctors",
                table: "Specialties",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "NumberOfDoctors",
                table: "Specialties");
        }
    }
}
