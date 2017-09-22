using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sprotify.API.Models;
using Sprotify.API.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Sprotify.API.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly ISprotifyRepository _repository;

        public UsersController(ISprotifyRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _repository.GetUsers();
            return Ok(Mapper.Map<IEnumerable<User>>(users));
        }

        [HttpGet("{id:guid}", Name = "GetUser")]
        public IActionResult GetUser(Guid id, [FromQuery] bool expand)
        {
            var user = _repository.GetUser(id, expand);
            if (user == null)
            {
                return NotFound();
            }

            if (expand)
            {
                return Ok(Mapper.Map<UserWithPlaylists>(user));
            }

            return Ok(Mapper.Map<User>(user));
        }
    }
}