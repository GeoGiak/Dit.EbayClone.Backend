using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dit.EbayClone.Backend.Domain.Migrations
{
    /// <inheritdoc />
    public partial class FixesBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Auctions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Auctions");
        }
    }
}
