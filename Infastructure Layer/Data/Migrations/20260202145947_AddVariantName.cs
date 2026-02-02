using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure_Layer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVariantName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VariantName",
                table: "ProductVariants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VariantName",
                table: "ProductVariants");
        }
    }
}
