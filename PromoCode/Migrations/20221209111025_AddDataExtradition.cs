using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCode.Migrations
{
    /// <inheritdoc />
    public partial class AddDataExtradition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "extraditionDate",
                table: "PromoCode",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "extraditionDate",
                table: "PromoCode");
        }
    }
}
