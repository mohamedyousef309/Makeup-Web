using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infastructure_Layer.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinalF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VariantAttributeValue",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "VariantAttributeValue");
        }
    }
}
