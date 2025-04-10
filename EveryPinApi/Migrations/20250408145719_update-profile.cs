using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EveryPinApi.Migrations
{
    /// <inheritdoc />
    public partial class updateprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ae0206f-f4c1-482e-ac34-0e9e7f36b134");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82c7a8b4-62ac-43d3-b33c-e9deb502968a");

            migrationBuilder.RenameColumn(
                name: "ProfileTag",
                table: "Profiles",
                newName: "ProfileDisplayId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9b511dda-af63-4e0d-b3cb-971942c13291", null, "Administrator", "ADMINISTRATOR" },
                    { "a49c2ee0-ed25-4c23-b55a-bd84c73cdb72", null, "NormalUser", "NORMALUSER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b511dda-af63-4e0d-b3cb-971942c13291");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a49c2ee0-ed25-4c23-b55a-bd84c73cdb72");

            migrationBuilder.RenameColumn(
                name: "ProfileDisplayId",
                table: "Profiles",
                newName: "ProfileTag");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7ae0206f-f4c1-482e-ac34-0e9e7f36b134", null, "Administrator", "ADMINISTRATOR" },
                    { "82c7a8b4-62ac-43d3-b33c-e9deb502968a", null, "NormalUser", "NORMALUSER" }
                });
        }
    }
}
