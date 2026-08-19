using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShopApp.Data;

#nullable disable

namespace ShopApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("ShopApp.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "CPUs",
                            Name = "معالجات"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "GPUs",
                            Name = "كروت شاشة"
                        });
                });

            modelBuilder.Entity("ShopApp.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            IsAvailable = true,
                            Name = "Ryzen 7 7800X3D",
                            Price = 1750m,
                            Stock = 12
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            IsAvailable = true,
                            Name = "Intel Core i5-14600K",
                            Price = 1250m,
                            Stock = 8
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 2,
                            IsAvailable = true,
                            Name = "RTX 4070 Super",
                            Price = 2600m,
                            Stock = 5
                        });
                });

            modelBuilder.Entity("ShopApp.Models.Product", b =>
                {
                    b.HasOne("ShopApp.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ShopApp.Models.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
