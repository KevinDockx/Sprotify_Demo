using Microsoft.EntityFrameworkCore;

namespace Sprotify.API.Entities
{
    public class SprotifyContext : DbContext
    {
        // constructor required for migrations 
        public SprotifyContext(DbContextOptions<SprotifyContext> options)
               : base(options)
        {            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaylistSong>()
                .HasKey(x => new {x.PlaylistId, x.SongId, x.Index});

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(x => x.Playlist)
                .WithMany(x => x.Songs)
                .HasForeignKey(x => x.PlaylistId);

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(x => x.Song)
                .WithMany(x => x.Playlists)
                .HasForeignKey(x => x.SongId);
        }
    }
}
