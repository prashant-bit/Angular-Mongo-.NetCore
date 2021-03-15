using DatingApp.DTOs;
using DatingApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    public class UserDisplayController : Controller
    {
        private readonly UserDisplayRepository _userRepo;


        public UserDisplayController(UserDisplayRepository userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpGet("/api/userdisplay/users")]
        public async Task<ActionResult<UserDisplayDto>> GetUsers()
        {
            var result = await _userRepo.GetUsersAsync();
            return Ok(result);
        }
    }
}
