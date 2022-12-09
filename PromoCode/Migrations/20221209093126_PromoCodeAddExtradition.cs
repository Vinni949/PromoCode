using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCode.Migrations
{
    /// <inheritdoc />
    public partial class PromoCodeAddExtradition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "extradition",
                table: "PromoCode",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "extradition",
                table: "PromoCode");
        }
    }
}
