﻿// <auto-generated />
using Mango.Services.ProductAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Mango.Services.ProductAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230102183106_SeedProducts")]
    partial class SeedProducts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Mango.Services.ProductAPI.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("ProductId");

                    b.ToTable("Products");

                    b.HasComment("A product to sell on the website");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryName = "Appetizer",
                            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            ImageUrl = "https://cdn.pixabay.com/photo/2022/05/07/11/37/samosa-7180078_960_720.jpg",
                            Name = "Samosa",
                            Price = 15.0
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryName = "Appetizer",
                            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            ImageUrl = "https://cdn.pixabay.com/photo/2021/07/02/05/09/paneer-tikka-6380891_960_720.jpg",
                            Name = "Paneer Tikka",
                            Price = 13.99
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryName = "Dessert",
                            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            ImageUrl = "https://cdn.pixabay.com/photo/2015/09/09/19/49/dessert-932884_960_720.jpg",
                            Name = "Sweet Pie",
                            Price = 10.99
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryName = "Entree",
                            Description = "Praesent scelerisque, mi sed ultrices condimentum, lacus ipsum viverra massa, in lobortis sapien eros in arcu. Quisque vel lacus ac magna vehicula sagittis ut non lacus.<br/>Sed volutpat tellus lorem, lacinia tincidunt tellus varius nec. Vestibulum arcu turpis, facilisis sed ligula ac, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            ImageUrl = "https://cdn.pixabay.com/photo/2014/06/18/16/02/pav-bhaji-371377_960_720.jpg",
                            Name = "Pav Bhaji",
                            Price = 15.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
