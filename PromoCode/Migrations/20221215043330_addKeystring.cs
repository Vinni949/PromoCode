using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCode.Migrations
{
    /// <inheritdoc />
    public partial class addKeystring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "keySessions",
                table: "PromoCode",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "qr",
                table: "loginViewModels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "keySessions",
                table: "PromoCode");

            migrationBuilder.DropColumn(
                name: "qr",
                table: "loginViewModels");
        }
    }
}
