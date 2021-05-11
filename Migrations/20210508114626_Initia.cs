using Microsoft.EntityFrameworkCore.Migrations;

namespace BatchAPI.Migrations
{
    public partial class Initia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_ACL_ACLId",
                table: "Batches");

            migrationBuilder.AlterColumn<int>(
                name: "ACLId",
                table: "Batches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ReadGroups",
                table: "ACL",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReadUsers",
                table: "ACL",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_ACL_ACLId",
                table: "Batches",
                column: "ACLId",
                principalTable: "ACL",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_ACL_ACLId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "ReadGroups",
                table: "ACL");

            migrationBuilder.DropColumn(
                name: "ReadUsers",
                table: "ACL");

            migrationBuilder.AlterColumn<int>(
                name: "ACLId",
                table: "Batches",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_ACL_ACLId",
                table: "Batches",
                column: "ACLId",
                principalTable: "ACL",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
