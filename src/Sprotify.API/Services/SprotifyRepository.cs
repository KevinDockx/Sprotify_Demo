using Microsoft.EntityFrameworkCore;
using Sprotify.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Sprotify.API.Services.Models;

namespace Sprotify.API.Services
{
    public class SprotifyRepository : ISprotifyRepository, IDisposable
    {
        private SprotifyContext _context;
        public SprotifyRepository(SprotifyContext context)
        {
            _context = context;
        }

        public void CreatePlaylist(Guid ownerId, Playlist playlist)
        {
            playlist.Id = Guid.NewGuid();
            playlist.OwnerId = ownerId;

            _context.Playlists.Add(playlist);
        }

        public PlaylistWithSongs GetPlaylist(Guid playlistId, bool includeSongs = false)
        {
            if (includeSongs)
            {
                return _context.Playlists
                    .Where(x => x.Id == playlistId)
                    .Select(x => new PlaylistWithSongs
                    {
                        Id = x.Id,
                        OwnerId = x.OwnerId,
                        Title = x.Title,
                        Description = x.Description,
                        Songs = x.Songs.OrderBy(s => s.Index).Select(s => new SongWithPlaylistInfo
                        {
                            Id = s.Song.Id,
                            Title = s.Song.Title,
                            Band = s.Song.Band,
                            Duration = s.Song.Duration,
                            Index = s.Index,
                            PlaylistId = s.PlaylistId
                        }).ToList()
                    })
                    .FirstOrDefault();
            }

            return _context.Playlists
                .Select(x => new PlaylistWithSongs
                {
                    Id = x.Id,
                    OwnerId = x.OwnerId,
                    Title = x.Title,
                    Description = x.Description
                })
                .FirstOrDefault(c => c.Id == playlistId);
        }

        public IEnumerable<Playlist> GetPlaylists()
        {
            return _context.Playlists.ToList();
        }

        public IEnumerable<Song> GetSongs(string search)
        {
            var query = _context.Songs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Band.Contains(search) || x.Title.Contains(search));
            }

            return query.ToList();
        }

        public Song GetSong(Guid songId)
        {
            return _context.Songs.FirstOrDefault(x => x.Id == songId);
        }

        public SongWithPlaylistInfo GetSongFromPlaylist(Guid playlistId, Guid songId)
        {
            return _context.Playlists
                .Where(x => x.Id == playlistId)
                .SelectMany(x => x.Songs)
                .Select(y => new SongWithPlaylistInfo
                {
                    Id = y.SongId,
                    Band = y.Song.Band,
                    Duration = y.Song.Duration,
                    Title = y.Song.Title,
                    PlaylistId = y.PlaylistId,
                    Index = y.Index
                })
                .FirstOrDefault(x => x.Id == songId);
        }

        public IEnumerable<SongWithPlaylistInfo> GetSongsFromPlaylist(Guid playlistId)
        {
            return _context.Playlists.Where(x => x.Id == playlistId)
                .SelectMany(x => x.Songs)
                .OrderBy(x => x.Index)
                .Select(y => new SongWithPlaylistInfo
                {
                    Id = y.SongId,
                    Band = y.Song.Band,
                    Duration = y.Song.Duration,
                    Title = y.Song.Title,
                    PlaylistId = y.PlaylistId,
                    Index = y.Index
                })
                .ToList();
        }

        public bool PlaylistExists(Guid playlistId)
        {
            return _context.Playlists.Any(p => p.Id == playlistId);
        }

        public void AddSong(Song song)
        {
            _context.Songs.Add(song);
        }

        public void UpdateSong(Song song)
        {
            // no code in this implementation
        }

        public void DeleteSong(Song song)
        {
            _context.Songs.Remove(song);
        }

        public void AddSongToPlaylist(Guid playlistId, Guid songId, int index)
        {
            var playlist = _context.Playlists.FirstOrDefault(x => x.Id == playlistId);
            playlist.Songs.Add(new PlaylistSong
            {
                PlaylistId = playlist.Id,
                SongId = songId,
                Index = index
            });
        }

        public void UpdateSongInPlaylist(Guid playlistId, Guid songId, int oldIndex, int newIndex)
        {
            var playlist = _context.Playlists.Include(x => x.Songs)
                .FirstOrDefault(x => x.Id == playlistId);

            var song = playlist.Songs.FirstOrDefault(x => x.SongId == songId && x.Index == oldIndex);
            playlist.Songs.Remove(song);

            // Reindex the other songs, this might however fail since Index is part of the PK
            for (var i = 1; i < newIndex && i < playlist.Songs.Count; i++)
            {
                if (i < newIndex)
                {
                    playlist.Songs.ElementAt(i - 1).Index = i;
                }
                else if (i >= newIndex)
                {
                    playlist.Songs.ElementAt(i - 1).Index = i + 1;
                }
            }

            song.Index = newIndex;
            playlist.Songs.Add(song);
        }

        public void RemoveSongFromPlaylist(Guid playlistId, Guid songId, int index)
        {
            var playlist = _context.Playlists.Include(x => x.Songs)
                .FirstOrDefault(x => x.Id == playlistId);

            var song = playlist.Songs.FirstOrDefault(x => x.SongId == songId && x.Index == index);
            playlist.Songs.Remove(song);

            // Reindex the other songs, this might however fail since Index is part of the PK
            var newIdx = 1;
            foreach (var s in playlist.Songs)
            {
                s.Index = newIdx++;
            }
        }

        public bool UserExists(Guid userId)
        {
            return _context.Users.Any(x => x.Id == userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(Guid userId, bool includePlaylists = false)
        {
            var query = _context.Users.Where(x => x.Id == userId);
            if (includePlaylists)
            {
                query = query.Include(x => x.Playlists);
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<Playlist> GetPlaylistsFromUser(Guid userId)
        {
            return _context.Playlists.Where(x => x.OwnerId == userId).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}

