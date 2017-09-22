using System;

namespace Sprotify.API.Services.Models
{
    public class SongWithPlaylistInfo : Entities.Song
    {
        public Guid PlaylistId { get; set; }
        public int Index { get; set; }
    }
}