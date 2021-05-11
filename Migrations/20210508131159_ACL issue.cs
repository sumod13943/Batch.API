using Microsoft.EntityFrameworkCore.Migrations;

namespace BatchAPI.Migrations
{
    public partial class ACLissue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_ACL_ACLId",
                table: "Batches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ACL",
                table: "ACL");

            migrationBuilder.RenameTable(
                name: "ACL",
                newName: "ACLs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ACLs",
                table: "ACLs",
                column: "Id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_ACLs",
                table: "ACLs");

            migrationBuilder.RenameTable(
                name: "ACLs",
                newName: "ACL");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ACL",
                table: "ACL",
                column: "Id");

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
