using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public ICollection<Song> Songs { get; set; }
               = new List<Song>();
    }
}
