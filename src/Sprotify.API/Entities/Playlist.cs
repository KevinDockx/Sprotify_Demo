using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprotify.API.Entities
{
    public class Playlist
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }

        public virtual ICollection<PlaylistSong> Songs { get; set; }
               = new List<PlaylistSong>();
    }
}
