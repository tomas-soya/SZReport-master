using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SZReport.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SHAs",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Closing = table.Column<decimal>(nullable: false),
                    Opening = table.Column<decimal>(nullable: false),
                    Highest = table.Column<decimal>(nullable: false),
                    Lowest = table.Column<decimal>(nullable: false),
                    FrontOpening = table.Column<decimal>(nullable: false),
                    UpDown = table.Column<decimal>(nullable: false),
                    UpDownWidth = table.Column<decimal>(nullable: false),
                    Volume = table.Column<decimal>(nullable: false),
                    Turnover = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHAs", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SHAs");
        }
    }
}
