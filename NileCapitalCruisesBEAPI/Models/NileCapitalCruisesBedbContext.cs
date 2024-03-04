using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NileCapitalCruisesBEAPI.Models;

public partial class NileCapitalCruisesBedbContext : DbContext
{
    public NileCapitalCruisesBedbContext()
    {
    }

    public NileCapitalCruisesBedbContext(DbContextOptions<NileCapitalCruisesBedbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCabin> TblCabins { get; set; }

    public virtual DbSet<TblCabinsAmenity> TblCabinsAmenities { get; set; }

    public virtual DbSet<TblCabinsContent> TblCabinsContents { get; set; }

    public virtual DbSet<TblCabinsPhoto> TblCabinsPhotos { get; set; }

    public virtual DbSet<TblCruise> TblCruises { get; set; }

    public virtual DbSet<TblCruisesContent> TblCruisesContents { get; set; }

    public virtual DbSet<TblDestination> TblDestinations { get; set; }

    public virtual DbSet<TblItinerariesContent> TblItinerariesContents { get; set; }

    public virtual DbSet<TblItinerary> TblItineraries { get; set; }

    public virtual DbSet<TblMasterDuration> TblMasterDurations { get; set; }

    public virtual DbSet<TblMasterLanguage> TblMasterLanguages { get; set; }

    public virtual DbSet<TblMasterRoomAmenitiesContent> TblMasterRoomAmenitiesContents { get; set; }

    public virtual DbSet<TblMasterRoomAmenity> TblMasterRoomAmenities { get; set; }

    public virtual DbSet<TblMasterWeekDay> TblMasterWeekDays { get; set; }

    public virtual DbSet<TblOperationDate> TblOperationDates { get; set; }

    public virtual DbSet<TblOperationDatesAllotment> TblOperationDatesAllotments { get; set; }

    public virtual DbSet<TblPeriod> TblPeriods { get; set; }

    public virtual DbSet<TblRate> TblRates { get; set; }

    public virtual DbSet<TblRatesCabinPerPeriod> TblRatesCabinPerPeriods { get; set; }

    public virtual DbSet<TblRatesPricesStructure> TblRatesPricesStructures { get; set; }

    public virtual DbSet<VwCabinsAmenity> VwCabinsAmenities { get; set; }

    public virtual DbSet<VwCabinsPhoto> VwCabinsPhotos { get; set; }

    public virtual DbSet<VwCruisesItinerariesOperationDate> VwCruisesItinerariesOperationDates { get; set; }

    public virtual DbSet<VwCruisesItinerary> VwCruisesItineraries { get; set; }

    public virtual DbSet<VwGetOperationDateAllotment> VwGetOperationDateAllotments { get; set; }

    public virtual DbSet<VwRatesPrice> VwRatesPrices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=5.77.32.134;Database=NileCapitalCruisesBEDB;User Id=titbBEdbUsr2024; Password=%i87yBn926Q4dt8^n9;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("titbBEdbUsr2024");

        modelBuilder.Entity<TblCabin>(entity =>
        {
            entity.HasKey(e => e.CabinId);

            entity.ToTable("tbl_Cabins", "dbo", tb => tb.HasTrigger("tbl_Cabins_Content_trigger"));

            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CabinNameSys).HasMaxLength(250);
            entity.Property(e => e.CabinPhoto).HasMaxLength(250);
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
        });

        modelBuilder.Entity<TblCabinsAmenity>(entity =>
        {
            entity.HasKey(e => e.CabinAmenitiesId);

            entity.ToTable("tbl_Cabins_Amenities", "dbo");

            entity.Property(e => e.CabinAmenitiesId).HasColumnName("CabinAmenitiesID");
            entity.Property(e => e.AmenitiesId).HasColumnName("AmenitiesID");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
        });

        modelBuilder.Entity<TblCabinsContent>(entity =>
        {
            entity.HasKey(e => e.CabinContentId);

            entity.ToTable("tbl_Cabins_Content", "dbo");

            entity.Property(e => e.CabinContentId).HasColumnName("CabinContentID");
            entity.Property(e => e.CabinBed).HasMaxLength(250);
            entity.Property(e => e.CabinDescription).HasColumnType("ntext");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CabinName).HasMaxLength(250);
            entity.Property(e => e.CabinSize).HasMaxLength(250);
            entity.Property(e => e.LangId).HasColumnName("LangID");

            entity.HasOne(d => d.Cabin).WithMany(p => p.TblCabinsContents)
                .HasForeignKey(d => d.CabinId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Cabins_Content_tbl_Cabins");
        });

