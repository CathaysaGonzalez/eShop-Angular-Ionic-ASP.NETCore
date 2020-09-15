using Microsoft.EntityFrameworkCore.Migrations;

namespace BE.Dal.EfStructures.Migrations
{
    public partial class AddedModelNumberModelNameProductImageProductImageLargeAndProductImageThumbToProductsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelName",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelNumber",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImage",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImageLarge",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImageThumb",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModelNumber",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImageLarge",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImageThumb",
                table: "Products");
        }
    }
}
