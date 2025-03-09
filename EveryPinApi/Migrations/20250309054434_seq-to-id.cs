using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EveryPinApi.Migrations
{
    /// <inheritdoc />
    public partial class seqtoid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostSeq",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_PostSeq",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostPhotos_Posts_PostSeq",
                table: "PostPhotos");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9e1a43d-5c3b-454d-bda3-7d9f2851a536");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e62a8f80-8483-4039-8431-e9b750db9bd4");

            migrationBuilder.RenameColumn(
                name: "ProfileSeq",
                table: "Profiles",
                newName: "ProfileId");

            migrationBuilder.RenameColumn(
                name: "PostSeq",
                table: "Posts",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "PostSeq",
                table: "PostPhotos",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "PostPhotoSeq",
                table: "PostPhotos",
                newName: "PostPhotoId");

            migrationBuilder.RenameIndex(
                name: "IX_PostPhotos_PostSeq",
                table: "PostPhotos",
                newName: "IX_PostPhotos_PostId");

            migrationBuilder.RenameColumn(
                name: "PostSeq",
                table: "Likes",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "LikeSeq",
                table: "Likes",
                newName: "LikeId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_PostSeq",
                table: "Likes",
                newName: "IX_Likes_PostId");

            migrationBuilder.RenameColumn(
                name: "FollowSeq",
                table: "Follows",
                newName: "FollowId");

            migrationBuilder.RenameColumn(
                name: "PostSeq",
                table: "Comments",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "CommentSeq",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostSeq",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7ae0206f-f4c1-482e-ac34-0e9e7f36b134", null, "Administrator", "ADMINISTRATOR" },
                    { "82c7a8b4-62ac-43d3-b33c-e9deb502968a", null, "NormalUser", "NORMALUSER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostPhotos_Posts_PostId",
                table: "PostPhotos",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostPhotos_Posts_PostId",
                table: "PostPhotos");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ae0206f-f4c1-482e-ac34-0e9e7f36b134");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82c7a8b4-62ac-43d3-b33c-e9deb502968a");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Profiles",
                newName: "ProfileSeq");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Posts",
                newName: "PostSeq");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "PostPhotos",
                newName: "PostSeq");

            migrationBuilder.RenameColumn(
                name: "PostPhotoId",
                table: "PostPhotos",
                newName: "PostPhotoSeq");

            migrationBuilder.RenameIndex(
                name: "IX_PostPhotos_PostId",
                table: "PostPhotos",
                newName: "IX_PostPhotos_PostSeq");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Likes",
                newName: "PostSeq");

            migrationBuilder.RenameColumn(
                name: "LikeId",
                table: "Likes",
                newName: "LikeSeq");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_PostId",
                table: "Likes",
                newName: "IX_Likes_PostSeq");

            migrationBuilder.RenameColumn(
                name: "FollowId",
                table: "Follows",
                newName: "FollowSeq");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Comments",
                newName: "PostSeq");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "CommentSeq");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                newName: "IX_Comments_PostSeq");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c9e1a43d-5c3b-454d-bda3-7d9f2851a536", null, "NormalUser", "NORMALUSER" },
                    { "e62a8f80-8483-4039-8431-e9b750db9bd4", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostSeq",
                table: "Comments",
                column: "PostSeq",
                principalTable: "Posts",
                principalColumn: "PostSeq");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostSeq",
                table: "Likes",
                column: "PostSeq",
                principalTable: "Posts",
                principalColumn: "PostSeq");

            migrationBuilder.AddForeignKey(
                name: "FK_PostPhotos_Posts_PostSeq",
                table: "PostPhotos",
                column: "PostSeq",
                principalTable: "Posts",
                principalColumn: "PostSeq");
        }
    }
}
