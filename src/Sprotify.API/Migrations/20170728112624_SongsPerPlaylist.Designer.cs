using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Sprotify.API.Entities;

namespace Sprotify.API.Migrations
{
    [DbContext(typeof(SprotifyContext))]
    [Migration("20170728112624_SongsPerPlaylist")]
    partial class SongsPerPlaylist
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sprotify.API.Entities.Playlist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<Guid>("OwnerId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("Sprotify.API.Entities.PlaylistSong", b =>
                {
                    b.Property<Guid>("PlaylistId");

                    b.Property<Guid>("SongId");

                    b.Property<int>("Index");

                    b.HasKey("PlaylistId", "SongId", "Index");

                    b.HasIndex("SongId");

                    b.ToTable("PlaylistSong");
                });

            modelBuilder.Entity("Sprotify.API.Entities.Song", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Band")
                        .HasMaxLength(150);

                    b.Property<TimeSpan>("Duration");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Sprotify.API.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sprotify.API.Entities.Playlist", b =>
                {
                    b.HasOne("Sprotify.API.Entities.User", "Owner")
                        .WithMany("Playlists")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sprotify.API.Entities.PlaylistSong", b =>
                {
                    b.HasOne("Sprotify.API.Entities.Playlist", "Playlist")
                        .WithMany("Songs")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sprotify.API.Entities.Song", "Song")
                        .WithMany("Playlists")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
