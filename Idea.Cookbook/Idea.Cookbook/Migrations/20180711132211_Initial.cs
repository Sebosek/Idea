using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Idea.Cookbook.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Directions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: true),
                    UnitId = table.Column<Guid>(nullable: false),
                    UnitId1 = table.Column<Guid>(nullable: true),
                    RecipeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredient_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ingredient_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingredient_Unit_UnitId1",
                        column: x => x.UnitId1,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Unit",
                columns: new[] { "Id", "Name", "Symbol" },
                values: new object[,]
                {
                    { new Guid("3078babe-42f2-41cf-a0a9-e3b0c7c7bf45"), "Kilogram", "Kg" },
                    { new Guid("f8c2854d-15eb-4372-930c-70244566bfb7"), "Gram", "g" },
                    { new Guid("576f18fe-fcdb-4937-969d-4480d3c5128f"), "Liter", "l" },
                    { new Guid("4463240b-eacc-45ca-89d0-aa8d326b87ae"), "Mililiter", "ml" },
                    { new Guid("1901a437-8249-44be-8373-0e72da21ff9e"), "Tea spoon", "tsp" },
                    { new Guid("b0bf7ddb-45be-4f31-836a-53476eb9cb65"), "Spoon", "sp" },
                    { new Guid("3fb0208b-cb90-431a-a4cd-6d38636c7706"), "Piece", "piece" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecipeId",
                table: "Ingredient",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_UnitId",
                table: "Ingredient",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_UnitId1",
                table: "Ingredient",
                column: "UnitId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropTable(
                name: "Unit");
        }
    }
}
