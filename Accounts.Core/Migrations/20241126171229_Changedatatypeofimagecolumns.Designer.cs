﻿// <auto-generated />
using System;
using Accounts.Core.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Accounts.Core.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241126171229_Changedatatypeofimagecolumns")]
    partial class Changedatatypeofimagecolumns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Accounts.Core.Models.AmountReceived", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("CardAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CardNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("CashAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("FromCreditCard")
                        .HasColumnType("bit");

                    b.Property<bool>("FromDebitCard")
                        .HasColumnType("bit");

                    b.Property<string>("NameOnCard")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("AmountReceived");
                });

            modelBuilder.Entity("Accounts.Core.Models.BrokerMaster", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("BrokerMaster");
                });

            modelBuilder.Entity("Accounts.Core.Models.Customer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AadharImageFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AadharImageFrontData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AadharNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AadhbarImageBackData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PanImageData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PanImageFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PanNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SignatureFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SignatureImageData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("CustomerMaster");
                });

            modelBuilder.Entity("Accounts.Core.Models.ItemMaster", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("CGSTRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("IGSTRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("ItemId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SGSTRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ItemMaster");
                });

            modelBuilder.Entity("Accounts.Core.Models.PurchaseDetails", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("PurchaseMasterId")
                        .HasColumnType("bigint");

                    b.Property<long>("StockId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("PurchaseDetails");
                });

            modelBuilder.Entity("Accounts.Core.Models.PurchaseMaster", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("BrokerId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("PurchaseSlipNo")
                        .HasColumnType("bigint");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("PurchaseMaster");
                });

            modelBuilder.Entity("Accounts.Core.Models.SalesDetails", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("AmountReceivedId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("CarratQty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("GstAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("StockId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AmountReceivedId");

                    b.ToTable("SalesDetails");
                });

            modelBuilder.Entity("Accounts.Core.Models.SalesMaster", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SalesMasters");
                });

            modelBuilder.Entity("Accounts.Core.Models.Stock", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("CGSTRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("IGSTRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SGSTRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("Accounts.Core.Models.UserMaster", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long?>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsAgent")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("UserMaster");
                });

            modelBuilder.Entity("Accounts.Core.Models.SalesDetails", b =>
                {
                    b.HasOne("Accounts.Core.Models.AmountReceived", "AmountReceived")
                        .WithMany()
                        .HasForeignKey("AmountReceivedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AmountReceived");
                });
#pragma warning restore 612, 618
        }
    }
}