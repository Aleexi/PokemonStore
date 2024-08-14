using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSellerToPokemonEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Seller",
                table: "Pokemons",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seller",
                table: "Pokemons");
        }
    }
}
