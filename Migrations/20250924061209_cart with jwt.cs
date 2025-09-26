using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecoranestBacknd.Migrations
{
    /// <inheritdoc />
    public partial class cartwithjwt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserID",
                table: "Carts");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Carts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "GuestId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserID",
                table: "Carts",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Carts");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserID",
                table: "Carts",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
