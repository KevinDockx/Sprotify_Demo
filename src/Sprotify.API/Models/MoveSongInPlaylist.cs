using System.ComponentModel.DataAnnotations;

namespace Sprotify.API.Models
{
    public class MoveSongInPlaylist
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int OldIndex { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int NewIndex { get; set; }
    }
}