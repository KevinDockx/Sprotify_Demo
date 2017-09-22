using System;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.Web.Models.Music
{
    public class AddSongModel
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(150)]
        public virtual string Band { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }
    }
}
