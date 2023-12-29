﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenData.Basetball.AbaLeague.Persistence;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    [DbContext(typeof(AbaLeagueDbContext))]
    [Migration("20231228112028_AddAnotherNamesForSinglePlayer")]
    partial class AddAnotherNamesForSinglePlayer
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BaseUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BoxScoreUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CalendarUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MatchUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfficalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short?>("ProcessorTypeId")
                        .HasColumnType("smallint");

                    b.Property<string>("RosterUrl")
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

                    b.HasIndex("ProcessorTypeId");

                    b.ToTable("Leagues");
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
                        },
                        new
                        {
                            Id = (short)6,
                            Name = "Coach"
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

                    b.Property<int>("TeamId")
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

                    b.Property<string>("IncrowdUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamSourceUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamUrl")
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
                });

            modelBuilder.Entity("OpenData.Basketball.AbaLeague.Domain.Entities.AnotherNameItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("AnotherNameItem");
                });

            modelBuilder.Entity("OpenData.Basketball.AbaLeague.Domain.Entities.BoxScore", b =>
                {
                    b.Property<int>("RoundMatchId")
                        .HasColumnType("int");

                    b.Property<int>("RosterItemId")
                        .HasColumnType("int");

                    b.Property<int?>("AgainstBlock")
                        .HasColumnType("int");

                    b.Property<int?>("Assists")
                        .HasColumnType("int");

                    b.Property<int?>("CommittedFoul")
                        .HasColumnType("int");

                    b.Property<int?>("DefensiveRebounds")
                        .HasColumnType("int");

                    b.Property<int?>("InFavoureOfBlock")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("Minutes")
                        .HasColumnType("time");

                    b.Property<int?>("OffensiveRebounds")
                        .HasColumnType("int");

                    b.Property<int?>("PlusMinus")
                        .HasColumnType("int");

                    b.Property<int?>("PointFrom2ndChance")
                        .HasColumnType("int");

                    b.Property<int?>("PointFromFastBreak")
                        .HasColumnType("int");

                    b.Property<int?>("PointFromPain")
                        .HasColumnType("int");

                    b.Property<int?>("Points")
                        .HasColumnType("int");

                    b.Property<int?>("RankValue")
                        .HasColumnType("int");

                    b.Property<int?>("ReceivedFoul")
                        .HasColumnType("int");

                    b.Property<int?>("ShotAttempted1Pt")
                        .HasColumnType("int");

                    b.Property<int?>("ShotAttempted2Pt")
                        .HasColumnType("int");

                    b.Property<int?>("ShotAttempted3Pt")
                        .HasColumnType("int");

                    b.Property<int?>("ShotMade1Pt")
                        .HasColumnType("int");

                    b.Property<int?>("ShotMade2Pt")
                        .HasColumnType("int");

                    b.Property<int?>("ShotMade3Pt")
                        .HasColumnType("int");

                    b.Property<decimal?>("ShotPrc")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("ShotPrc1Pt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("ShotPrc2Pt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("Steals")
                        .HasColumnType("int");

                    b.Property<int?>("TotalRebounds")
                        .HasColumnType("int");

                    b.Property<int?>("Turnover")
                        .HasColumnType("int");

                    b.Property<decimal?>("shotPrc3Pt")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("RoundMatchId", "RosterItemId");

                    b.HasIndex("RosterItemId");

                    b.ToTable("BoxScores");
                });

            modelBuilder.Entity("OpenData.Basketball.AbaLeague.Domain.Entities.ProcessorType", b =>
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

                    b.ToTable("ProcessorTypes");

                    b.HasData(
                        new
                        {
                            Id = (short)1,
                            Name = "Unknow"
                        },
                        new
                        {
                            Id = (short)2,
                            Name = "Euro"
                        },
                        new
                        {
                            Id = (short)3,
                            Name = "Aba"
                        });
                });

            modelBuilder.Entity("OpenData.Basketball.AbaLeague.Domain.Entities.Result", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Attendency")
                        .HasColumnType("int");

                    b.Property<int>("AwayTeamPoint")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("HomeTeamPoints")
                        .HasColumnType("int");

                    b.Property<int>("RoundMatchId")
                        .HasColumnType("int");

                    b.Property<string>("UpdateBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Venue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoundMatchId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("OpenData.Basketball.AbaLeague.Domain.Entities.RoundMatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AwayTeamId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("HomeTeamId")
                        .HasColumnType("int");

                    b.Property<int?>("LeagueId")
                        .HasColumnType("int");

                    b.Property<int>("MatchNo")
                        .HasColumnType("int");

                    b.Property<bool>("OffSeason")
                        .HasColumnType("bit");

                    b.Property<int>("Round")
                        .HasColumnType("int");

                    b.Property<string>("UpdateBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("LeagueId");

                    b.ToTable("RoundMatches");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.League", b =>
                {
                    b.HasOne("OpenData.Basketball.AbaLeague.Domain.Entities.ProcessorType", "ProcessorType")
                        .WithMany()
                        .HasForeignKey("ProcessorTypeId");

                    b.Navigation("ProcessorType");
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

                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Team", "Team")
                        .WithMany("RosterItems")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("League");

                    b.Navigation("Player");

                    b.Navigation("Team");
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

            modelBuilder.Entity("OpenData.Basketball.AbaLeague.Domain.Entities.AnotherNameItem", b =>
                {
                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Player", "Player")
                        .WithMany("AnotherNameItems")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("OpenData.Basketball.AbaLeague.Domain.Entities.BoxScore", b =>
                {
                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.RosterItem", "RosterItem")
                        .WithMany()
                        .HasForeignKey("RosterItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenData.Basketball.AbaLeague.Domain.Entities.RoundMatch", "RoundMatch")
                        .WithMany()
                        .HasForeignKey("RoundMatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RosterItem");

                    b.Navigation("RoundMatch");
                });

            modelBuilder.Entity("OpenData.Basketball.AbaLeague.Domain.Entities.Result", b =>
                {
                    b.HasOne("OpenData.Basketball.AbaLeague.Domain.Entities.RoundMatch", "RoundMatch")
                        .WithMany()
                        .HasForeignKey("RoundMatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoundMatch");
                });

            modelBuilder.Entity("OpenData.Basketball.AbaLeague.Domain.Entities.RoundMatch", b =>
                {
                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Team", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId");

                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.Team", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId");

                    b.HasOne("OpenData.Basetball.AbaLeague.Domain.Entities.League", null)
                        .WithMany("RoundMatches")
                        .HasForeignKey("LeagueId");

                    b.Navigation("AwayTeam");

                    b.Navigation("HomeTeam");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.League", b =>
                {
                    b.Navigation("RoundMatches");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.Player", b =>
                {
                    b.Navigation("AnotherNameItems");
                });

            modelBuilder.Entity("OpenData.Basetball.AbaLeague.Domain.Entities.Team", b =>
                {
                    b.Navigation("RosterItems");
                });
#pragma warning restore 612, 618
        }
    }
}
