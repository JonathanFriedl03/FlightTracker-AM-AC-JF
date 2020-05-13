using Microsoft.EntityFrameworkCore.Migrations;

namespace Flight_Tracker.Migrations
{
    public partial class data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78917f26-8b20-4117-b1f5-f38a1f461c90");

            migrationBuilder.AlterColumn<double>(
                name: "duration",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "distance",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "272f8f43-9ba0-44ec-938f-6298edfb3a08", "f68fb2e0-f1cc-45e2-bcd2-7d79be64de05", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "272f8f43-9ba0-44ec-938f-6298edfb3a08");

            migrationBuilder.AlterColumn<int>(
                name: "duration",
                table: "Customers",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "distance",
                table: "Customers",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "78917f26-8b20-4117-b1f5-f38a1f461c90", "906b23f9-91bb-41d2-a780-88db35933a9d", "Customer", "CUSTOMER" });
        }
    }
}
