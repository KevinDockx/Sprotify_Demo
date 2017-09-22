using System.Collections.Generic;

namespace Sprotify.API.Services.Models
{
    public class PlaylistWithSongs : Entities.Playlist
    {
        public new ICollection<SongWithPlaylistInfo> Songs { get; set; } = new List<SongWithPlaylistInfo>();
    }
}