﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PokemonService;

#nullable disable

namespace PokemonService.Data.Migrations
{
    [DbContext(typeof(PokemonDbContext))]
    [Migration("20240805193812_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PokemonService.Attack", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Damage")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("PokemonId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PokemonId");

                    b.ToTable("Attacks");
                });

            modelBuilder.Entity("PokemonService.Pokemon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("HealthPower")
                        .HasColumnType("integer");

                    b.Property<bool>("Holographic")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("Rarity")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Pokemons");
                });

            modelBuilder.Entity("PokemonService.Attack", b =>
                {
                    b.HasOne("PokemonService.Pokemon", null)
                        .WithMany("Attacks")
                        .HasForeignKey("PokemonId");
                });

            modelBuilder.Entity("PokemonService.Pokemon", b =>
                {
                    b.Navigation("Attacks");
                });
#pragma warning restore 612, 618
        }
    }
}