        modelBuilder.Entity<TblCabinsPhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId);

            entity.ToTable("tbl_Cabins_Photos", "dbo");

            entity.Property(e => e.PhotoId).HasColumnName("PhotoID");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.PhotoFileName).HasMaxLength(250);
            entity.Property(e => e.PhotoPosition).HasDefaultValue(1);
            entity.Property(e => e.PhotoStatus).HasDefaultValue(true);
        });

        modelBuilder.Entity<TblCruise>(entity =>
        {
            entity.HasKey(e => e.CruiseId);

            entity.ToTable("tbl_Cruises", "dbo", tb => tb.HasTrigger("tbl_CruisesURL"));

            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.CruiseBanner).HasMaxLength(250);
            entity.Property(e => e.CruiseNameSys).HasMaxLength(250);
            entity.Property(e => e.CruisePhoto).HasMaxLength(250);
            entity.Property(e => e.CruiseUrl)
                .HasMaxLength(250)
                .HasColumnName("CruiseURL");
        });

        modelBuilder.Entity<TblCruisesContent>(entity =>
        {
            entity.HasKey(e => e.CruiseContentId);

            entity.ToTable("tbl_Cruises_Content", "dbo");

            entity.Property(e => e.CruiseContentId).HasColumnName("CruiseContentID");
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.CruiseName).HasMaxLength(250);
            entity.Property(e => e.LangId).HasColumnName("LangID");

            entity.HasOne(d => d.Cruise).WithMany(p => p.TblCruisesContents)
                .HasForeignKey(d => d.CruiseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Cruises_Content_tbl_Cruises");
        });

        modelBuilder.Entity<TblDestination>(entity =>
        {
            entity.HasKey(e => e.DestinationId);

            entity.ToTable("tbl_Destinations", "dbo");

            entity.Property(e => e.DestinationId).HasColumnName("DestinationID");
            entity.Property(e => e.DestinationNameSys).HasMaxLength(250);
        });

        modelBuilder.Entity<TblItinerariesContent>(entity =>
        {
            entity.HasKey(e => e.ItineraryContentId);

            entity.ToTable("tbl_Itineraries_Content", "dbo");

            entity.Property(e => e.ItineraryContentId).HasColumnName("ItineraryContentID");
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.ItineraryName).HasMaxLength(250);
            entity.Property(e => e.LangId).HasColumnName("LangID");
        });

        modelBuilder.Entity<TblItinerary>(entity =>
        {
            entity.HasKey(e => e.ItineraryId);

            entity.ToTable("tbl_Itineraries", "dbo", tb => tb.HasTrigger("tbl_Itineraries_Content_trigger"));

            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.DestinationIdfrom).HasColumnName("DestinationIDFrom");
            entity.Property(e => e.DestinationIdto).HasColumnName("DestinationIDTo");
            entity.Property(e => e.DurationId).HasColumnName("DurationID");
            entity.Property(e => e.ItineraryNameSys).HasMaxLength(250);
            entity.Property(e => e.WeekDayId).HasColumnName("WeekDayID");
        });

        modelBuilder.Entity<TblMasterDuration>(entity =>
        {
            entity.HasKey(e => e.DurationId).HasName("PK_tbl_Durations");

            entity.ToTable("tbl_Master_Durations", "dbo");

            entity.Property(e => e.DurationId).HasColumnName("DurationID");
            entity.Property(e => e.DurationName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblMasterLanguage>(entity =>
        {
            entity.HasKey(e => e.LangId);

            entity.ToTable("tbl_Master_Languages", "dbo");

            entity.Property(e => e.LangId).HasColumnName("LangID");
            entity.Property(e => e.LangAbbreviation).HasMaxLength(250);
            entity.Property(e => e.LangName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblMasterRoomAmenitiesContent>(entity =>
        {
            entity.HasKey(e => e.RoomAmenitiesContentId);

            entity.ToTable("tbl_Master_RoomAmenities_Content", "dbo");

            entity.Property(e => e.RoomAmenitiesContentId).HasColumnName("RoomAmenitiesContentID");
            entity.Property(e => e.LangId).HasColumnName("LangID");
            entity.Property(e => e.RoomAmenitiesId).HasColumnName("RoomAmenitiesID");
            entity.Property(e => e.RoomAmenitiesName).HasMaxLength(250);

            entity.HasOne(d => d.RoomAmenities).WithMany(p => p.TblMasterRoomAmenitiesContents)
                .HasForeignKey(d => d.RoomAmenitiesId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Master_RoomAmenities_Content_tbl_Master_RoomAmenities");
        });

        modelBuilder.Entity<TblMasterRoomAmenity>(entity =>
        {
            entity.HasKey(e => e.RoomAmenitiesId);

            entity.ToTable("tbl_Master_RoomAmenities", "dbo", tb => tb.HasTrigger("Master_RoomAmenitie_trigger"));

            entity.Property(e => e.RoomAmenitiesId).HasColumnName("RoomAmenitiesID");
            entity.Property(e => e.RoomAmenitiesCategoryId).HasColumnName("RoomAmenitiesCategoryID");
            entity.Property(e => e.RoomAmenitiesIconFont).HasMaxLength(250);
            entity.Property(e => e.RoomAmenitiesPhoto).HasMaxLength(250);
            entity.Property(e => e.RoomAmenitiesPosition).HasDefaultValue(1);
            entity.Property(e => e.RoomAmenitiesStatus).HasDefaultValue(true);
            entity.Property(e => e.RoomAmenitiesSys).HasMaxLength(250);
        });

        modelBuilder.Entity<TblMasterWeekDay>(entity =>
        {
            entity.HasKey(e => e.WeekDayId);

            entity.ToTable("tbl_Master_WeekDays", "dbo");

            entity.Property(e => e.WeekDayId)
                .ValueGeneratedNever()
                .HasColumnName("WeekDayID");
            entity.Property(e => e.WeekDayName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblOperationDate>(entity =>
        {
            entity.HasKey(e => e.OperationDateId);

            entity.ToTable("tbl_OperationDates", "dbo");

            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.OperationDateWeekDayId).HasColumnName("OperationDateWeekDayID");
        });

        modelBuilder.Entity<TblOperationDatesAllotment>(entity =>
        {
            entity.HasKey(e => e.OperationDateIdcabinId).HasName("PK_tbl_OperationDates_CabinAllotments");

            entity.ToTable("tbl_OperationDates_Allotments", "dbo");

            entity.Property(e => e.OperationDateIdcabinId).HasColumnName("OperationDateIDCabinID");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");

            entity.HasOne(d => d.OperationDate).WithMany(p => p.TblOperationDatesAllotments)
                .HasForeignKey(d => d.OperationDateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_OperationDates_Allotments_tbl_OperationDates");
        });

        modelBuilder.Entity<TblPeriod>(entity =>
        {
            entity.HasKey(e => e.PeriodId).HasName("PK_tbl_Rates_Periods");

            entity.ToTable("tbl_Periods", "dbo");

            entity.Property(e => e.PeriodId).HasColumnName("PeriodID");
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblRate>(entity =>
        {
            entity.HasKey(e => e.RateId);

            entity.ToTable("tbl_Rates", "dbo");

            entity.Property(e => e.RateId).HasColumnName("RateID");
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.RateNameSys).HasMaxLength(250);
        });

        modelBuilder.Entity<TblRatesCabinPerPeriod>(entity =>
        {
            entity.HasKey(e => e.RateCabinId);

            entity.ToTable("tbl_Rates_CabinPerPeriods", "dbo");

            entity.Property(e => e.RateCabinId).HasColumnName("RateCabinID");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.PeriodId).HasColumnName("PeriodID");
            entity.Property(e => e.RateId).HasColumnName("RateID");
        });

        modelBuilder.Entity<TblRatesPricesStructure>(entity =>
        {
            entity.HasKey(e => e.RatePriceStructureId).HasName("PK_tbl_Rate_Price_Structure");

            entity.ToTable("tbl_Rates_Prices_Structure", "dbo");

            entity.Property(e => e.RatePriceStructureId).HasColumnName("RatePriceStructureID");
            entity.Property(e => e.Description).HasColumnType("ntext");
        });

        modelBuilder.Entity<VwCabinsAmenity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Cabins_Amenities", "dbo");

            entity.Property(e => e.AmenitiesId).HasColumnName("AmenitiesID");
            entity.Property(e => e.CabinAmenitiesId).HasColumnName("CabinAmenitiesID");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CabinNameSys).HasMaxLength(250);
            entity.Property(e => e.RoomAmenitiesPhoto).HasMaxLength(250);
            entity.Property(e => e.RoomAmenitiesSys).HasMaxLength(250);
        });

        modelBuilder.Entity<VwCabinsPhoto>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Cabins_Photos", "dbo");

            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CabinNameSys).HasMaxLength(250);
            entity.Property(e => e.CabinPhoto).HasMaxLength(250);
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.CruiseNameSys).HasMaxLength(250);
            entity.Property(e => e.CruiseUrl)
                .HasMaxLength(250)
                .HasColumnName("CruiseURL");
            entity.Property(e => e.PhotoFileName).HasMaxLength(250);
            entity.Property(e => e.PhotoId).HasColumnName("PhotoID");
        });

        modelBuilder.Entity<VwCruisesItinerariesOperationDate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Cruises_Itineraries_OperationDates", "dbo");

            entity.Property(e => e.CruiseBanner).HasMaxLength(250);
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.CruiseNameSys).HasMaxLength(250);
            entity.Property(e => e.CruisePhoto).HasMaxLength(250);
            entity.Property(e => e.CruiseUrl)
                .HasMaxLength(250)
                .HasColumnName("CruiseURL");
            entity.Property(e => e.DurationId).HasColumnName("DurationID");
            entity.Property(e => e.DurationName).HasMaxLength(250);
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.ItineraryNameSys).HasMaxLength(250);
            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");
            entity.Property(e => e.OperationDateWeekDayId).HasColumnName("OperationDateWeekDayID");
            entity.Property(e => e.WeekDayId).HasColumnName("WeekDayID");
            entity.Property(e => e.WeekDayName).HasMaxLength(250);
        });

        modelBuilder.Entity<VwCruisesItinerary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Cruises_Itineraries", "dbo");

            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.CruiseNameSys).HasMaxLength(250);
            entity.Property(e => e.CruisePhoto).HasMaxLength(250);
            entity.Property(e => e.CruiseUrl)
                .HasMaxLength(250)
                .HasColumnName("CruiseURL");
            entity.Property(e => e.DestinationIdfrom).HasColumnName("DestinationIDFrom");
            entity.Property(e => e.DestinationIdto).HasColumnName("DestinationIDTo");
            entity.Property(e => e.DurationId).HasColumnName("DurationID");
            entity.Property(e => e.ItineraryContentId).HasColumnName("ItineraryContentID");
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.ItineraryName).HasMaxLength(250);
            entity.Property(e => e.ItineraryNameSys).HasMaxLength(250);
            entity.Property(e => e.LangAbbreviation).HasMaxLength(250);
            entity.Property(e => e.LangId).HasColumnName("LangID");
            entity.Property(e => e.LangName).HasMaxLength(250);
            entity.Property(e => e.WeekDayId).HasColumnName("WeekDayID");
            entity.Property(e => e.WeekDayName).HasMaxLength(250);
        });

        modelBuilder.Entity<VwGetOperationDateAllotment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_GetOperationDateAllotment", "dbo");

            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");
        });

        modelBuilder.Entity<VwRatesPrice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Rates_Prices", "dbo");

            entity.Property(e => e.CabinBed).HasMaxLength(250);
            entity.Property(e => e.CabinDescription).HasColumnType("ntext");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CabinNameSys).HasMaxLength(250);
            entity.Property(e => e.CabinPhoto).HasMaxLength(250);
            entity.Property(e => e.CabinSize).HasMaxLength(250);
            entity.Property(e => e.CruiseBanner).HasMaxLength(250);
            entity.Property(e => e.CruiseNameSys).HasMaxLength(250);
            entity.Property(e => e.CruisePhoto).HasMaxLength(250);
            entity.Property(e => e.CruiseUrl)
                .HasMaxLength(250)
                .HasColumnName("CruiseURL");
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.DestinationFrom).HasMaxLength(250);
            entity.Property(e => e.DestinationIdfrom).HasColumnName("DestinationIDFrom");
            entity.Property(e => e.DestinationIdto).HasColumnName("DestinationIDTo");
            entity.Property(e => e.DestinationTo).HasMaxLength(250);
            entity.Property(e => e.DurationId).HasColumnName("DurationID");
            entity.Property(e => e.DurationName).HasMaxLength(250);
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.ItineraryNameSys).HasMaxLength(250);
            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");
            entity.Property(e => e.PeriodId).HasColumnName("PeriodID");
            entity.Property(e => e.RateCabinId).HasColumnName("RateCabinID");
            entity.Property(e => e.RateId).HasColumnName("RateID");
            entity.Property(e => e.RateNameSys).HasMaxLength(250);
            entity.Property(e => e.WeekDayId).HasColumnName("WeekDayID");
            entity.Property(e => e.WeekDayName).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
