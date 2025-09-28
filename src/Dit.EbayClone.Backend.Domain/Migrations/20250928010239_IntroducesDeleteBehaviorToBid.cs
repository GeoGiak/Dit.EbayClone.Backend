using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dit.EbayClone.Backend.Domain.Migrations
{
    /// <inheritdoc />
    public partial class IntroducesDeleteBehaviorToBid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Auctions_AuctionId",
                table: "Bids",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
