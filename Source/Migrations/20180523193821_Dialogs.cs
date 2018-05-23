using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AspNetCore.Migrations
{
    public partial class Dialogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_FromId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ToId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_FromId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ToId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FromId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ToId",
                table: "Messages");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DialogueId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromUserId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dialogues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dialogues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dialogues_AspNetUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dialogues_AspNetUsers_ToId",
                        column: x => x.ToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DialogueId",
                table: "Messages",
                column: "DialogueId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromUserId",
                table: "Messages",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogues_FromId",
                table: "Dialogues",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogues_ToId",
                table: "Dialogues",
                column: "ToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Dialogues_DialogueId",
                table: "Messages",
                column: "DialogueId",
                principalTable: "Dialogues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_FromUserId",
                table: "Messages",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Dialogues_DialogueId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_FromUserId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Dialogues");

            migrationBuilder.DropIndex(
                name: "IX_Messages_DialogueId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_FromUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DialogueId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "FromId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToId",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromId",
                table: "Messages",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToId",
                table: "Messages",
                column: "ToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_FromId",
                table: "Messages",
                column: "FromId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ToId",
                table: "Messages",
                column: "ToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
