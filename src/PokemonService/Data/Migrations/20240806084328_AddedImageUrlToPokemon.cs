using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonService.data.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageUrlToPokemon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Pokemons",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Pokemons",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Pokemons");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Pokemons",
                newName: "createdAt");
        }
    }
}
