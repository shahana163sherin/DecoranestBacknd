﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecoranestBacknd.Migrations
{
    /// <inheritdoc />
    public partial class cartandorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Wishlists",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Wishlists",
                newName: "TotalPrice");
        }
    }
}
