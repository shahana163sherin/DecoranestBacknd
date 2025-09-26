﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecoranestBacknd.Migrations
{
    /// <inheritdoc />
    public partial class OrderCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Orders",
                newName: "OrderDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Orders",
                newName: "CreatedAt");
        }
    }
}
