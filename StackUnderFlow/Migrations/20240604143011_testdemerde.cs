using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackUnderFlow.Migrations
{
    /// <inheritdoc />
    public partial class testdemerde : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "coucou",
                table: "ScriptVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "coucou",
                table: "ScriptVersions");
        }
    }
}
