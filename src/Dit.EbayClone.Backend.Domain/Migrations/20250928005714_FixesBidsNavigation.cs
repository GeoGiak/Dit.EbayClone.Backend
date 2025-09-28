using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dit.EbayClone.Backend.Domain.Migrations
{
    /// <inheritdoc />
    public partial class FixesBidsNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Auctions_AuctionId1",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_AuctionId1",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "AuctionId1",
                table: "Bids");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuctionId1",
                table: "Bids",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionId1",
                table: "Bids",
                column: "AuctionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Auctions_AuctionId1",
                table: "Bids",
                column: "AuctionId1",
                principalTable: "Auctions",
                principalColumn: "Id");
        }
    }
}
