using System;
using System.Collections.Generic;

namespace Sprotify.API.Models
{
    public class UserWithPlaylists
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}