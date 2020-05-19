using Microsoft.EntityFrameworkCore.Migrations;

namespace LSA.Migrations
{
    public partial class Add_connection_between_Product_and_Tasting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductToTastings",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    TastingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductToTastings", x => new { x.ProductId, x.TastingId });
                    table.ForeignKey(
                        name: "FK_ProductToTastings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductToTastings_Tastings_TastingId",
                        column: x => x.TastingId,
                        principalTable: "Tastings",
                        principalColumn: "TastingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductToTastings_TastingId",
                table: "ProductToTastings",
                column: "TastingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductToTastings");
        }
    }
}
