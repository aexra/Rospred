﻿// <auto-generated />
using Backend.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Web.Migrations.SDGDB
{
    [DbContext(typeof(SDGDBContext))]
    [Migration("20240607150421_SDGCreate")]
    partial class SDGCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("Backend.Web.Data.SDGTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SDG")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SDGTables");
                });
#pragma warning restore 612, 618
        }
    }
}
