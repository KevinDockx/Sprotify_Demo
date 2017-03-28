using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprotify.API.Entities
{
    public class SprotifyContext : DbContext
    {
        // constructor required for migrations 
        public SprotifyContext(DbContextOptions<SprotifyContext> options)
               : base(options)
        {            
        }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
    }
}
