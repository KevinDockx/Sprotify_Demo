using Sprotify.API.Entities;
using System;
using System.Collections.Generic;

namespace Sprotify.API.Services
{
    public interface ISprotifyRepository
    {
        bool PlaylistExists(Guid playlistId);
        IEnumerable<Playlist> GetPlaylists();
        Playlist GetPlaylist(Guid playlistId, bool includeSongs = false);
        IEnumerable<Song> GetSongsFromPlaylist(Guid playlistId);
        Song GetSongFromPlaylist(Guid playlistId, Guid songId);
        void AddSongToPlaylist(Guid playlistId, Song song);
        void UpdateSong(Song song);
        void DeleteSong(Song song);
        bool Save();
    }
}
