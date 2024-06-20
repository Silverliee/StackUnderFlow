using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackUnderFlow.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRequests_Groups_GroupId",
                table: "GroupRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRequests_Users_UserId",
                table: "GroupRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRequests_Groups_GroupId",
                table: "GroupRequests",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRequests_Users_UserId",
                table: "GroupRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRequests_Groups_GroupId",
                table: "GroupRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupRequests_Users_UserId",
                table: "GroupRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRequests_Groups_GroupId",
                table: "GroupRequests",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRequests_Users_UserId",
                table: "GroupRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
