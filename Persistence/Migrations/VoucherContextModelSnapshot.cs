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

            modelBuilder.Entity("Core.Domain.Batch", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<DateTimeOffset?>("UploadTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Batches");
                });

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

                    b.Property<DateTimeOffset>("ContactDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Core.Domain.CompanyDiscount", b =>
                {
                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("integer");

                    b.HasKey("CompanyId", "DiscountId");

                    b.HasIndex("DiscountId");

                    b.ToTable("CompanyDiscount");
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

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<string>("UnityOfMeasurement")
                        .HasColumnType("text");

                    b.HasKey("Id");

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

                    b.Property<int?>("BatchId")
                        .HasColumnType("integer");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("integer");

                    b.Property<bool?>("IsAssignedToCompany")
                        .HasColumnType("boolean");

                    b.Property<bool?>("IsAssignedToUser")
                        .HasColumnType("boolean");

                    b.Property<int?>("UsageLimit")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BatchId");

                    b.HasIndex("DiscountId");

                    b.ToTable("DiscountCodes");
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

                    b.Property<DateTimeOffset?>("ExpireDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InviteCode")
                        .HasColumnType("text");

                    b.Property<int?>("JoinRequestId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("StartDate")
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

            modelBuilder.Entity("Core.Domain.Offer", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int?>("Availability")
                        .HasColumnType("integer");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<int?>("DiscountCodeId")
                        .HasColumnType("integer");

                    b.Property<double?>("Price")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("DiscountCodeId");

                    b.ToTable("Offers");
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

            modelBuilder.Entity("Core.Domain.Purchase", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int?>("DiscountCodeId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("PurchaseTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DiscountCodeId");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases");
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

            modelBuilder.Entity("Core.Domain.CompanyDiscount", b =>
                {
                    b.HasOne("Core.Domain.Company", "Company")
                        .WithMany("CompanyDiscounts")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Discount", "Discount")
                        .WithMany("CompanyDiscounts")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Discount");
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

                            b1.Property<DateTimeOffset>("EndDate")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<int?>("Id")
                                .HasColumnType("integer");

                            b1.Property<DateTimeOffset>("StartDate")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("DiscountId");

                            b1.ToTable("Discounts");

                            b1.WithOwner()
                                .HasForeignKey("DiscountId");
                        });

                    b.Navigation("DiscountType");

                    b.Navigation("Player");

                    b.Navigation("ValidityPeriod");
                });

            modelBuilder.Entity("Core.Domain.DiscountCode", b =>
                {
                    b.HasOne("Core.Domain.Batch", "Batch")
                        .WithMany("DiscountCodes")
                        .HasForeignKey("BatchId");

                    b.HasOne("Core.Domain.Discount", "Discount")
                        .WithMany("DiscountCodes")
                        .HasForeignKey("DiscountId");

                    b.Navigation("Batch");

                    b.Navigation("Discount");
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

            modelBuilder.Entity("Core.Domain.Offer", b =>
                {
                    b.HasOne("Core.Domain.Company", "Company")
                        .WithMany("Offers")
                        .HasForeignKey("CompanyId");

                    b.HasOne("Core.Domain.DiscountCode", "DiscountCode")
                        .WithMany("Offers")
                        .HasForeignKey("DiscountCodeId");

                    b.Navigation("Company");

                    b.Navigation("DiscountCode");
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

            modelBuilder.Entity("Core.Domain.Purchase", b =>
                {
                    b.HasOne("Core.Domain.DiscountCode", "DiscountCode")
                        .WithMany("Purchases")
                        .HasForeignKey("DiscountCodeId");

                    b.HasOne("Core.Domain.User", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("UserId");

                    b.Navigation("DiscountCode");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.HasOne("Core.Domain.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Core.Domain.Batch", b =>
                {
                    b.Navigation("DiscountCodes");
                });

            modelBuilder.Entity("Core.Domain.Category", b =>
                {
                    b.Navigation("Image");

                    b.Navigation("PlayerCategories");
                });

            modelBuilder.Entity("Core.Domain.Company", b =>
                {
                    b.Navigation("CompanyDiscounts");

                    b.Navigation("CompanyPlayers");

                    b.Navigation("InvitationCodes");

                    b.Navigation("Offers");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Core.Domain.Discount", b =>
                {
                    b.Navigation("CompanyDiscounts");

                    b.Navigation("DiscountCodes");
                });

            modelBuilder.Entity("Core.Domain.DiscountCode", b =>
                {
                    b.Navigation("Offers");

                    b.Navigation("Purchases");
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

            modelBuilder.Entity("Core.Domain.User", b =>
                {
                    b.Navigation("JoinRequest");

                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
