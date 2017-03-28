using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("PlaylistId")]
        public Playlist Playlist { get; set; }
        public Guid PlaylistId { get; set; }
    }
}
