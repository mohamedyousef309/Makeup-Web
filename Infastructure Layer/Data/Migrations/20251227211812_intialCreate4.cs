using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure_Layer.Data.Migrations
{
    /// <inheritdoc />
    public partial class intialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "productStock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productStock",
                table: "Products");
        }
    }
}
