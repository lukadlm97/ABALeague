﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenData.Basetball.AbaLeague.Persistence;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    [DbContext(typeof(AbaLeagueDbContext))]
    partial class AbaLeagueDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeIso")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodeIso2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("nvarchar(125)");

                    b.HasKey("Id");

                    b.ToTable("Countries");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CodeIso = "SRB",
                            CodeIso2 = "RS",
                            Name = "Serbia"
                        },
                        new
                        {
                            Id = 2,
                            CodeIso = "BIH",
                            CodeIso2 = "BH",
                            Name = "Bosnia and Herzegovina"
                        },
                        new
                        {
                            Id = 3,
                            CodeIso = "CRO",
                            CodeIso2 = "HR",
                            Name = "Croatia"
                        });
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BaseUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OfficalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Season")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StandingUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdateBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Leagues");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedBy = "Sys",
                            CreatedDate = new DateTime(2023, 6, 11, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(3150),
                            OfficalName = "NLB ABA League",
                            Season = "2022/23",
                            ShortName = "ABA",
                            StandingUrl = "ur1",
                            UpdateBy = "Sys",
                            UpdatedDate = new DateTime(2023, 6, 13, 17, 53, 13, 587, DateTimeKind.Utc).AddTicks(3164)
                        },
                        new
                        {
                            Id = 2,
                            CreatedBy = "Sys",
                            CreatedDate = new DateTime(2023, 6, 11, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(3175),
                            OfficalName = "NLB ABA League 2",
                            Season = "2022/23",
                            ShortName = "ABA2",
                            StandingUrl = "ur1",
                            UpdateBy = "Sys",
                            UpdatedDate = new DateTime(2023, 6, 13, 17, 53, 13, 587, DateTimeKind.Utc).AddTicks(3175)
                        });
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("PositionId")
                        .HasColumnType("smallint");

                    b.Property<string>("UpdateBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("PositionId");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            Id = 3,
                            CountryId = 1,
                            CreatedBy = "Sys",
                            CreatedDate = new DateTime(2023, 6, 11, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(5085),
                            DateOfBirth = new DateTime(1991, 6, 13, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(5078),
                            Height = 193,
                            Name = "Dragan Milosavljevic",
                            Nationality = "Serbian",
                            PositionId = (short)2,
                            UpdateBy = "Sys",
                            UpdatedDate = new DateTime(2023, 6, 13, 17, 53, 13, 587, DateTimeKind.Utc).AddTicks(5087)
                        },
                        new
                        {
                            Id = 4,
                            CountryId = 3,
                            CreatedBy = "Sys",
                            CreatedDate = new DateTime(2023, 6, 11, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(5098),
                            DateOfBirth = new DateTime(1987, 6, 13, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(5097),
                            Height = 210,
                            Name = "Miro Bilan",
                            Nationality = "Croatian",
                            PositionId = (short)5,
                            UpdateBy = "Sys",
                            UpdatedDate = new DateTime(2023, 6, 13, 17, 53, 13, 587, DateTimeKind.Utc).AddTicks(5099)
                        },
                        new
                        {
                            Id = 5,
                            CountryId = 1,
                            CreatedBy = "Sys",
                            CreatedDate = new DateTime(2023, 6, 11, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(5103),
                            DateOfBirth = new DateTime(2002, 6, 13, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(5102),
                            Height = 201,
                            Name = "Uros Trifunovic",
                            Nationality = "Serbian",
                            PositionId = (short)3,
                            UpdateBy = "Sys",
                            UpdatedDate = new DateTime(2023, 6, 13, 17, 53, 13, 587, DateTimeKind.Utc).AddTicks(5104)
                        },
                        new
                        {
                            Id = 6,
                            CountryId = 1,
                            CreatedBy = "Sys",
                            CreatedDate = new DateTime(2023, 6, 11, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(5110),
                            DateOfBirth = new DateTime(2006, 6, 13, 19, 53, 13, 587, DateTimeKind.Utc).AddTicks(5107),
                            Height = 210,
                            Name = "Nikola Topic",
                            Nationality = "Serbian",
                            PositionId = (short)1,
                            UpdateBy = "Sys",
                            UpdatedDate = new DateTime(2023, 6, 13, 17, 53, 13, 587, DateTimeKind.Utc).AddTicks(5110)
                        });
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.Position", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("Positions");

                    b.HasData(
                        new
                        {
                            Id = (short)1,
                            Name = "Guard"
                        },
                        new
                        {
                            Id = (short)2,
                            Name = "ShootingGuard"
                        },
                        new
                        {
                            Id = (short)3,
                            Name = "Forward"
                        },
                        new
                        {
                            Id = (short)4,
                            Name = "PowerForward"
                        },
                        new
                        {
                            Id = (short)5,
                            Name = "Center"
                        });
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.RosterItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfInsertion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndOfActivePeriod")
                        .HasColumnType("datetime2");

                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("RosterItems");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.SeasonResources", b =>
                {
                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<string>("TeamSourceUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeamId", "LeagueId");

                    b.HasIndex("LeagueId");

                    b.ToTable("SeasonResources");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdateBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Teams");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CountryId = 1,
                            CreatedBy = "Sys",
                            CreatedDate = new DateTime(2023, 6, 11, 19, 53, 13, 588, DateTimeKind.Utc).AddTicks(7348),
                            Name = "Partizan",
                            ShortCode = "PAR",
                            UpdateBy = "Sys",
                            UpdatedDate = new DateTime(2023, 6, 13, 17, 53, 13, 588, DateTimeKind.Utc).AddTicks(7353)
                        },
                        new
                        {
                            Id = 2,
                            CountryId = 2,
                            CreatedBy = "Sys",
                            CreatedDate = new DateTime(2023, 6, 11, 19, 53, 13, 588, DateTimeKind.Utc).AddTicks(7364),
                            Name = "Igokea",
                            ShortCode = "IGO",
                            UpdateBy = "Sys",
                            UpdatedDate = new DateTime(2023, 6, 13, 17, 53, 13, 588, DateTimeKind.Utc).AddTicks(7365)
                        });
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.Player", b =>
                {
                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.RosterItem", b =>
                {
                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.League", "League")
                        .WithMany()
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Team", null)
                        .WithMany("RosterItems")
                        .HasForeignKey("TeamId");

                    b.Navigation("League");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.SeasonResources", b =>
                {
                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.League", "League")
                        .WithMany()
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("League");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.Team", b =>
                {
                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.Team", b =>
                {
                    b.Navigation("RosterItems");
                });
#pragma warning restore 612, 618
        }
    }
}
