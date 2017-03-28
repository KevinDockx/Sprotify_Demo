using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sprotify.API.Models
{
    public abstract class SongForManipulation
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(150)]
        public virtual string Band { get; set; }
    }
}
