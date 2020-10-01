﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationPlugin;
using CinemaApi.Data;
using CinemaApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public UsersController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }
        [HttpPost]
        public IActionResult Register([FromBody] User user )
        {
          var userWithSameEmail=  _db.Users.Where(u => u.Email == user.Email).SingleOrDefault();
            if (userWithSameEmail!=null)
            {
                return BadRequest("User Already Exists with this Email");
            }
            var userObj = new User
            {
                Name=user.Name,
                Email=user.Email,
                Password= SecurePasswordHasherHelper.Hash(user.Password),
                Role="Users"
            };
            _db.Users.Add(userObj);
            _db.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var userEmail = _db.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userEmail==null)
            {
                return NotFound();
            }
            if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password))
            {
                return Unauthorized();
            }
            var claims = new[]
                {
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   new Claim(ClaimTypes.Email, user.Email),
                   new Claim(ClaimTypes.Role,userEmail.Role)
                 };
            var token = _auth.GenerateAccessToken(claims);

            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id=userEmail.Id
            });

        }
    }
}
