using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportAuditSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDispatchAccountability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cadence",
                table: "TaskItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "TaskItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EscalateToRole",
                table: "TaskItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EscalationWindow",
                table: "TaskItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleRole",
                table: "TaskItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HodSignOffAt",
                table: "ChecklistSubmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HodSignOffName",
                table: "ChecklistSubmissions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RosterPeople",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListKind = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RosterPeople", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RosterPeople_ListKind_Name",
                table: "RosterPeople",
                columns: new[] { "ListKind", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RosterPeople");

            migrationBuilder.DropColumn(
                name: "Cadence",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "EscalateToRole",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "EscalationWindow",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "ResponsibleRole",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "HodSignOffAt",
                table: "ChecklistSubmissions");

            migrationBuilder.DropColumn(
                name: "HodSignOffName",
                table: "ChecklistSubmissions");
        }
    }
}
