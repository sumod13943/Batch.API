using Microsoft.EntityFrameworkCore.Migrations;

namespace BatchAPI.Migrations
{
    public partial class ACLIDadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_ACL_ACLId",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "ACLId",
                table: "Batches",
                newName: "ACLId1");

            migrationBuilder.RenameIndex(
                name: "IX_Batches_ACLId",
                table: "Batches",
                newName: "IX_Batches_ACLId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_ACL_ACLId1",
                table: "Batches",
                column: "ACLId1",
                principalTable: "ACL",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_ACL_ACLId1",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "ACLId1",
                table: "Batches",
                newName: "ACLId");

            migrationBuilder.RenameIndex(
                name: "IX_Batches_ACLId1",
                table: "Batches",
                newName: "IX_Batches_ACLId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_ACL_ACLId",
                table: "Batches",
                column: "ACLId",
                principalTable: "ACL",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
