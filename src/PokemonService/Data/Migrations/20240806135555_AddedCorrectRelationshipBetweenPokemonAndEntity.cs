﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCorrectRelationshipBetweenPokemonAndEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attacks_Pokemons_PokemonId",
                table: "Attacks");

            migrationBuilder.AlterColumn<Guid>(
                name: "PokemonId",
                table: "Attacks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attacks_Pokemons_PokemonId",
                table: "Attacks",
                column: "PokemonId",
                principalTable: "Pokemons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attacks_Pokemons_PokemonId",
                table: "Attacks");

            migrationBuilder.AlterColumn<Guid>(
                name: "PokemonId",
                table: "Attacks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Attacks_Pokemons_PokemonId",
                table: "Attacks",
                column: "PokemonId",
                principalTable: "Pokemons",
                principalColumn: "Id");
        }
    }
}
