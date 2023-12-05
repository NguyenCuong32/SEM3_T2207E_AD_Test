using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopMvc.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_UsersId",
                table: "AspNetRoles",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Users_UsersId",
                table: "AspNetRoles",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Users_UsersId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_UsersId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "AspNetRoles");
        }
    }
}
