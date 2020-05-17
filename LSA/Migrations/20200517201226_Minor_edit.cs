using Microsoft.EntityFrameworkCore.Migrations;

namespace LSA.Migrations
{
    public partial class Minor_edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TastingHistory_TastingHistory_PreviousTastingHistoryId",
                table: "TastingHistory");

            migrationBuilder.DropIndex(
                name: "IX_TastingHistory_PreviousTastingHistoryId",
                table: "TastingHistory");

            migrationBuilder.DropColumn(
                name: "PreviousTastingHistoryId",
                table: "TastingHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreviousTastingHistoryId",
                table: "TastingHistory",
                type: "int",
                nullable: true);

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
    }
}
