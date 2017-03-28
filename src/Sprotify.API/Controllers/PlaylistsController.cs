using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sprotify.API.Models;
using Sprotify.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprotify.API.Controllers
{
    [Route("api/playlists")]
    public class PlaylistsController : Controller
    {
        private readonly ISprotifyRepository _sprotifyRepository;

        public PlaylistsController(ISprotifyRepository sprotifyRepository)
        {
            _sprotifyRepository = sprotifyRepository;
        }
        
        [HttpGet]
        public IActionResult GetPlaylists()
        {
            var playlists = _sprotifyRepository.GetPlaylists();
            return Ok(Mapper.Map<IEnumerable<Playlist>>(playlists));
        }

        [HttpGet("{playlistId}")]
        public IActionResult GetPlaylist(Guid playlistId, [FromQuery] bool expand)
        {
            var playlist = _sprotifyRepository.GetPlaylist(playlistId, expand);

            if (playlist == null)
            {
                return NotFound();
            }

            if (expand)
            {
                var playlistWithSongsToReturn = Mapper.Map<PlaylistWithSongs>(playlist);
                return Ok(playlistWithSongsToReturn);
            }

            return Ok(Mapper.Map<Playlist>(playlist));
         }
    }
}
