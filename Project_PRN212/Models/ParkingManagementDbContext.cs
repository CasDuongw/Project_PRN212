using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project_PRN212.Models;

public partial class ParkingManagementDbContext : DbContext
{
    public ParkingManagementDbContext()
    {
    }

    public ParkingManagementDbContext(DbContextOptions<ParkingManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ParkingSession> ParkingSessions { get; set; }

    public virtual DbSet<ParkingSlot> ParkingSlots { get; set; }

    public virtual DbSet<PriceConfig> PriceConfigs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleType> VehicleTypes { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LILDUONGW;Database=ParkingManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__ParkingS__C9F49270F08E8AFC");

            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.CardCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CheckInTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SlotId).HasColumnName("SlotID");
            entity.Property(e => e.StaffInId).HasColumnName("StaffInID");
            entity.Property(e => e.StaffOutId).HasColumnName("StaffOutID");
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.Property(e => e.TotalFee).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");

            entity.HasOne(d => d.Slot).WithMany(p => p.ParkingSessions)
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK__ParkingSe__SlotI__3F466844");

            entity.HasOne(d => d.StaffIn).WithMany(p => p.ParkingSessionStaffIns)
                .HasForeignKey(d => d.StaffInId)
                .HasConstraintName("FK__ParkingSe__Staff__412EB0B6");

            entity.HasOne(d => d.StaffOut).WithMany(p => p.ParkingSessionStaffOuts)
                .HasForeignKey(d => d.StaffOutId)
                .HasConstraintName("FK__ParkingSe__Staff__4222D4EF");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.ParkingSessions)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("FK__ParkingSe__Vehic__3E52440B");
        });

        modelBuilder.Entity<ParkingSlot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__ParkingS__0A124A4FF69A1258");

            entity.HasIndex(e => e.SlotCode, "UQ__ParkingS__38BD98CC35418681").IsUnique();

            entity.Property(e => e.SlotId).HasColumnName("SlotID");
            entity.Property(e => e.SlotCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ZoneId).HasColumnName("ZoneID");

            entity.HasOne(d => d.Zone).WithMany(p => p.ParkingSlots)
                .HasForeignKey(d => d.ZoneId)
                .HasConstraintName("FK__ParkingSl__ZoneI__36B12243");
        });

        modelBuilder.Entity<PriceConfig>(entity =>
        {
            entity.HasKey(e => e.PriceConfigId).HasName("PK__PriceCon__3EFB06E9F55779C3");

            entity.Property(e => e.PriceConfigId).HasColumnName("PriceConfigID");
            entity.Property(e => e.ApplyDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PricePerDay).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PricePerHour).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VehicleTypeId).HasColumnName("VehicleTypeID");

            entity.HasOne(d => d.VehicleType).WithMany(p => p.PriceConfigs)
                .HasForeignKey(d => d.VehicleTypeId)
                .HasConstraintName("FK__PriceConf__Vehic__3A81B327");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A3A628B55");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160EC093387").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC73B563A9");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4228A2BCD").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__286302EC");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicles__476B54B2DC56D00F");

            entity.HasIndex(e => e.LicensePlate, "UQ__Vehicles__026BC15C0CC6258F").IsUnique();

            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.VehicleTypeId).HasColumnName("VehicleTypeID");

            entity.HasOne(d => d.Owner).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Vehicles__OwnerI__300424B4");

            entity.HasOne(d => d.VehicleType).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.VehicleTypeId)
                .HasConstraintName("FK__Vehicles__Vehicl__2F10007B");
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(e => e.VehicleTypeId).HasName("PK__VehicleT__9F449623CBA2EE98");

            entity.HasIndex(e => e.TypeName, "UQ__VehicleT__D4E7DFA805D1E898").IsUnique();

            entity.Property(e => e.VehicleTypeId).HasColumnName("VehicleTypeID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(e => e.ZoneId).HasName("PK__Zones__601667953C2C9789");

            entity.Property(e => e.ZoneId).HasColumnName("ZoneID");
            entity.Property(e => e.VehicleTypeId).HasColumnName("VehicleTypeID");
            entity.Property(e => e.ZoneName).HasMaxLength(50);

            entity.HasOne(d => d.VehicleType).WithMany(p => p.Zones)
                .HasForeignKey(d => d.VehicleTypeId)
                .HasConstraintName("FK__Zones__VehicleTy__32E0915F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
