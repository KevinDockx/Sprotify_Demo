using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sprotify.API.Models;
using Sprotify.API.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Sprotify.API.Controllers
{
    [Authorize]
    [Route("api/songs")]
    public class SongsController : Controller
    {
        private readonly ISprotifyRepository _sprotifyRepository;

        public SongsController(ISprotifyRepository sprotifyRepository)
        {
            _sprotifyRepository = sprotifyRepository;
        }

        [HttpGet]
        public IActionResult GetSongs([FromQuery]string search = null)
        {
            var songs = _sprotifyRepository.GetSongs(search);
            return Ok(Mapper.Map<IEnumerable<Song>>(songs));
        }

        [HttpGet("{songId}", Name = "GetSong")]
        public IActionResult GetSong(Guid songId)
        {
            var song = _sprotifyRepository.GetSong(songId);
            if (song == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Song>(song));
        }

        [HttpGet("~/api/playlists/{playlistId}/songs")]
        public IActionResult GetSongsFromPlaylist(Guid playlistId)
        {
            if (!_sprotifyRepository.PlaylistExists(playlistId))
            {
                return NotFound();
            }

            var songs = _sprotifyRepository.GetSongsFromPlaylist(playlistId);
            return Ok(Mapper.Map<IEnumerable<SongInPlaylist>>(songs));
        }

        [HttpGet("~/api/playlists/{playlistId}/songs/{songId}", Name = "GetSongFromPlaylist")]
        public IActionResult GetSongFromPlaylist(Guid playlistId, Guid songId)
        {
            if (!_sprotifyRepository.PlaylistExists(playlistId))
            {
                return NotFound();
            }

            var song = _sprotifyRepository.GetSongFromPlaylist(playlistId, songId);

            if (song == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<SongInPlaylist>(song));
        }

        [HttpPost]
        public IActionResult CreateSong([FromBody] SongForCreation songForCreation)
        {
            if (songForCreation == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var mappedSong = Mapper.Map<Entities.Song>(songForCreation);

            _sprotifyRepository.AddSong(mappedSong);

            if (!_sprotifyRepository.Save())
            {
                throw new Exception("Adding a song failed.");
            }

            var createdSongToReturn = Mapper.Map<Models.Song>(mappedSong);

            return CreatedAtRoute("GetSong",
                new
                {
                    songId = mappedSong.Id
                },
                createdSongToReturn);
        }

        [HttpPut("{songId}")]
        public IActionResult UpdateSong(Guid songId, [FromBody] SongForUpdate songForUpdate)
        {
            if (songForUpdate == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            
            var song = _sprotifyRepository.GetSong(songId);
            if (song == null)
            {
                return NotFound();
            }

            Mapper.Map(songForUpdate, song);

            _sprotifyRepository.UpdateSong(song);

            if (!_sprotifyRepository.Save())
            {
                throw new Exception($"Updating song {songId} failed.");
            }

            return NoContent();
        }

        [HttpPatch("{songId}")]
        public IActionResult PartiallyUpdateSong(Guid songId,
            [FromBody] JsonPatchDocument<SongForUpdate> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var song = _sprotifyRepository.GetSong(songId);
            if (song == null)
            {
                return NotFound();
            }

            var songToPatch = Mapper.Map<SongForUpdate>(song);
            patchDoc.ApplyTo(songToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            TryValidateModel(songToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            Mapper.Map(songToPatch, song);

            _sprotifyRepository.UpdateSong(song);

            if (!_sprotifyRepository.Save())
            {
                throw new Exception($"Updating song {songId} failed.");
            }

            return NoContent();
        }

        [HttpPost("~/api/playlists/{playlistId}/songs")]
        public IActionResult AddSongToPlaylist(Guid playlistId, [FromBody] SongForPlaylist songForPlaylist)
        {
            if (songForPlaylist == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!_sprotifyRepository.PlaylistExists(playlistId))
            {
                return NotFound();
            }

            var mappedSong = Mapper.Map<Entities.PlaylistSong>(songForPlaylist);

            _sprotifyRepository.AddSongToPlaylist(playlistId, mappedSong.SongId, mappedSong.Index);

            if (!_sprotifyRepository.Save())
            {
                throw new Exception($"Adding a song to playlist {playlistId} failed.");
            }

            var addedSongToReturn = Mapper.Map<Models.SongInPlaylist>(_sprotifyRepository.GetSongFromPlaylist(playlistId, mappedSong.SongId));

            return CreatedAtRoute("GetSongFromPlaylist",
                new
                {
                    playlistId = addedSongToReturn.PlaylistId,
                    songId = addedSongToReturn.Id
                },
                addedSongToReturn);
        }

        [HttpPut("~/api/playlists/{playlistId}/songs/{songId}")]
        public IActionResult UpdateSong(Guid playlistId, Guid songId,
            [FromBody] MoveSongInPlaylist moveSongInPlaylist)
        {
            if (moveSongInPlaylist == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!_sprotifyRepository.PlaylistExists(playlistId))
            {
                return NotFound();
            }

            var song = _sprotifyRepository.GetSongFromPlaylist(playlistId, songId);
            if (song == null)
            {
                return NotFound();
            }

            if (moveSongInPlaylist.OldIndex == moveSongInPlaylist.NewIndex)
            {
                return NoContent();
            }

            _sprotifyRepository.UpdateSongInPlaylist(playlistId, songId, moveSongInPlaylist.OldIndex, moveSongInPlaylist.NewIndex);

            if (!_sprotifyRepository.Save())
            {
                throw new Exception($"Updating song {songId} from playlist {playlistId} failed.");
            }

            return NoContent();
        }

        [HttpDelete("~/api/playlists/{playlistId}/songs/{songId}")]
        public IActionResult DeleteSongFromPlaylist(Guid playlistId, Guid songId)
        {
            if (!_sprotifyRepository.PlaylistExists(playlistId))
            {
                return NotFound();
            }

            var song = _sprotifyRepository.GetSongFromPlaylist(playlistId, songId);
            if (song == null)
            {
                return NotFound();
            }

            _sprotifyRepository.RemoveSongFromPlaylist(song.PlaylistId, song.Id, song.Index);

            if (!_sprotifyRepository.Save())
            {
                throw new Exception($"Deleting song {songId} from playlist {playlistId} failed.");
            }
             
            return NoContent();
        }
    }
}
