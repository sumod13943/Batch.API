using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BatchAPI.Migrations
{
    public partial class ChangeinBatchfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchFiles_Batches_BatchId",
                table: "BatchFiles");

            migrationBuilder.DropIndex(
                name: "IX_BatchFiles_BatchId",
                table: "BatchFiles");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "BatchFiles");

            migrationBuilder.AddColumn<int>(
                name: "BatchFileId",
                table: "Batches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Batches_BatchFileId",
                table: "Batches",
                column: "BatchFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_BatchFiles_BatchFileId",
                table: "Batches",
                column: "BatchFileId",
                principalTable: "BatchFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_BatchFiles_BatchFileId",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Batches_BatchFileId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "BatchFileId",
                table: "Batches");

            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
                table: "BatchFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BatchFiles_BatchId",
                table: "BatchFiles",
                column: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchFiles_Batches_BatchId",
                table: "BatchFiles",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "BatchId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
