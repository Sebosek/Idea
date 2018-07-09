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
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    Removed = table.Column<DateTime>(nullable: true),
                    RemovedBy = table.Column<Guid>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: false),
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
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    Removed = table.Column<DateTime>(nullable: true),
                    RemovedBy = table.Column<Guid>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: false),
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
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    Removed = table.Column<DateTime>(nullable: true),
                    RemovedBy = table.Column<Guid>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: false),
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
                columns: new[] { "Id", "Created", "CreatedBy", "Name", "Removed", "RemovedBy", "Symbol", "Updated", "UpdatedBy" },
                values: new object[] { new Guid("011dbeeb-895d-4bde-b651-91ccf204b9fa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "Kilogram", null, new Guid("00000000-0000-0000-0000-000000000000"), "Kg", null, new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "Unit",
                columns: new[] { "Id", "Created", "CreatedBy", "Name", "Removed", "RemovedBy", "Symbol", "Updated", "UpdatedBy" },
                values: new object[] { new Guid("4ec94793-cc7f-45cc-a06a-05c65f0c2d58"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "Gram", null, new Guid("00000000-0000-0000-0000-000000000000"), "g", null, new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "Unit",
                columns: new[] { "Id", "Created", "CreatedBy", "Name", "Removed", "RemovedBy", "Symbol", "Updated", "UpdatedBy" },
                values: new object[] { new Guid("2611bf28-869e-4fdb-84e7-6b418abb3cca"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "Liter", null, new Guid("00000000-0000-0000-0000-000000000000"), "l", null, new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecipeId",
                table: "Ingredient",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_Removed",
                table: "Ingredient",
                column: "Removed");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_UnitId",
                table: "Ingredient",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_UnitId1",
                table: "Ingredient",
                column: "UnitId1");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_Removed",
                table: "Recipe",
                column: "Removed");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_Removed",
                table: "Unit",
                column: "Removed");
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
