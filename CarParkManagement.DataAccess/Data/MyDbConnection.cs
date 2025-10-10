using System;
using System.Collections.Generic;
using CarParkManagement.DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.DataAccess.Data;

public partial class MyDbConnection : DbContext
{
    public MyDbConnection(DbContextOptions<MyDbConnection> options)
        : base(options)
    {
    }

    public virtual DbSet<ChargeRate> ChargeRates { get; set; }

    public virtual DbSet<ParkingAllocation> ParkingAllocations { get; set; }

    public virtual DbSet<ParkingSpace> ParkingSpaces { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChargeRate>(entity =>
        {
            entity.HasKey(e => e.ChargeRateId).HasName("PK__ChargeRa__3271C5C462379CDD");

            entity.ToTable("ChargeRate");

            entity.HasIndex(e => e.Size, "UQ__ChargeRa__8E8EABE574047334").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExtraChargePer5Min).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.RatePerMinute).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Size).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ParkingAllocation>(entity =>
        {
            entity.HasKey(e => e.ParkingAllocationId).HasName("PK__ParkingA__A0CC232D4E5BD213");

            entity.ToTable("ParkingAllocation");

            entity.Property(e => e.Charge).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.TimeIn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ParkingSpace).WithMany(p => p.ParkingAllocations)
                .HasForeignKey(d => d.ParkingSpaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ParkingAl__Parki__48CFD27E");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.ParkingAllocations)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ParkingAl__Vehic__47DBAE45");
        });

        modelBuilder.Entity<ParkingSpace>(entity =>
        {
            entity.HasKey(e => e.ParkingSpaceId).HasName("PK__ParkingS__9653A2162C2D7409");

            entity.ToTable("ParkingSpace");

            entity.HasIndex(e => e.SpaceNumber, "UQ__ParkingS__0B4A45DDD4BD10E8").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsOccupied).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicle__476B54925857B8F3");

            entity.ToTable("Vehicle");

            entity.HasIndex(e => e.VehicleReg, "UQ__Vehicle__1DC72E9E09527F01").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.VehicleReg).HasMaxLength(20);
            entity.Property(e => e.VehicleType).HasMaxLength(20);

            entity.HasOne(d => d.ChargeRate).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.ChargeRateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehicle_ChargeRate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
