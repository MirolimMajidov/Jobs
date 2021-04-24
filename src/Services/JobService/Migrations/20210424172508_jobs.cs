using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobService.Migrations
{
    public partial class jobs : Migration
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
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    { new Guid("482be1d9-7c96-4e5d-ad60-707cbd3862d6"), "Web Development, Mobile Development, Desktop Software Developmen, QA & Testing", false, "Web, Mobile & Software Dev", true },
                    { new Guid("5bd0d6ab-340d-4f7c-bb3b-55539422cac5"), "Sales & Marketing Strategy", false, "Sales & Marketing", true },
                    { new Guid("f4671ba3-0c0c-4adf-95da-af11f816ebe4"), "Design, Writing, Photography & Translator", false, "Design & Writing", true },
                    { new Guid("9cd993d3-de91-4cc9-b72a-2988f6be4107"), "Engineering & Architecture", false, "Engineering & Architecture", true }
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "CategoryId", "Cost", "Description", "Duration", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("11402986-27fc-4ba6-bd41-dc7bedda3245"), new Guid("482be1d9-7c96-4e5d-ad60-707cbd3862d6"), 25, "ASP.Net Core and Xamarin developer", 2, "Back-end developer", 0 },
                    { new Guid("ce6ee095-a4b4-464c-9634-2bf2dff0d798"), new Guid("482be1d9-7c96-4e5d-ad60-707cbd3862d6"), 25, "We need experienced Angular developer for short term project.", 1, "Angular Developer Needed", 0 },
                    { new Guid("97a09511-2607-4e53-a9c7-25716019beaa"), new Guid("5bd0d6ab-340d-4f7c-bb3b-55539422cac5"), 2000, "Salesperson needed", 2, "Salesperson", 1 },
                    { new Guid("e3901e7a-c0e3-4ae2-bed5-c3821f6e65d1"), new Guid("f4671ba3-0c0c-4adf-95da-af11f816ebe4"), 30, "Design & Photography needed to build mockup of mobile app", 4, "Design & Photography", 0 }
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
