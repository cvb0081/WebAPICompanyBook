using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPIBook.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f"), "Address test 123", "USA", "IT Company" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("c4952391-bbc9-47e2-94b7-e50ce255fe26"), "Adress 2 test 123", "EU", "Admin Company" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("39317070-8da4-4b8e-adfc-03a46b31d4b9"), 26, new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f"), "Sam Raiden", "Software Developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("42dc3e81-1e45-415a-8889-09fef70d08f2"), 30, new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f"), "Jana McLeaf", "Software Developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("6000dc78-6cc1-489d-8c18-2a82b42115e2"), 35, new Guid("c4952391-bbc9-47e2-94b7-e50ce255fe26"), "KaneMiller", "Software Developer" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("39317070-8da4-4b8e-adfc-03a46b31d4b9"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("42dc3e81-1e45-415a-8889-09fef70d08f2"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("6000dc78-6cc1-489d-8c18-2a82b42115e2"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("c4952391-bbc9-47e2-94b7-e50ce255fe26"));
        }
    }
}
