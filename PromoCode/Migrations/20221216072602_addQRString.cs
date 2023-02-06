using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCode.Migrations
{
    /// <inheritdoc />
    public partial class addQRString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "qrString",
                table: "PromoCode",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "qrString",
                table: "PromoCode");
        }
    }
}
