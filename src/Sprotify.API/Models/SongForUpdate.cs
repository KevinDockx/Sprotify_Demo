using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sprotify.API.Models
{
    public class SongForUpdate : SongForManipulation
    {
        [Required]
        public override string Band { get; set; }
    }
}
