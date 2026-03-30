using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookTypes",
                columns: new[] { "TypeId", "Description", "TypeName" },
                values: new object[] { 1, null, "Fiction" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleId", "Description", "RoleName" },
                values: new object[] { 1, null, "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreateDate", "Email", "Password", "Phone", "UserName", "UserRoleId" },
                values: new object[] { 1, new DateTime(2025, 12, 19, 11, 59, 3, 330, DateTimeKind.Utc).AddTicks(930), "admin@bookstore.com", "admin123", null, "admin", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookTypes",
                keyColumn: "TypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: 1);
        }
    }
}
