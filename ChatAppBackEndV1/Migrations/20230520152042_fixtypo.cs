using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEndV2.Migrations
{
    /// <inheritdoc />
    public partial class fixtypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FisrtName",
                table: "AppUsers",
                newName: "FirstName");

            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "Attachments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "34e70a81-df7e-43e2-8413-49f4354e98e1", "AQAAAAIAAYagAAAAEDZ2+5yoN1pnO8H1B4DWFqaUK74R12Mm2usn1oCH1yXFCzm39FyzDLygIZ67UYTbSA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AppUsers",
                newName: "FisrtName");

            migrationBuilder.AlterColumn<float>(
                name: "FileSize",
                table: "Attachments",
                type: "real",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ca6ae2ad-5bfb-4277-821e-e6cd43e9a2cc", "AQAAAAIAAYagAAAAEIEs3hxndj+yy2JaZbl+xo2AvaviUic+I2H3oQB5eP9ATy8fgpZK1jpbzV7xK+1Ihw==" });
        }
    }
}
