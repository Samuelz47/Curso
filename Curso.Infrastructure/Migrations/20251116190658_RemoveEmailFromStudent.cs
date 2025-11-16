using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Curso.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailFromStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_Email",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);
        }
    }
}
