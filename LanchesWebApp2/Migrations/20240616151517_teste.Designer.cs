﻿// <auto-generated />
using LanchesWebApp2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LanchesWebApp2.Migrations
{
    [DbContext(typeof(DbBitesContext))]
    [Migration("20240616151517_teste")]
    partial class teste
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LanchesWebApp2.Models.Ingrediente", b =>
                {
                    b.Property<int>("IngredienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IngredienteId"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("PrecoUni")
                        .HasColumnType("real");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.HasKey("IngredienteId");

                    b.ToTable("Ingredientes");
                });

            modelBuilder.Entity("LanchesWebApp2.Models.Lanche", b =>
                {
                    b.Property<int>("LancheId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LancheId"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IngredienteIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Preco")
                        .HasColumnType("real");

                    b.HasKey("LancheId");

                    b.ToTable("Lanches");
                });

            modelBuilder.Entity("LanchesWebApp2.Models.LancheIngrediente", b =>
                {
                    b.Property<int>("IngredienteId")
                        .HasColumnType("int");

                    b.Property<int>("LancheId")
                        .HasColumnType("int");

                    b.HasKey("IngredienteId", "LancheId");

                    b.HasIndex("LancheId");

                    b.ToTable("LancheIngredientes");
                });

            modelBuilder.Entity("LanchesWebApp2.Models.LancheIngrediente", b =>
                {
                    b.HasOne("LanchesWebApp2.Models.Ingrediente", "Ingrediente")
                        .WithMany("LancheIngredientes")
                        .HasForeignKey("IngredienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("LanchesWebApp2.Models.Lanche", "Lanche")
                        .WithMany("LancheIngredientes")
                        .HasForeignKey("LancheId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Ingrediente");

                    b.Navigation("Lanche");
                });

            modelBuilder.Entity("LanchesWebApp2.Models.Ingrediente", b =>
                {
                    b.Navigation("LancheIngredientes");
                });

            modelBuilder.Entity("LanchesWebApp2.Models.Lanche", b =>
                {
                    b.Navigation("LancheIngredientes");
                });
#pragma warning restore 612, 618
        }
    }
}
