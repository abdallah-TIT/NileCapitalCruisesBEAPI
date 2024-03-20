using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NileCapitalCruisesBEAPI.Models;

public partial class NileCapitalCruisesBeopdbContext : DbContext
{
    public NileCapitalCruisesBeopdbContext()
    {
    }

    public NileCapitalCruisesBeopdbContext(DbContextOptions<NileCapitalCruisesBeopdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<TblCountriesNationality> TblCountriesNationalities { get; set; }

    public virtual DbSet<TblCountry> TblCountries { get; set; }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<VwOrder> VwOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=5.77.32.134;Database=NileCapitalCruisesBEOPDB;User Id=titbBEdbOPUsr2024; Password=%i87yBn926Q4dt8^n9;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("titbBEdbOPUsr2024");

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("PK_PaymentStatus");

            entity.ToTable("OrderStatus", "dbo");

            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");
            entity.Property(e => e.OrderStatus1)
                .HasMaxLength(250)
                .HasColumnName("OrderStatus");
        });

        modelBuilder.Entity<TblCountriesNationality>(entity =>
        {
            entity.HasKey(e => e.NationalityId);

            entity.ToTable("tbl_Countries_Nationalities", "dbo");

            entity.Property(e => e.NationalityId).HasColumnName("NationalityID");
            entity.Property(e => e.IsVisa).HasDefaultValue(false);
            entity.Property(e => e.NationalityName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId);

            entity.ToTable("tbl_Countries", "dbo");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CountryCode).HasMaxLength(250);
            entity.Property(e => e.CountryName).HasMaxLength(250);
            entity.Property(e => e.CountryNameAr)
                .HasMaxLength(250)
                .HasColumnName("CountryNameAR");
            entity.Property(e => e.CountryStatus).HasDefaultValue(true);
            entity.Property(e => e.IsVisa).HasDefaultValue(false);
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("tbl_Orders", "dbo");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.CustomerEmail).HasMaxLength(250);
            entity.Property(e => e.CustomerFirstName).HasMaxLength(250);
            entity.Property(e => e.CustomerLastName).HasMaxLength(250);
            entity.Property(e => e.CustomerNationalityId).HasColumnName("CustomerNationalityID");
            entity.Property(e => e.CustomerPhone).HasMaxLength(250);
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");
            entity.Property(e => e.OrderConfirmationNumber).HasMaxLength(250);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderSpecialRequest).HasColumnType("ntext");
            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");
            entity.Property(e => e.PmcardBrand)
                .HasMaxLength(250)
                .HasColumnName("PMcardBrand");
            entity.Property(e => e.PmcardDataToken)
                .HasMaxLength(250)
                .HasColumnName("PMcardDataToken");
            entity.Property(e => e.Pmcurrency)
                .HasMaxLength(250)
                .HasColumnName("PMcurrency");
            entity.Property(e => e.PmmaskedCard)
                .HasMaxLength(250)
                .HasColumnName("PMmaskedCard");
            entity.Property(e => e.PmmerchantOrderId)
                .HasMaxLength(250)
                .HasColumnName("PMmerchantOrderId");
            entity.Property(e => e.Pmmode)
                .HasMaxLength(250)
                .HasColumnName("PMmode");
            entity.Property(e => e.PmorderId)
                .HasMaxLength(250)
                .HasColumnName("PMorderId");
            entity.Property(e => e.PmorderReference)
                .HasMaxLength(250)
                .HasColumnName("PMorderReference");
            entity.Property(e => e.PmpaymentStatus)
                .HasMaxLength(250)
                .HasColumnName("PMpaymentStatus");
            entity.Property(e => e.Pmsignature)
                .HasMaxLength(250)
                .HasColumnName("PMsignature");
            entity.Property(e => e.PmtransactionId)
                .HasMaxLength(250)
                .HasColumnName("PMtransactionId");
            entity.Property(e => e.PriceAdultRate).HasColumnName("Price_Adult_Rate");
            entity.Property(e => e.PriceChildRate).HasColumnName("Price_Child_Rate");
        });

        modelBuilder.Entity<VwOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_order", "dbo");

            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CustomerEmail).HasMaxLength(250);
            entity.Property(e => e.CustomerFirstName).HasMaxLength(250);
            entity.Property(e => e.CustomerLastName).HasMaxLength(250);
            entity.Property(e => e.CustomerPhone).HasMaxLength(250);
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.NationalityName).HasMaxLength(250);
            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");
            entity.Property(e => e.OrderConfirmationNumber).HasMaxLength(250);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderSpecialRequest).HasColumnType("ntext");
            entity.Property(e => e.OrderStatus).HasMaxLength(250);
            entity.Property(e => e.PmcardBrand)
                .HasMaxLength(250)
                .HasColumnName("PMcardBrand");
            entity.Property(e => e.PmcardDataToken)
                .HasMaxLength(250)
                .HasColumnName("PMcardDataToken");
            entity.Property(e => e.Pmcurrency)
                .HasMaxLength(250)
                .HasColumnName("PMcurrency");
            entity.Property(e => e.PmmaskedCard)
                .HasMaxLength(250)
                .HasColumnName("PMmaskedCard");
            entity.Property(e => e.PmmerchantOrderId)
                .HasMaxLength(250)
                .HasColumnName("PMmerchantOrderId");
            entity.Property(e => e.Pmmode)
                .HasMaxLength(250)
                .HasColumnName("PMmode");
            entity.Property(e => e.PmorderId)
                .HasMaxLength(250)
                .HasColumnName("PMorderId");
            entity.Property(e => e.PmorderReference)
                .HasMaxLength(250)
                .HasColumnName("PMorderReference");
            entity.Property(e => e.PmpaymentStatus)
                .HasMaxLength(250)
                .HasColumnName("PMpaymentStatus");
            entity.Property(e => e.Pmsignature)
                .HasMaxLength(250)
                .HasColumnName("PMsignature");
            entity.Property(e => e.PmtransactionId)
                .HasMaxLength(250)
                .HasColumnName("PMtransactionId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
