﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(VoucherContext))]
    partial class VoucherContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int?>("DiscountCodeId")
                        .HasColumnType("integer");

                    b.Property<int?>("DiscountTypeId")
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
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfUsagePerUser")
                        .HasColumnType("integer");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<string>("UnityOfMeasurement")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DiscountCodeId")
                        .IsUnique();

                    b.HasIndex("DiscountTypeId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("Core.Domain.DiscountCode", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<int?>("UnassignedCollectionId")
                        .HasColumnType("integer");

                    b.Property<int?>("UnassignedDiscountCodeCollectionsId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UnassignedDiscountCodeCollectionsId");

                    b.ToTable("DiscountCode");
                });

            modelBuilder.Entity("Core.Domain.DiscountType", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DiscountTypes");
                });

            modelBuilder.Entity("Core.Domain.Image", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<byte[]>("Content")
                        .HasColumnType("bytea");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId")
                        .IsUnique();

                    b.HasIndex("PlayerId")
                        .IsUnique();

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Core.Domain.InvitationCode", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ExpireDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InviteCode")
                        .HasColumnType("text");

                    b.Property<int?>("JoinRequestId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("InvitationCodes");
                });

            modelBuilder.Entity("Core.Domain.JoinRequest", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<bool>("Declined")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int?>("InvitationCodeId")
                        .HasColumnType("integer");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("InvitationCodeId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("JoinRequests");
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

                    b.Property<string>("Color")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Yellow");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<string>("LinkDescription")
                        .HasColumnType("text");

                    b.Property<string>("PlayStoreLink")
                        .HasColumnType("text");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Core.Domain.PlayerCategories", b =>
                {
                    b.Property<int?>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int?>("Id")
                        .HasColumnType("integer");

                    b.HasKey("PlayerId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PlayerCategories");
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

            modelBuilder.Entity("Core.Domain.PlayerDiscountType", b =>
                {
                    b.Property<int?>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<int?>("DiscountTypeId")
                        .HasColumnType("integer");

                    b.HasKey("PlayerId", "DiscountTypeId");

                    b.HasIndex("DiscountTypeId");

                    b.ToTable("PlayerDiscountType");
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

            modelBuilder.Entity("Core.Domain.UnassignedDiscountCodeCollection", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.HasKey("Id");

                    b.ToTable("UnassignedDiscountCodeCollection");
                });

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("IdentityUserId")
                        .IsUnique();

                    b.ToTable("Users");
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
                    b.HasOne("Core.Domain.DiscountCode", "DiscountCode")
                        .WithOne("Discount")
                        .HasForeignKey("Core.Domain.Discount", "DiscountCodeId");

                    b.HasOne("Core.Domain.DiscountType", "DiscountType")
                        .WithMany("Discounts")
                        .HasForeignKey("DiscountTypeId");

                    b.HasOne("Core.Domain.Player", "Player")
                        .WithMany("Discounts")
                        .HasForeignKey("PlayerId");

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

                    b.Navigation("DiscountCode");

                    b.Navigation("DiscountType");

                    b.Navigation("Player");

                    b.Navigation("ValidityPeriod");
                });

            modelBuilder.Entity("Core.Domain.DiscountCode", b =>
                {
                    b.HasOne("Core.Domain.UnassignedDiscountCodeCollection", "UnassignedDiscountCodeCollections")
                        .WithMany("DiscountCodes")
                        .HasForeignKey("UnassignedDiscountCodeCollectionsId");

                    b.Navigation("UnassignedDiscountCodeCollections");
                });

            modelBuilder.Entity("Core.Domain.Image", b =>
                {
                    b.HasOne("Core.Domain.Category", "Category")
                        .WithOne("Image")
                        .HasForeignKey("Core.Domain.Image", "CategoryId");

                    b.HasOne("Core.Domain.Player", "Player")
                        .WithOne("Image")
                        .HasForeignKey("Core.Domain.Image", "PlayerId");

                    b.Navigation("Category");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Core.Domain.InvitationCode", b =>
                {
                    b.HasOne("Core.Domain.Company", "Company")
                        .WithMany("InvitationCodes")
                        .HasForeignKey("CompanyId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Core.Domain.JoinRequest", b =>
                {
                    b.HasOne("Core.Domain.InvitationCode", "InvitationCode")
                        .WithOne("JoinRequest")
                        .HasForeignKey("Core.Domain.JoinRequest", "InvitationCodeId");

                    b.HasOne("Core.Domain.User", "User")
                        .WithOne("JoinRequest")
                        .HasForeignKey("Core.Domain.JoinRequest", "UserId");

                    b.Navigation("InvitationCode");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Core.Domain.PlayerCategories", b =>
                {
                    b.HasOne("Core.Domain.Category", "Category")
                        .WithMany("PlayerCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Player", "Player")
                        .WithMany("PlayerCategories")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Player");
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

            modelBuilder.Entity("Core.Domain.PlayerDiscountType", b =>
                {
                    b.HasOne("Core.Domain.DiscountType", "DiscountType")
                        .WithMany("PlayerDiscountTypes")
                        .HasForeignKey("DiscountTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Player", "Player")
                        .WithMany("PlayerDiscountTypes")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiscountType");

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
                        .WithMany("Users")
                        .HasForeignKey("CompanyId");

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
                    b.Navigation("Image");

                    b.Navigation("PlayerCategories");

                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("Core.Domain.Company", b =>
                {
                    b.Navigation("CompanyPlayers");

                    b.Navigation("InvitationCodes");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Core.Domain.Discount", b =>
                {
                    b.Navigation("Vouchers");
                });

            modelBuilder.Entity("Core.Domain.DiscountCode", b =>
                {
                    b.Navigation("Discount");
                });

            modelBuilder.Entity("Core.Domain.DiscountType", b =>
                {
                    b.Navigation("Discounts");

                    b.Navigation("PlayerDiscountTypes");
                });

            modelBuilder.Entity("Core.Domain.InvitationCode", b =>
                {
                    b.Navigation("JoinRequest");
                });

            modelBuilder.Entity("Core.Domain.Location", b =>
                {
                    b.Navigation("PlayerLocations");
                });

            modelBuilder.Entity("Core.Domain.Player", b =>
                {
                    b.Navigation("CompanyPlayers");

                    b.Navigation("Discounts");

                    b.Navigation("Image");

                    b.Navigation("PlayerCategories");

                    b.Navigation("PlayerContacts");

                    b.Navigation("PlayerDiscountTypes");

                    b.Navigation("PlayerLocations");
                });

            modelBuilder.Entity("Core.Domain.UnassignedDiscountCodeCollection", b =>
                {
                    b.Navigation("DiscountCodes");
                });

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.Navigation("JoinRequest");
                });
#pragma warning restore 612, 618
        }
    }
}
