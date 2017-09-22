using System;
using Microsoft.AspNetCore.Mvc;
using Sprotify.Web.Models.Music;
using Sprotify.Web.Services;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Sprotify.Web.Controllers
{
    [Authorize]
    public class MusicController : Controller
    {
        private readonly PlaylistService _playlistService;

        public MusicController(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [Route("search")]
        public IActionResult Search()
        {
            return View();
        }

        [Route("browse")]
        public IActionResult Browse()
        {
            return View();
        }

        [Route("your-music")]
        public async Task<IActionResult> YourMusic()
        {
            var playlists = await _playlistService.GetPlaylists();
            return View(Mapper.Map<IEnumerable<PlaylistInYourMusic>>(playlists));
        }

        [Route("playlist/{id:guid}")]
        public async Task<IActionResult> Playlist([FromRoute]Guid id)
        {
            var playlist = await _playlistService.GetPlaylist(id);
            return View(Mapper.Map<PlaylistDetails>(playlist));
        }

        [Route("new-playlist")]
        public IActionResult NewPlaylist()
        {
            return View();
        }

        [Route("new-playlist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPlaylist(NewPlaylistModel model)
        {
            if (ModelState.IsValid)
            {
                var playlist = await _playlistService.CreatePlaylist(model.Title, model.Description);
                return RedirectToAction("Playlist", new { Id = playlist.Id });
            }

            return View(model);
        }

        [Route("playlist/{id:guid}/add-song")]
        public IActionResult AddSong(Guid id)
        {
            ViewData["playlistId"] = id;
            return View();
        }

        [Route("playlist/{id:guid}/add-song")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSong(Guid id, AddSongModel model)
        {
            if (ModelState.IsValid)
            {
                var song = await _playlistService.AddSongToPlaylist(id, model.Title, model.Band, model.Duration);
                return RedirectToAction("Playlist", new { Id = id });
            }

            ViewData["playlistId"] = id;
            return View(model);
        }
    }
}
