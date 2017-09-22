using Sprotify.Web.Models.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sprotify.Web.Services
{
    public class PlaylistService : ApiServiceBase
    {
        public PlaylistService(SprotifyHttpClient client) : base(client) { }

        public async Task<IEnumerable<Playlist>> GetPlaylists()
        {
            return await Get<IEnumerable<Playlist>>("playlists").ConfigureAwait(false);
        }

        public async Task<PlaylistWithSongs> GetPlaylist(Guid id)
        {
            return await Get<PlaylistWithSongs>($"playlists/{id}?expand=true").ConfigureAwait(false);
        }

        public async Task<PlaylistWithSongs> CreatePlaylist(string title, string description)
        {
            return await Post<PlaylistWithSongs>("playlists", new { Title = title, Description = description });
        }

        public async Task<Song> AddSongToPlaylist(Guid id, string title, string band, TimeSpan duration)
        {
            return await Post<Song>($"playlists/{id}/songs", new { Title = title, Band = band, Duration = duration });
        }
    }
}
