using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sprotify.API.Models;
using Sprotify.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprotify.API.Controllers
{
    [Route("api/playlists/{playlistId}/songs")]
    public class SongsController : Controller
    {
        private readonly ISprotifyRepository _sprotifyRepository;

        public SongsController(ISprotifyRepository sprotifyRepository)
        {
            _sprotifyRepository = sprotifyRepository;
        }

        [HttpGet]
        public IActionResult GetSongsFromPlaylist(Guid playlistId)
        {
            if (!_sprotifyRepository.PlaylistExists(playlistId))
            {
                return NotFound();
            }

            var songs = _sprotifyRepository.GetSongsFromPlaylist(playlistId);
            return Ok(Mapper.Map<IEnumerable<Song>>(songs));
        }

        [HttpGet("{songId}", Name = "GetSongFromPlaylist")]
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

            return Ok(Mapper.Map<Song>(song));
        }

        [HttpPost]
        public IActionResult AddSongToPlaylist(Guid playlistId, 
            [FromBody] SongForCreation songForCreation)
        {
            if (songForCreation == null)
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

            var mappedSong = Mapper.Map<Entities.Song>(songForCreation);

            _sprotifyRepository.AddSongToPlaylist(playlistId, mappedSong);

            if (!_sprotifyRepository.Save())
            {
                throw new Exception($"Adding a song to playlist {playlistId} failed.");
            }

            var createdSongToReturn = Mapper.Map<Models.Song>(mappedSong);

            return CreatedAtRoute("GetSongFromPlaylist", 
                new {
                playlistId = mappedSong.PlaylistId,
                songId = mappedSong.Id }, 
                createdSongToReturn);
        }


        [HttpPut("{songId}")]
        public IActionResult UpdateSong(Guid playlistId, Guid songId,
            [FromBody] SongForUpdate songForUpdate)
        {
            if (songForUpdate == null)
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

            Mapper.Map(songForUpdate, song);

            _sprotifyRepository.UpdateSong(song);

            if (!_sprotifyRepository.Save())
            {
                throw new Exception($"Updating song {songId} from playlist {playlistId} failed.");
            }

            return NoContent();
        }

        [HttpPatch("{songId}")]
        public IActionResult PartiallyUpdateSong(Guid playlistId, Guid songId,
            [FromBody] JsonPatchDocument<SongForUpdate> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
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
                throw new Exception($"Updating song {songId} from playlist {playlistId} failed.");
            }

            return NoContent();
        }

        [HttpDelete("{songId}")]
        public IActionResult DeleteSong(Guid playlistId, Guid songId)
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

            _sprotifyRepository.DeleteSong(song);

            if (!_sprotifyRepository.Save())
            {
                throw new Exception($"Deleting song {songId} from playlist {playlistId} failed.");
            }
             
            return NoContent();
        }
    }
}
