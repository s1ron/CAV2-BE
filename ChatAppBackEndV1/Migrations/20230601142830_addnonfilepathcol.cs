using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEndV2.Migrations
{
    /// <inheritdoc />
    public partial class addnonfilepathcol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NonFilePath",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "37b4b19c-b5c3-464c-b914-6704039d8414", "AQAAAAIAAYagAAAAELNQMlOWMKagcVjv3ZaeOFcw0hzCOTHaEzLgIWF5ky0qOIFIE3NnhgfUJSof5eaY7A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NonFilePath",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "34e70a81-df7e-43e2-8413-49f4354e98e1", "AQAAAAIAAYagAAAAEDZ2+5yoN1pnO8H1B4DWFqaUK74R12Mm2usn1oCH1yXFCzm39FyzDLygIZ67UYTbSA==" });
        }
    }
}
