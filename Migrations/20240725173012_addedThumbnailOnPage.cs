﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedThumbnailOnPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Pages");
        }
    }
}
