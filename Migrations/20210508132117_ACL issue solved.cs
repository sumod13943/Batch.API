using Microsoft.EntityFrameworkCore.Migrations;

namespace BatchAPI.Migrations
{
    public partial class ACLissuesolved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_ACLs_ACLId",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "ACLId",
                table: "Batches",
                newName: "ACLID");

            migrationBuilder.RenameIndex(
                name: "IX_Batches_ACLId",
                table: "Batches",
                newName: "IX_Batches_ACLID");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_ACLs_ACLID",
                table: "Batches",
                column: "ACLID",
                principalTable: "ACLs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_ACLs_ACLID",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "ACLID",
                table: "Batches",
                newName: "ACLId");

            migrationBuilder.RenameIndex(
                name: "IX_Batches_ACLID",
                table: "Batches",
                newName: "IX_Batches_ACLId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_ACLs_ACLId",
                table: "Batches",
                column: "ACLId",
                principalTable: "ACLs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
