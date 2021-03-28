﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PucMinas.Services.Charity.Infrastructure.Entity;

namespace PucMinas.Services.Charity.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Approvals.Approval", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<Guid>("CharitableEntityId");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date")
                        .HasColumnType("datetime")
                        .HasMaxLength(10);

                    b.Property<string>("Detail")
                        .HasColumnName("Detail")
                        .HasMaxLength(250);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnName("Message")
                        .HasMaxLength(250);

                    b.Property<int>("Status")
                        .HasColumnName("Status");

                    b.HasKey("Id");

                    b.HasIndex("CharitableEntityId");

                    b.ToTable("TbApproval");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<string>("Approver")
                        .HasColumnName("Approver")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("ApproverData")
                        .HasColumnName("ApproverData")
                        .HasColumnType("datetime");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasColumnName("CNPJ")
                        .HasMaxLength(20);

                    b.Property<bool>("IsActive")
                        .HasColumnName("Activated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(250);

                    b.Property<string>("RepresentantName")
                        .IsRequired()
                        .HasColumnName("RepresentantName")
                        .HasMaxLength(150);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("Status");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("TbCharitableEntity");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableInformation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<string>("About")
                        .IsRequired()
                        .HasColumnName("About")
                        .HasMaxLength(500);

                    b.Property<Guid>("CharitableEntityId");

                    b.Property<string>("Email")
                        .HasColumnName("Email");

                    b.Property<string>("Goal")
                        .IsRequired()
                        .HasColumnName("Goal")
                        .HasMaxLength(500);

                    b.Property<string>("ManagerDescription")
                        .IsRequired()
                        .HasColumnName("ManagerDescription")
                        .HasMaxLength(500);

                    b.Property<string>("Mission")
                        .IsRequired()
                        .HasColumnName("Mission")
                        .HasMaxLength(500);

                    b.Property<string>("Nickname")
                        .HasColumnName("Nickname")
                        .HasMaxLength(250);

                    b.Property<string>("PicturePath")
                        .HasColumnName("PicturePath");

                    b.Property<string>("PictureUrl")
                        .HasColumnName("PictureUrl");

                    b.Property<string>("SiteUrl")
                        .HasColumnName("SiteUrl");

                    b.Property<string>("TransparencyDescription")
                        .IsRequired()
                        .HasColumnName("TransparencyDescription")
                        .HasMaxLength(500);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Values")
                        .HasMaxLength(500);

                    b.Property<string>("Vision")
                        .IsRequired()
                        .HasColumnName("Vision")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.HasIndex("CharitableEntityId")
                        .IsUnique();

                    b.ToTable("TbCharitableInformation");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableInformationItem", b =>
                {
                    b.Property<Guid>("CharitableInformationId")
                        .HasColumnName("CharitableInformationId");

                    b.Property<Guid>("ItemId")
                        .HasColumnName("ItemId");

                    b.HasKey("CharitableInformationId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("TbCharitableInformationItem");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.Donation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<bool>("Canceled")
                        .HasColumnName("Canceled");

                    b.Property<Guid>("CharitableEntityId");

                    b.Property<bool>("Completed")
                        .HasColumnName("Completed");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date")
                        .HasColumnType("datetime");

                    b.Property<double>("Total")
                        .HasColumnName("Total");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CharitableEntityId");

                    b.HasIndex("UserId");

                    b.ToTable("TbDonation");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.DonationItem", b =>
                {
                    b.Property<Guid>("DonationId")
                        .HasColumnName("DonationId");

                    b.Property<Guid>("ItemId")
                        .HasColumnName("ItemId");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int>("Quantity")
                        .HasColumnName("Quantity");

                    b.HasKey("DonationId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("TbDonationItem");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.DonorPF", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<DateTime>("Birthday")
                        .HasColumnName("Birthday")
                        .HasColumnType("datetime");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnName("CPF")
                        .HasMaxLength(20);

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnName("Genre")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(250);

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("TbDonorPF");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.DonorPJ", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnName("CNPJ")
                        .HasMaxLength(20);

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnName("CompanyName")
                        .HasMaxLength(250);

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnName("ContactName")
                        .HasMaxLength(250);

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("TbDonorPJ");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("TbGroup");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Description")
                        .HasMaxLength(500);

                    b.Property<Guid>("GroupId");

                    b.Property<string>("ImagePath")
                        .HasColumnName("ImagePath");

                    b.Property<string>("ImageUrl")
                        .HasColumnName("ImageUrl");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(250);

                    b.Property<double>("Price")
                        .HasColumnName("Price");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("TbItem");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Login.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("TbRole");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Login.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("Id");

                    b.Property<Guid>("ActivationCode")
                        .HasColumnName("ActivationCode");

                    b.Property<bool>("IsActive")
                        .HasColumnName("Activated");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnName("Login")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("Password");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnName("Type");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("TbUser");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Login.UserRole", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnName("RoleId");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("TbUserRoles");
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Approvals.Approval", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableEntity", "CharitableEntity")
                        .WithMany("Approvals")
                        .HasForeignKey("CharitableEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableEntity", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Login.User", "User")
                        .WithOne("CharitableEntity")
                        .HasForeignKey("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("PucMinas.Services.Charity.Domain.ValueObject.ContactNumber", "ContactNumber", b1 =>
                        {
                            b1.Property<Guid>("CharitableEntityId");

                            b1.Property<string>("CellPhone")
                                .IsRequired()
                                .HasColumnName("CellPhone");

                            b1.Property<string>("Telephone")
                                .IsRequired()
                                .HasColumnName("Telephone");

                            b1.HasKey("CharitableEntityId");

                            b1.ToTable("TbCharitableEntity");

                            b1.HasOne("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableEntity")
                                .WithOne("ContactNumber")
                                .HasForeignKey("PucMinas.Services.Charity.Domain.ValueObject.ContactNumber", "CharitableEntityId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("PucMinas.Services.Charity.Domain.ValueObject.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("CharitableEntityId");

                            b1.Property<string>("AddressName")
                                .IsRequired()
                                .HasColumnName("AddressName")
                                .HasMaxLength(200);

                            b1.Property<string>("CEP")
                                .IsRequired()
                                .HasColumnName("CEP")
                                .HasMaxLength(25);

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnName("City")
                                .HasMaxLength(200);

                            b1.Property<string>("Complement")
                                .HasColumnName("Complement")
                                .HasMaxLength(150);

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnName("Country")
                                .HasMaxLength(200);

                            b1.Property<string>("District")
                                .IsRequired()
                                .HasColumnName("District")
                                .HasMaxLength(200);

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnName("Number")
                                .HasMaxLength(15);

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnName("State")
                                .HasMaxLength(200);

                            b1.HasKey("CharitableEntityId");

                            b1.ToTable("TbCharitableEntity");

                            b1.HasOne("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableEntity")
                                .WithOne("Address")
                                .HasForeignKey("PucMinas.Services.Charity.Domain.ValueObject.Address", "CharitableEntityId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableInformation", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableEntity", "CharitableEntity")
                        .WithOne("CharitableInformation")
                        .HasForeignKey("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableInformation", "CharitableEntityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("PucMinas.Services.Charity.Domain.ValueObject.Image", "Photo01", b1 =>
                        {
                            b1.Property<Guid>("CharitableInformationId");

                            b1.Property<string>("ImagePath")
                                .IsRequired()
                                .HasColumnName("ImagePath01");

                            b1.Property<string>("ImageUrl")
                                .IsRequired()
                                .HasColumnName("ImageUrl01");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasColumnName("Title01");

                            b1.HasKey("CharitableInformationId");

                            b1.ToTable("TbCharitableInformation");

                            b1.HasOne("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableInformation")
                                .WithOne("Photo01")
                                .HasForeignKey("PucMinas.Services.Charity.Domain.ValueObject.Image", "CharitableInformationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("PucMinas.Services.Charity.Domain.ValueObject.Image", "Photo02", b1 =>
                        {
                            b1.Property<Guid>("CharitableInformationId");

                            b1.Property<string>("ImagePath")
                                .IsRequired()
                                .HasColumnName("ImagePath02");

                            b1.Property<string>("ImageUrl")
                                .IsRequired()
                                .HasColumnName("ImageUrl02");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasColumnName("Title02");

                            b1.HasKey("CharitableInformationId");

                            b1.ToTable("TbCharitableInformation");

                            b1.HasOne("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableInformation")
                                .WithOne("Photo02")
                                .HasForeignKey("PucMinas.Services.Charity.Domain.ValueObject.Image", "CharitableInformationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableInformationItem", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableInformation", "CharitableInformation")
                        .WithMany("CharitableInformationItem")
                        .HasForeignKey("CharitableInformationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Donor.Item", "Item")
                        .WithMany("CharitableInformationItem")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.Donation", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Charitable.CharitableEntity", "CharitableEntity")
                        .WithMany("Donations")
                        .HasForeignKey("CharitableEntityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Login.User", "User")
                        .WithMany("Donations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.DonationItem", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Donor.Donation", "Donation")
                        .WithMany("DonationItem")
                        .HasForeignKey("DonationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Donor.Item", "Item")
                        .WithMany("DonationItem")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.DonorPF", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Login.User", "User")
                        .WithOne("DonorPF")
                        .HasForeignKey("PucMinas.Services.Charity.Domain.Models.Donor.DonorPF", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("PucMinas.Services.Charity.Domain.ValueObject.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("DonorPFId");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnName("City")
                                .HasMaxLength(200);

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnName("Country")
                                .HasMaxLength(200);

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnName("State")
                                .HasMaxLength(200);

                            b1.HasKey("DonorPFId");

                            b1.ToTable("TbDonorPF");

                            b1.HasOne("PucMinas.Services.Charity.Domain.Models.Donor.DonorPF")
                                .WithOne("Address")
                                .HasForeignKey("PucMinas.Services.Charity.Domain.ValueObject.Address", "DonorPFId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.DonorPJ", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Login.User", "User")
                        .WithOne("DonorPJ")
                        .HasForeignKey("PucMinas.Services.Charity.Domain.Models.Donor.DonorPJ", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("PucMinas.Services.Charity.Domain.ValueObject.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("DonorPJId");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnName("City")
                                .HasMaxLength(200);

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnName("Country")
                                .HasMaxLength(200);

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnName("State")
                                .HasMaxLength(200);

                            b1.HasKey("DonorPJId");

                            b1.ToTable("TbDonorPJ");

                            b1.HasOne("PucMinas.Services.Charity.Domain.Models.Donor.DonorPJ")
                                .WithOne("Address")
                                .HasForeignKey("PucMinas.Services.Charity.Domain.ValueObject.Address", "DonorPJId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Donor.Item", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Donor.Group", "Group")
                        .WithMany("Items")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PucMinas.Services.Charity.Domain.Models.Login.UserRole", b =>
                {
                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Login.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PucMinas.Services.Charity.Domain.Models.Login.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
