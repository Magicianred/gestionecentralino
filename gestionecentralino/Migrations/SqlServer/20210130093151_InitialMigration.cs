using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gestionecentralino.Migrations.SqlServer
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternalNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CdCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Incoming = table.Column<bool>(type: "bit", nullable: false),
                    Sede = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calls", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calls");
        }
    }
}
