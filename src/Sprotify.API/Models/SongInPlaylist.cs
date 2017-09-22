using System;

namespace Sprotify.API.Models
{
    public class SongInPlaylist : Song
    {
        public Guid PlaylistId { get; set; }
        public int Index { get; set; }
    }
}