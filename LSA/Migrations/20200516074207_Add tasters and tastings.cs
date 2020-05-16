using Microsoft.EntityFrameworkCore.Migrations;

namespace LSA.Migrations
{
    public partial class Addtastersandtastings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasters",
                columns: table => new
                {
                    TasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TasterName = table.Column<string>(nullable: false),
                    TasterSecondName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasters", x => x.TasterId);
                });

            migrationBuilder.CreateTable(
                name: "Tastings",
                columns: table => new
                {
                    TastingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TastingName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tastings", x => x.TastingId);
                });

            migrationBuilder.CreateTable(
                name: "TasterToTastings",
                columns: table => new
                {
                    TasterId = table.Column<int>(nullable: false),
                    TastingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasterToTastings", x => new { x.TasterId, x.TastingId });
                    table.ForeignKey(
                        name: "FK_TasterToTastings_Tasters_TasterId",
                        column: x => x.TasterId,
                        principalTable: "Tasters",
                        principalColumn: "TasterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasterToTastings_Tastings_TastingId",
                        column: x => x.TastingId,
                        principalTable: "Tastings",
                        principalColumn: "TastingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TasterToTastings_TastingId",
                table: "TasterToTastings",
                column: "TastingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TasterToTastings");

            migrationBuilder.DropTable(
                name: "Tasters");

            migrationBuilder.DropTable(
                name: "Tastings");
        }
    }
}
