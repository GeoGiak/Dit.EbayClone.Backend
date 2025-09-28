using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dit.EbayClone.Backend.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemovesForeignKeyDependenciesFromAuctionImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionImages_Auctions_AuctionId1",
                table: "AuctionImages");

            migrationBuilder.DropIndex(
                name: "IX_AuctionImages_AuctionId1",
                table: "AuctionImages");

            migrationBuilder.DropColumn(
                name: "AuctionId1",
                table: "AuctionImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuctionId1",
                table: "AuctionImages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AuctionImages_AuctionId1",
                table: "AuctionImages",
                column: "AuctionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionImages_Auctions_AuctionId1",
                table: "AuctionImages",
                column: "AuctionId1",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
