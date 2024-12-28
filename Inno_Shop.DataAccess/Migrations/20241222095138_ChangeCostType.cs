using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inno_Shop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCostType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b83a87f8-6cbd-4057-b027-fc93fa70845f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8fb3d9a-9f2b-454f-9c66-42ce2e7cc55c");

            migrationBuilder.AlterColumn<float>(
                name: "Cost",
                table: "Products",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ed14e2c-d9ef-4e3f-ad4a-4cd19a1aa637", null, "Admin", "ADMIN" },
                    { "f456bb22-e976-43a8-b8ab-080be2cf92c0", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ed14e2c-d9ef-4e3f-ad4a-4cd19a1aa637");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f456bb22-e976-43a8-b8ab-080be2cf92c0");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b83a87f8-6cbd-4057-b027-fc93fa70845f", null, "User", "USER" },
                    { "c8fb3d9a-9f2b-454f-9c66-42ce2e7cc55c", null, "Admin", "ADMIN" }
                });
        }
    }
}
