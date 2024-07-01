using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Email.Services.InboundEmailAPI.Migrations
{
    /// <inheritdoc />
    public partial class OutboundEmailService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketId",
                table: "InboundEmails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "InboundEmails");
        }
    }
}
