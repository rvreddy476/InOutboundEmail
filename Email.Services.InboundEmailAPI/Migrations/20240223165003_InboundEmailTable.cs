using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Email.Services.InboundEmailAPI.Migrations
{
    /// <inheritdoc />
    public partial class InboundEmailTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "InboundEmails");

            migrationBuilder.DropColumn(
                name: "Recipient",
                table: "InboundEmails");

            migrationBuilder.DropColumn(
                name: "Email_id",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "File_data",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "File_name",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "InboundEmails",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Sender",
                table: "InboundEmails",
                newName: "From");

            migrationBuilder.RenameColumn(
                name: "File_type",
                table: "Attachments",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "File_size",
                table: "Attachments",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Attachment_id",
                table: "Attachments",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "From",
                table: "InboundEmails",
                newName: "Sender");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "InboundEmails",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Attachments",
                newName: "File_type");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Attachments",
                newName: "File_size");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Attachments",
                newName: "Attachment_id");

            migrationBuilder.AddColumn<string>(
                name: "IsRead",
                table: "InboundEmails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Recipient",
                table: "InboundEmails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email_id",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "File_data",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "File_name",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
