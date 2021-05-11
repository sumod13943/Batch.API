using Microsoft.EntityFrameworkCore.Migrations;

namespace BatchAPI.Migrations
{
    public partial class ChangeinBatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_ACLs_ACLID",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Batches_ACLID",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "ACLID",
                table: "Batches",
                newName: "ACLId");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_ACLId",
                table: "Batches",
                column: "ACLId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_ACLs_ACLId",
                table: "Batches",
                column: "ACLId",
                principalTable: "ACLs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_ACLs_ACLId",
                table: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_Batches_ACLId",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "ACLId",
                table: "Batches",
                newName: "ACLID");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_ACLID",
                table: "Batches",
                column: "ACLID",
                unique: true,
                filter: "[ACLID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_ACLs_ACLID",
                table: "Batches",
                column: "ACLID",
                principalTable: "ACLs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
