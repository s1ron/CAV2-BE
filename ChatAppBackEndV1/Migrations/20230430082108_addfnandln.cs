using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEndV2.Migrations
{
    /// <inheritdoc />
    public partial class addfnandln : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AppUsers",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FisrtName",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "FisrtName", "LastName", "PasswordHash" },
                values: new object[] { "26991bbd-1a94-4eda-a642-a96df5f61026", "Vo Anh", "Vu", "AQAAAAIAAYagAAAAEG/FXnb0u1CFU1mRFF7ZrH8aOVUTrdXqbB01HjpB/zku38iNdM8jCRgNPROcEn4X0Q==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FisrtName",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AppUsers",
                newName: "FullName");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "FullName", "PasswordHash" },
                values: new object[] { "595b9450-0e16-493d-bafb-e0761e7a278e", "VoAnhVu", "AQAAAAIAAYagAAAAELDEtW1dxGW3uBzdMpr074mXt/G64MMfnmpMCCcnmd/R6OC4xSIhqyXhrrPNPiutlw==" });
        }
    }
}
