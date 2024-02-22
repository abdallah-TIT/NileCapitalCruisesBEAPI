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

    public virtual DbSet<TblCabinsContent> TblCabinsContents { get; set; }

    public virtual DbSet<TblCruise> TblCruises { get; set; }

    public virtual DbSet<TblCruisesContent> TblCruisesContents { get; set; }

    public virtual DbSet<TblDestination> TblDestinations { get; set; }

    public virtual DbSet<TblItinerariesContent> TblItinerariesContents { get; set; }

    public virtual DbSet<TblItinerary> TblItineraries { get; set; }

    public virtual DbSet<TblMasterDuration> TblMasterDurations { get; set; }

    public virtual DbSet<TblMasterLanguage> TblMasterLanguages { get; set; }

    public virtual DbSet<TblMasterWeekDay> TblMasterWeekDays { get; set; }

    public virtual DbSet<TblOperationDate> TblOperationDates { get; set; }

    public virtual DbSet<TblOperationDatesAllotment> TblOperationDatesAllotments { get; set; }

    public virtual DbSet<TblPeriod> TblPeriods { get; set; }

    public virtual DbSet<TblRate> TblRates { get; set; }

    public virtual DbSet<TblRatesCabinPerPeriod> TblRatesCabinPerPeriods { get; set; }

    public virtual DbSet<VwCruisesItinerariesOperationDate> VwCruisesItinerariesOperationDates { get; set; }

    public virtual DbSet<VwCruisesItinerary> VwCruisesItineraries { get; set; }

    public virtual DbSet<VwGetOperationDateAllotment> VwGetOperationDateAllotments { get; set; }

    public virtual DbSet<VwRatesPrice> VwRatesPrices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQL2019;Initial Catalog=NileCapitalCruisesBEDB;Persist Security Info=False;User ID=sa;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCabin>(entity =>
        {
            entity.HasKey(e => e.CabinId);

            entity.ToTable("tbl_Cabins", tb => tb.HasTrigger("tbl_Cabins_Content_trigger"));

            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CabinNameSys).HasMaxLength(250);
            entity.Property(e => e.CabinPhoto).HasMaxLength(250);
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
        });

        modelBuilder.Entity<TblCabinsContent>(entity =>
        {
            entity.HasKey(e => e.CabinContentId);

            entity.ToTable("tbl_Cabins_Content");

            entity.Property(e => e.CabinContentId).HasColumnName("CabinContentID");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CabinName).HasMaxLength(250);
            entity.Property(e => e.LangId).HasColumnName("LangID");

            entity.HasOne(d => d.Cabin).WithMany(p => p.TblCabinsContents)
                .HasForeignKey(d => d.CabinId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Cabins_Content_tbl_Cabins");
        });

        modelBuilder.Entity<TblCruise>(entity =>
        {
            entity.HasKey(e => e.CruiseId);

            entity.ToTable("tbl_Cruises", tb => tb.HasTrigger("tbl_CruisesURL"));

            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.CruiseNameSys).HasMaxLength(250);
            entity.Property(e => e.CruiseUrl)
                .HasMaxLength(250)
                .HasColumnName("CruiseURL");
        });

        modelBuilder.Entity<TblCruisesContent>(entity =>
        {
            entity.HasKey(e => e.CruiseContentId);

            entity.ToTable("tbl_Cruises_Content");

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

            entity.ToTable("tbl_Destinations");

            entity.Property(e => e.DestinationId).HasColumnName("DestinationID");
            entity.Property(e => e.DestinationNameSys).HasMaxLength(250);
        });

        modelBuilder.Entity<TblItinerariesContent>(entity =>
        {
            entity.HasKey(e => e.ItineraryContentId);

            entity.ToTable("tbl_Itineraries_Content");

            entity.Property(e => e.ItineraryContentId).HasColumnName("ItineraryContentID");
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.ItineraryName).HasMaxLength(250);
            entity.Property(e => e.LangId).HasColumnName("LangID");
        });

        modelBuilder.Entity<TblItinerary>(entity =>
        {
            entity.HasKey(e => e.ItineraryId);

            entity.ToTable("tbl_Itineraries", tb => tb.HasTrigger("tbl_Itineraries_Content_trigger"));

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

            entity.ToTable("tbl_Master_Durations");

            entity.Property(e => e.DurationId).HasColumnName("DurationID");
            entity.Property(e => e.DurationName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblMasterLanguage>(entity =>
        {
            entity.HasKey(e => e.LangId);

            entity.ToTable("tbl_Master_Languages");

            entity.Property(e => e.LangId).HasColumnName("LangID");
            entity.Property(e => e.LangAbbreviation).HasMaxLength(250);
            entity.Property(e => e.LangName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblMasterWeekDay>(entity =>
        {
            entity.HasKey(e => e.WeekDayId);

            entity.ToTable("tbl_Master_WeekDays");

            entity.Property(e => e.WeekDayId)
                .ValueGeneratedNever()
                .HasColumnName("WeekDayID");
            entity.Property(e => e.WeekDayName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblOperationDate>(entity =>
        {
            entity.HasKey(e => e.OperationDateId);

            entity.ToTable("tbl_OperationDates");

            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.OperationDateWeekDayId).HasColumnName("OperationDateWeekDayID");
        });

        modelBuilder.Entity<TblOperationDatesAllotment>(entity =>
        {
            entity.HasKey(e => e.OperationDateIdcabinId).HasName("PK_tbl_OperationDates_CabinAllotments");

            entity.ToTable("tbl_OperationDates_Allotments");

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

            entity.ToTable("tbl_Periods");

            entity.Property(e => e.PeriodId).HasColumnName("PeriodID");
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.DateEnd).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblRate>(entity =>
        {
            entity.HasKey(e => e.RateId);

            entity.ToTable("tbl_Rates");

            entity.Property(e => e.RateId).HasColumnName("RateID");
            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.RateNameSys).HasMaxLength(250);
        });

        modelBuilder.Entity<TblRatesCabinPerPeriod>(entity =>
        {
            entity.HasKey(e => e.RateCabinId);

            entity.ToTable("tbl_Rates_CabinPerPeriods");

            entity.Property(e => e.RateCabinId).HasColumnName("RateCabinID");
            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.PeriodId).HasColumnName("PeriodID");
            entity.Property(e => e.RateId).HasColumnName("RateID");
        });

        modelBuilder.Entity<VwCruisesItinerariesOperationDate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Cruises_Itineraries_OperationDates");

            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.DurationId).HasColumnName("DurationID");
            entity.Property(e => e.DurationName).HasMaxLength(250);
            entity.Property(e => e.ItineraryId).HasColumnName("ItineraryID");
            entity.Property(e => e.ItineraryNameSys).HasMaxLength(250);
            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");
            entity.Property(e => e.OperationDateWeekDayId).HasColumnName("OperationDateWeekDayID");
        });

        modelBuilder.Entity<VwCruisesItinerary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Cruises_Itineraries");

            entity.Property(e => e.CruiseId).HasColumnName("CruiseID");
            entity.Property(e => e.CruiseNameSys).HasMaxLength(250);
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
                .ToView("vw_GetOperationDateAllotment");

            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.OperationDateId).HasColumnName("OperationDateID");
        });

        modelBuilder.Entity<VwRatesPrice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Rates_Prices");

            entity.Property(e => e.CabinId).HasColumnName("CabinID");
            entity.Property(e => e.CabinNameSys).HasMaxLength(250);
            entity.Property(e => e.CabinPhoto).HasMaxLength(250);
            entity.Property(e => e.CruiseNameSys).HasMaxLength(250);
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
