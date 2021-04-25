using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountService.Migrations
{
    public partial class accountDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsDeleted", "Name", "Status" },
                values: new object[] { new Guid("eef82b08-13ca-4cae-a64d-c412a26ee3de"), false, "User 1", true });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsDeleted", "Name", "Status" },
                values: new object[] { new Guid("19d6a2c8-94c9-48fd-a935-cbb3cadb5d78"), false, "User 2", true });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsDeleted", "Name", "Status" },
                values: new object[] { new Guid("d4358276-7e03-4637-8ecf-9c0260208457"), false, "User 3", true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
