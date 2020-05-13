using Microsoft.EntityFrameworkCore.Migrations;

namespace Flight_Tracker.Migrations
{
    public partial class newData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e37d491-99f7-444e-9133-e9a8bbfdacdf");

            migrationBuilder.AlterColumn<int>(
                name: "ZipCode",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b23cc6d2-7989-4bc2-8d87-78979bc1aea8", "fb1f04ae-6766-4cfa-94ad-4c1c2a775ceb", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b23cc6d2-7989-4bc2-8d87-78979bc1aea8");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6e37d491-99f7-444e-9133-e9a8bbfdacdf", "acdf7d9b-29dd-4647-a577-cb95d98b8f6d", "Customer", "CUSTOMER" });
        }
    }
}
