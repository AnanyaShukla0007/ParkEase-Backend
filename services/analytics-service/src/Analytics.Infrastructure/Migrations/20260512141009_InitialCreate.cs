using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Analytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemandSignals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LotId = table.Column<int>(type: "integer", nullable: true),
                    EventType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    SearchTerm = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Reason = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandSignals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OccupancyLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LotId = table.Column<int>(type: "integer", nullable: false),
                    SpotId = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OccupancyRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    AvailableSpots = table.Column<int>(type: "integer", nullable: false),
                    TotalSpots = table.Column<int>(type: "integer", nullable: false),
                    VehicleType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OccupancyLogs", x => x.LogId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandSignals_City",
                table: "DemandSignals",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_DemandSignals_OccurredAt",
                table: "DemandSignals",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_OccupancyLogs_LotId",
                table: "OccupancyLogs",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_OccupancyLogs_Timestamp",
                table: "OccupancyLogs",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandSignals");

            migrationBuilder.DropTable(
                name: "OccupancyLogs");
        }
    }
}
