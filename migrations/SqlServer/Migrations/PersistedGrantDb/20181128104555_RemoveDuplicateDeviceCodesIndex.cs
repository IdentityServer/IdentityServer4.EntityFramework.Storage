using Microsoft.EntityFrameworkCore.Migrations;

namespace SqlServer.Migrations.PersistedGrantDb
{
    public partial class RemoveDuplicateDeviceCodesIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeviceCodes_UserCode",
                table: "DeviceCodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_UserCode",
                table: "DeviceCodes",
                column: "UserCode",
                unique: true);
        }
    }
}
