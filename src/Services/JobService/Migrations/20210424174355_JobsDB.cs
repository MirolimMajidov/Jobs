using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobService.Migrations
{
    public partial class JobsDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "IsDeleted", "Name", "Status" },
                values: new object[,]
                {
                    { new Guid("aff61f2d-02f4-42de-896d-5aace3880669"), "Web Development, Mobile Development, Desktop Software Developmen, QA & Testing", false, "Web, Mobile & Software Dev", true },
                    { new Guid("c8726fe0-3ac9-4352-bef3-0819b53f3768"), "Sales & Marketing Strategy", false, "Sales & Marketing", true },
                    { new Guid("4887cb56-9a91-4fe0-bcbe-8a03a49112ea"), "Design, Writing, Photography & Translator", false, "Design & Writing", true },
                    { new Guid("018211c5-1114-4ce3-9f6a-c836f857a529"), "Engineering & Architecture", false, "Engineering & Architecture", true }
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "CategoryId", "Cost", "Description", "Duration", "IsDeleted", "Name", "Status", "Type" },
                values: new object[,]
                {
                    { new Guid("32e1eb2a-e0aa-4747-9f6b-c6c39a63169f"), new Guid("aff61f2d-02f4-42de-896d-5aace3880669"), 25, "ASP.Net Core and Xamarin developer", 2, false, "Back-end developer", true, 0 },
                    { new Guid("9724f2a5-4afd-49ba-8517-59df13a53c7b"), new Guid("aff61f2d-02f4-42de-896d-5aace3880669"), 25, "We need experienced Angular developer for short term project.", 1, false, "Angular Developer Needed", true, 0 },
                    { new Guid("e4d90499-5d77-45d8-8257-05e57554500f"), new Guid("c8726fe0-3ac9-4352-bef3-0819b53f3768"), 2000, "Salesperson needed", 2, false, "Salesperson", true, 1 },
                    { new Guid("752e9873-f453-4418-81cb-6a3b1f993533"), new Guid("4887cb56-9a91-4fe0-bcbe-8a03a49112ea"), 30, "Design & Photography needed to build mockup of mobile app", 4, false, "Design & Photography", true, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CategoryId",
                table: "Jobs",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
