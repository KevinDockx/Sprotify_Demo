using System;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.API.Entities
{
    public class PlaylistSong
    {
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }
        [Required]
        public int Index { get; set; }

        public Playlist Playlist { get; set; }
        public Song Song { get; set; }
    }
}