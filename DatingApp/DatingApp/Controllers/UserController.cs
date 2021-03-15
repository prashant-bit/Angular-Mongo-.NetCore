using DatingApp.DTOs;
using DatingApp.Interfaces;
using DatingApp.Models;
using DatingApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]") ]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepo;

        private readonly ITokenService _tokenService;

        public UserController(UserRepository userRepo,ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userRepo = userRepo;
        }
        [HttpPost("/api/user/register")]
        public async Task<ActionResult<UserDto>> AddUser([FromBody] RegisterDto user)
        {
            if (await _userRepo.UserExists(user.UserName))
                return BadRequest("User Name Taken");
            using var hmac = new HMACSHA512();

            var user_ = new User
            {
                UserName = user.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password)),
                PasswordSalt = hmac.Key
            };
            _userRepo.CreateAsync(user_);
            return new UserDto
            {
                UserName = user_.UserName,
                Token = _tokenService.CreateToken(user_)
            };
        }
        [HttpPost("/api/user/Login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto user)
        {
            var UserData = await _userRepo.GetUser(user.UserName);
            if (UserData == null) return Unauthorized("EITHER USERNAME OR PASSWORD IS WRONG");
            using var hmac = new HMACSHA512(UserData.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            for (int i = 0; i<computedHash.Length; i++)
            {
                if (computedHash[i] != UserData.PasswordHash[i]) return Unauthorized("EITHER USERNAME OR PASSWORD IS WRONG");
            }

            return new UserDto
            {
                UserName = UserData.UserName,
                Token = _tokenService.CreateToken(UserData)
            };
        }


    }
}
