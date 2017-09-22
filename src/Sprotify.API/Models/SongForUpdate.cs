using System.ComponentModel.DataAnnotations;

namespace Sprotify.API.Models
{
    public class SongForUpdate : SongForManipulation
    {
        [Required]
        public override string Band { get; set; }
    }
}
