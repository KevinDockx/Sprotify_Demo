using System;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.API.Models
{
    public class SongForPlaylist
    {
        [Required]
        public Guid SongId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Index { get; set; }
    }
}