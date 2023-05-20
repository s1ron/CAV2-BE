using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEndV2.Migrations
{
    /// <inheritdoc />
    public partial class addthemename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThemeName",
                table: "ConversationThemes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ca6ae2ad-5bfb-4277-821e-e6cd43e9a2cc", "AQAAAAIAAYagAAAAEIEs3hxndj+yy2JaZbl+xo2AvaviUic+I2H3oQB5eP9ATy8fgpZK1jpbzV7xK+1Ihw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeName",
                table: "ConversationThemes");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b362d021-1ea1-41e6-ae2a-329e20dd49a7", "AQAAAAIAAYagAAAAELt/Eg8yv3DblcuTb9SaU5/qd8iYn4bRWoBAC54ztD4SsO5ClWNF7Dz+4bS6KtsnJQ==" });
        }
    }
}
