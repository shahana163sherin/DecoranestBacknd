using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecoranestBacknd.Migrations
{
    /// <inheritdoc />
    public partial class CartChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Carts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
