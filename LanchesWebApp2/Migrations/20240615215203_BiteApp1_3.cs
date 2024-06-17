using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanchesWebApp2.Migrations
{
    /// <inheritdoc />
    public partial class BiteApp1_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IngredienteId",
                table: "Lanches",
                newName: "IngredienteIds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IngredienteIds",
                table: "Lanches",
                newName: "IngredienteId");
        }
    }
}
