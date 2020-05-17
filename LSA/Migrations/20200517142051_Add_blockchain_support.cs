using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LSA.Migrations
{
    public partial class Add_blockchain_support : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TastingIsFinished",
                table: "TastingHistory");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "TastingHistory",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PreviousTastingHistoryId",
                table: "TastingHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TastingHistoryPreviousId",
                table: "TastingHistory",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TransactionDate",
                table: "TastingHistory",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_TastingHistory_PreviousTastingHistoryId",
                table: "TastingHistory",
                column: "PreviousTastingHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TastingHistory_TastingHistory_PreviousTastingHistoryId",
                table: "TastingHistory",
                column: "PreviousTastingHistoryId",
                principalTable: "TastingHistory",
                principalColumn: "TastingHistoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TastingHistory_TastingHistory_PreviousTastingHistoryId",
                table: "TastingHistory");

            migrationBuilder.DropIndex(
                name: "IX_TastingHistory_PreviousTastingHistoryId",
                table: "TastingHistory");

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "TastingHistory");

            migrationBuilder.DropColumn(
                name: "PreviousTastingHistoryId",
                table: "TastingHistory");

            migrationBuilder.DropColumn(
                name: "TastingHistoryPreviousId",
                table: "TastingHistory");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "TastingHistory");

            migrationBuilder.AddColumn<bool>(
                name: "TastingIsFinished",
                table: "TastingHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
