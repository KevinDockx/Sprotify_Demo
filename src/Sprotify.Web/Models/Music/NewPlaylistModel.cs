using System.ComponentModel.DataAnnotations;

namespace Sprotify.Web.Models.Music
{
    public class NewPlaylistModel
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
