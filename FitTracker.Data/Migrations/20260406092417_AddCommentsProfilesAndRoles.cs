using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsProfilesAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-5678-9012-abcd-ef1234567890",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "99d4b82f-4850-440b-b774-b3b306ee0e63", "AQAAAAIAAYagAAAAEKXItLB2tpTU+i0UsKsrMIm08OL98c7qk57DfTWvvsE0ByXMBwOx5PMrqD7utQiD2A==", "ad1fb19e-9626-4dfe-9c86-97c6e3b14d66" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-5678-9012-abcd-ef1234567890",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "966a8f4b-49fd-4565-a711-f78f14c43f5a", "AQAAAAIAAYagAAAAENr2OD7xCC8vvEuiFgZovboMZPzhMgV+jgODml+bmE2/6D4fmJ/yDB9XcVOSPHjfOg==", "ab4e8c84-e7fb-45cf-aaea-ce50f0ffa0b8" });
        }
    }
}
