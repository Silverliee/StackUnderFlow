using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackUnderFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    UserId1 = table.Column<int>(type: "int", nullable: false),
                    UserId2 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => new { x.UserId1, x.UserId2 });
                    table.CheckConstraint("CK_Friend_UserIds_Different", "[UserId1] <> [UserId2]");
                    table.ForeignKey(
                        name: "FK_Friend_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Friend_Users_UserId2",
                        column: x => x.UserId2,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friend_UserId2",
                table: "Friend",
                column: "UserId2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friend");
        }
    }
}
