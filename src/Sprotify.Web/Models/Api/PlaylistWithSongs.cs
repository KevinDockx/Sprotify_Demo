using System;
using System.Collections.Generic;

namespace Sprotify.Web.Models.Api
{
    public class PlaylistWithSongs
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid OwnerId { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
