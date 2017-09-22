using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.API.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}