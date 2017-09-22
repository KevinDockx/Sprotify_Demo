using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.API.Entities
{
    public class Song
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(150)]
        public string Band { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public virtual ICollection<PlaylistSong> Playlists { get; set; }
    }
}
