using Microsoft.EntityFrameworkCore.Migrations;

namespace BatchAPI.Migrations
{
    public partial class ACLissuefixe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Batches_ACLID",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "AId",
                table: "Batches");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_ACLID",
                table: "Batches",
                column: "ACLID",
                unique: true,
                filter: "[ACLID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Batches_ACLID",
                table: "Batches");

            migrationBuilder.AddColumn<int>(
                name: "AId",
                table: "Batches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Batches_ACLID",
                table: "Batches",
                column: "ACLID");
        }
    }
}
