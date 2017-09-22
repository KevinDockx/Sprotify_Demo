using System.ComponentModel.DataAnnotations;

namespace Sprotify.API.Models
{
    public class PlaylistForCreation
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
