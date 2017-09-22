using System;
using System.Collections.Generic;

namespace Sprotify.API.Models
{
    public class PlaylistWithSongs
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid OwnerId { get; set; }
        public ICollection<SongInPlaylist> Songs { get; set; }
            = new List<SongInPlaylist>();
    }
}
