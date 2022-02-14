using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPIBook.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ecf67e70-22db-4acf-a33b-e146540fb5ac", "2b7f5c24-1ae5-4949-90cf-e7f6a5995aff", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e0f7fc3d-2b84-4006-8ec3-0369f1310289", "5724d5ca-74b7-4c8e-8f4d-84083093dfb8", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0f7fc3d-2b84-4006-8ec3-0369f1310289");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ecf67e70-22db-4acf-a33b-e146540fb5ac");
        }
    }
}
