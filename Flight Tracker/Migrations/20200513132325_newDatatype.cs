using Microsoft.EntityFrameworkCore.Migrations;

namespace Flight_Tracker.Migrations
{
    public partial class newDatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b23cc6d2-7989-4bc2-8d87-78979bc1aea8");

            migrationBuilder.AlterColumn<int>(
                name: "duration",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "distance",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "78917f26-8b20-4117-b1f5-f38a1f461c90", "906b23f9-91bb-41d2-a780-88db35933a9d", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78917f26-8b20-4117-b1f5-f38a1f461c90");

            migrationBuilder.AlterColumn<double>(
                name: "duration",
                table: "Customers",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "distance",
                table: "Customers",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b23cc6d2-7989-4bc2-8d87-78979bc1aea8", "fb1f04ae-6766-4cfa-94ad-4c1c2a775ceb", "Customer", "CUSTOMER" });
        }
    }
}
