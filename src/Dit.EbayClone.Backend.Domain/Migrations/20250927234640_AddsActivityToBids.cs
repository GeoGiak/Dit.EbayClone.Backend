using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dit.EbayClone.Backend.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddsActivityToBids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidHistory");

            migrationBuilder.AddColumn<Guid>(
                name: "AuctionHistoryId",
                table: "Bids",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Bids",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionHistoryId",
                table: "Bids",
                column: "AuctionHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AuctionHistory_AuctionHistoryId",
                table: "Bids",
                column: "AuctionHistoryId",
                principalTable: "AuctionHistory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AuctionHistory_AuctionHistoryId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_AuctionHistoryId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "AuctionHistoryId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Bids");

            migrationBuilder.CreateTable(
                name: "BidHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuctionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    DateOfBid = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BidHistory_AuctionHistory_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "AuctionHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BidHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BidHistory_AuctionId",
                table: "BidHistory",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_BidHistory_UserId",
                table: "BidHistory",
                column: "UserId");
        }
    }
}
