using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BussinessObject.Models;

public partial class FinalProPrn231Context : DbContext
{
    public FinalProPrn231Context()
    {
    }

    public FinalProPrn231Context(DbContextOptions<FinalProPrn231Context> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<FavoriteClub> FavoriteClubs { get; set; }

    public virtual DbSet<Goal> Goals { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<ManagerClub> ManagerClubs { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchDetail> MatchDetails { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerMatchRegistration> PlayerMatchRegistrations { get; set; }

    public virtual DbSet<Ranking> Rankings { get; set; }

    public virtual DbSet<Referee> Referees { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Stadium> Stadia { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=LAPTOP-G54QJ7QB\\MSSQLSERVER01; database =FinalPro_PRN231;uid=sa;pwd=123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.ToTable("Club");

            entity.Property(e => e.Logo).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.City).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Club_City");

            entity.HasOne(d => d.Stadium).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.StadiumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Club_Stadium");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FavoriteClub>(entity =>
        {
            entity.ToTable("FavoriteClub");

            entity.HasOne(d => d.Club).WithMany(p => p.FavoriteClubs)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteClub_Club");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteClubs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoriteClub_User");
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.ToTable("Goal");

            entity.HasOne(d => d.Match).WithMany(p => p.Goals)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Goal_Match");

            entity.HasOne(d => d.Player).WithMany(p => p.Goals)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Goal_Player");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.ToTable("Manager");

            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.Managers)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Manager_Country");
        });

        modelBuilder.Entity<ManagerClub>(entity =>
        {
            entity.HasKey(e => new { e.ManagerId, e.ClubId });

            entity.ToTable("Manager_Club");

            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Club).WithMany(p => p.ManagerClubs)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Manager_Club_Club");

            entity.HasOne(d => d.Manager).WithMany(p => p.ManagerClubs)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Manager_Club_Manager");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("Match");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Result)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AwayNavigation).WithMany(p => p.MatchAwayNavigations)
                .HasForeignKey(d => d.Away)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_Away");

            entity.HasOne(d => d.HomeNavigation).WithMany(p => p.MatchHomeNavigations)
                .HasForeignKey(d => d.Home)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_Home");

            entity.HasOne(d => d.Ref).WithMany(p => p.Matches)
                .HasForeignKey(d => d.RefId)
                .HasConstraintName("FK_Match_Referee");

            entity.HasOne(d => d.Stadium).WithMany(p => p.Matches)
                .HasForeignKey(d => d.StadiumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_Stadium");
        });

        modelBuilder.Entity<MatchDetail>(entity =>
        {
            entity.ToTable("MatchDetail");

            entity.Property(e => e.AwayRcard).HasColumnName("AwayRCard");
            entity.Property(e => e.AwayYcard).HasColumnName("AwayYCard");
            entity.Property(e => e.HomeRcard).HasColumnName("HomeRCard");
            entity.Property(e => e.HomeYcard).HasColumnName("HomeYCard");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchDetails)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MatchDetail_Match");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("Player");

            entity.Property(e => e.Avatar).IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Club).WithMany(p => p.Players)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_Club");

            entity.HasOne(d => d.Country).WithMany(p => p.Players)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_Country");
        });

        modelBuilder.Entity<PlayerMatchRegistration>(entity =>
        {
            entity.ToTable("PlayerMatchRegistration");

            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Match).WithMany(p => p.PlayerMatchRegistrations)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlayerMatchRegistration_Match");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerMatchRegistrations)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlayerMatchRegistration_Player");
        });

        modelBuilder.Entity<Ranking>(entity =>
        {
            entity.HasKey(e => new { e.ClubId, e.Round });

            entity.ToTable("Ranking");

            entity.HasOne(d => d.Club).WithMany(p => p.Rankings)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ranking_Club1");
        });

        modelBuilder.Entity<Referee>(entity =>
        {
            entity.HasKey(e => e.RefId);

            entity.ToTable("Referee");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Stadium>(entity =>
        {
            entity.ToTable("Stadium");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
