﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(VoucherContext))]
    [Migration("20220429112223_PlayerColorChangedToNullableString")]
    partial class PlayerColorChangedToNullableString
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Domain.Category", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Core.Domain.Company", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime>("ContactDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int?>("NumberOfEmployees")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Core.Domain.CompanyPlayer", b =>
                {
                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<int?>("Id")
                        .HasColumnType("integer");

                    b.HasKey("CompanyId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("CompanyPlayer");
                });

            modelBuilder.Entity("Core.Domain.Discount", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<int>("DiscountType")
                        .HasColumnType("integer");

                    b.Property<float?>("DiscountValue")
                        .HasColumnType("real");

                    b.Property<int?>("FinalPrice")
                        .HasColumnType("integer");

                    b.Property<int?>("InitialPrice")
                        .HasColumnType("integer");

                    b.Property<string>("LinkTermsAndConditions")
                        .HasColumnType("text");

                    b.Property<int?>("NumberOfUsagePerCompany")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfUsagePerUser")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("PlayerId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<string>("UnityOfMeasurement")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("PlayerId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("Core.Domain.Location", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Core.Domain.Player", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("AppStoreLink")
                        .HasColumnType("text");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Color")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Yellow");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LinkDescription")
                        .HasColumnType("text");

                    b.Property<string>("PlayStoreLink")
                        .HasColumnType("text");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Core.Domain.PlayerContact", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PhoneNumber")
                        .HasColumnType("integer");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerContacts");
                });

            modelBuilder.Entity("Core.Domain.PlayerLocation", b =>
                {
                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<int?>("Id")
                        .HasColumnType("integer");

                    b.HasKey("PlayerId", "LocationId");

                    b.HasIndex("LocationId");

                    b.ToTable("PlayerLocation");
                });

            modelBuilder.Entity("Core.Domain.Report", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Core.Domain.Voucher", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("DiscountId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DiscountId");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("Core.Domain.CompanyPlayer", b =>
                {
                    b.HasOne("Core.Domain.Company", "Company")
                        .WithMany("CompanyPlayers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Player", "Player")
                        .WithMany("CompanyPlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Core.Domain.Discount", b =>
                {
                    b.HasOne("Core.Domain.Player", "Player")
                        .WithMany("Discounts")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Core.Domain.ValidityPeriod", "ValidityPeriod", b1 =>
                        {
                            b1.Property<int>("DiscountId")
                                .HasColumnType("integer");

                            b1.Property<DateTime>("EndDate")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<int?>("Id")
                                .HasColumnType("integer");

                            b1.Property<DateTime>("StartDate")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("DiscountId");

                            b1.ToTable("Discounts");

                            b1.WithOwner()
                                .HasForeignKey("DiscountId");
                        });

                    b.Navigation("Player");

                    b.Navigation("ValidityPeriod")
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Player", b =>
                {
                    b.HasOne("Core.Domain.Category", "Category")
                        .WithMany("Players")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Core.Domain.PlayerContact", b =>
                {
                    b.HasOne("Core.Domain.Player", "Player")
                        .WithMany("PlayerContacts")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Core.Domain.PlayerLocation", b =>
                {
                    b.HasOne("Core.Domain.Location", "Location")
                        .WithMany("PlayerLocations")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Player", "Player")
                        .WithMany("PlayerLocations")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.HasOne("Core.Domain.Company", "Company")
                        .WithMany("Workers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Core.Domain.Voucher", b =>
                {
                    b.HasOne("Core.Domain.Category", "Category")
                        .WithMany("Vouchers")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Discount", "Discount")
                        .WithMany("Vouchers")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Discount");
                });

            modelBuilder.Entity("Core.Domain.Category", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("Core.Domain.Company", b =>
                {
                    b.Navigation("CompanyPlayers");

                    b.Navigation("Workers");
                });

            modelBuilder.Entity("Core.Domain.Discount", b =>
                {
                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("Core.Domain.Location", b =>
                {
                    b.Navigation("PlayerLocations");
                });

            modelBuilder.Entity("Core.Domain.Player", b =>
                {
                    b.Navigation("CompanyPlayers");

                    b.Navigation("Discounts");

                    b.Navigation("PlayerContacts");

                    b.Navigation("PlayerLocations");
                });
#pragma warning restore 612, 618
        }
    }
}
