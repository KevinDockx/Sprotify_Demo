using Sprotify.API.Entities;
using System;
using System.Collections.Generic;
using Sprotify.API.Services.Models;

namespace Sprotify.API.Services
{
    public interface ISprotifyRepository
    {
        bool PlaylistExists(Guid playlistId);
        IEnumerable<Playlist> GetPlaylists();
        PlaylistWithSongs GetPlaylist(Guid playlistId, bool includeSongs = false);

        void CreatePlaylist(Guid ownerId, Playlist playlist);

        IEnumerable<Song> GetSongs(string search);
        Song GetSong(Guid songId);

        IEnumerable<SongWithPlaylistInfo> GetSongsFromPlaylist(Guid playlistId);
        SongWithPlaylistInfo GetSongFromPlaylist(Guid playlistId, Guid songId);

        void AddSong(Song song);
        void UpdateSong(Song song);
        void DeleteSong(Song song);

        void AddSongToPlaylist(Guid playlistId, Guid songId, int index);
        void UpdateSongInPlaylist(Guid playlistId, Guid songId, int oldIndex, int newIndex);
        void RemoveSongFromPlaylist(Guid playlistId, Guid songId, int index);

        bool UserExists(Guid userId);
        IEnumerable<User> GetUsers();
        User GetUser(Guid userId, bool includePlaylists = false);
        IEnumerable<Playlist> GetPlaylistsFromUser(Guid userId);

        bool Save();
    }
}
