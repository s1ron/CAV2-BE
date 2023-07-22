using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBackEndV2.Migrations
{
    /// <inheritdoc />
    public partial class addcasecasetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Messages_MessageId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bb97ea1e-2346-4a3c-bb6f-d52a3e718a52", "AQAAAAIAAYagAAAAEFut3eK/0sJ6Opc0dWh6fVPlhwO48T1KdxAPoxrfGLRThaNqdAcnIOOMW7QCjthFxw==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Messages_MessageId",
                table: "Attachments",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Messages_MessageId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "37b4b19c-b5c3-464c-b914-6704039d8414", "AQAAAAIAAYagAAAAELNQMlOWMKagcVjv3ZaeOFcw0hzCOTHaEzLgIWF5ky0qOIFIE3NnhgfUJSof5eaY7A==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Messages_MessageId",
                table: "Attachments",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id");
        }
    }
}
