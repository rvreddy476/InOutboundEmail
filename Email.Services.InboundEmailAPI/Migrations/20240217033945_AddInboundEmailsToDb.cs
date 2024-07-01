using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Email.Services.InboundEmailAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddInboundEmailsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Attachment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Attachment_id);
                });

            migrationBuilder.CreateTable(
                name: "EmailRecipients",
                columns: table => new
                {
                    RecipentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email_Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailRecipients", x => x.RecipentId);
                });

            migrationBuilder.CreateTable(
                name: "EmailSenders",
                columns: table => new
                {
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email_Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSenders", x => x.SenderId);
                });

            migrationBuilder.CreateTable(
                name: "InboundEmails",
                columns: table => new
                {
                    EmailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundEmails", x => x.EmailId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "EmailRecipients");

            migrationBuilder.DropTable(
                name: "EmailSenders");

            migrationBuilder.DropTable(
                name: "InboundEmails");
        }
    }
}
