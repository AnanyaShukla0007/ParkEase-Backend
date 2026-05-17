using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Infrastructure.Migrations
{
    public partial class AddManagerApplicationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerApplicationNotes",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerApplicationStatus",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NONE");

            migrationBuilder.AddColumn<string>(
                name: "ProposedLotAddress",
                table: "Users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposedLotCity",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposedLotName",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ManagerApplicationNotes", table: "Users");
            migrationBuilder.DropColumn(name: "ManagerApplicationStatus", table: "Users");
            migrationBuilder.DropColumn(name: "ProposedLotAddress", table: "Users");
            migrationBuilder.DropColumn(name: "ProposedLotCity", table: "Users");
            migrationBuilder.DropColumn(name: "ProposedLotName", table: "Users");
        }
    }
}
