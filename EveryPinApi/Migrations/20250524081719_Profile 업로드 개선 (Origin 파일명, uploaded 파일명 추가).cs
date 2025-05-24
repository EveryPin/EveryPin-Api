using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EveryPinApi.Migrations
{
    /// <inheritdoc />
    public partial class Profile업로드개선Origin파일명uploaded파일명추가 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b511dda-af63-4e0d-b3cb-971942c13291");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a49c2ee0-ed25-4c23-b55a-bd84c73cdb72");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileName",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "OriginPhotoFileName",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadedPhotoFileName",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoFileName",
                table: "PostPhotos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "184cea9e-779c-470b-8494-5f8a515898e2", null, "Administrator", "ADMINISTRATOR" },
                    { "b706669d-62e2-4d7b-8fab-1b8ead2fcadd", null, "NormalUser", "NORMALUSER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "184cea9e-779c-470b-8494-5f8a515898e2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b706669d-62e2-4d7b-8fab-1b8ead2fcadd");

            migrationBuilder.DropColumn(
                name: "OriginPhotoFileName",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "UploadedPhotoFileName",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PhotoFileName",
                table: "PostPhotos");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileName",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9b511dda-af63-4e0d-b3cb-971942c13291", null, "Administrator", "ADMINISTRATOR" },
                    { "a49c2ee0-ed25-4c23-b55a-bd84c73cdb72", null, "NormalUser", "NORMALUSER" }
                });
        }
    }
}
