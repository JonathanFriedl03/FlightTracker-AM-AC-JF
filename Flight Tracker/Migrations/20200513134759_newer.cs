using Microsoft.EntityFrameworkCore.Migrations;

namespace Flight_Tracker.Migrations
{
    public partial class newer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "272f8f43-9ba0-44ec-938f-6298edfb3a08");

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
                values: new object[] { "ca861844-3e6f-42f2-8740-19628a6650b3", "d147a584-c494-4ef3-9b0e-620d15029298", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca861844-3e6f-42f2-8740-19628a6650b3");

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
                values: new object[] { "272f8f43-9ba0-44ec-938f-6298edfb3a08", "f68fb2e0-f1cc-45e2-bcd2-7d79be64de05", "Customer", "CUSTOMER" });
        }
    }
}
