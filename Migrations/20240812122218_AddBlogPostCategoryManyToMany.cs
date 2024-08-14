using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogPostCategoryManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BlogPostId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BlogPostId",
                table: "Categories",
                column: "BlogPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_BlogPosts_BlogPostId",
                table: "Categories",
                column: "BlogPostId",
                principalTable: "BlogPosts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_BlogPosts_BlogPostId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_BlogPostId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "BlogPostId",
                table: "Categories");
        }
    }
}
