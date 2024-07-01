using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Email.Services.EmailTrailAPI.Migrations
{
    public partial class AddEmailTrailTableToDb : Migration
    {
    /// <inheritdoc />
    /// 
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "TicketDetails",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                TicketId = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TicketDetails", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "EmailTrails",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                TicketId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ParentEmailId = table.Column<int>(type: "int", nullable: true),
                Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Recipient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TicketDetailsId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmailTrails", x => x.Id);
                table.ForeignKey(
                    name: "FK_EmailTrails_EmailTrails_ParentEmailId",
                    column: x => x.ParentEmailId,
                    principalTable: "EmailTrails",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_EmailTrails_TicketDetails_TicketDetailsId",
                    column: x => x.TicketDetailsId,
                    principalTable: "TicketDetails",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmailTrails_ParentEmailId",
            table: "EmailTrails",
            column: "ParentEmailId");

        migrationBuilder.CreateIndex(
            name: "IX_EmailTrails_TicketDetailsId",
            table: "EmailTrails",
            column: "TicketDetailsId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EmailTrails");

        migrationBuilder.DropTable(
            name: "TicketDetails");
    }
   
}
}
