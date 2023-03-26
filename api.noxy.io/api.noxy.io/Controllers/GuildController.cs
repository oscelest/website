﻿using api.noxy.io.Interface;
using api.noxy.io.Models;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace api.noxy.io.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuildController : ControllerBase
    {
        private readonly IJWT _jwt;
        private readonly IUserRepository _user;
        private readonly IGameRepository _guild;

        public GuildController(IJWT jwt, IUserRepository user, IGameRepository guild)
        {
            _jwt = jwt;
            _user = user;
            _guild = guild;
        }

        [HttpGet("Self")]
        public async Task<ActionResult<GuildEntity.DTO?>> Self()
        {
            JwtSecurityToken? token = JWT.ReadToken(await HttpContext.GetTokenAsync("access_token"));
            if (token == null) return Unauthorized();

            UserEntity? user = await _user.FindByID(_jwt.GetUserID(token));
            if (user == null) return Unauthorized();

            GuildEntity? guild = await _guild.FindByUser(user);
            return Ok(guild?.ToDTO());
        }

        [HttpPost("Register")]
        public async Task<ActionResult<GuildEntity.DTO>> Register(GuildRegisterRequest input)
        {
            JwtSecurityToken? token = JWT.ReadToken(await HttpContext.GetTokenAsync("access_token"));
            if (token == null) return Unauthorized();

            UserEntity? user = await _user.FindByID(_jwt.GetUserID(token));
            if (user == null) return Unauthorized();

            GuildEntity? guild = await _guild.FindByUser(user);
            if (guild != null) return Conflict();

            guild = await _guild.Create(input.Name, user);

            return Ok(guild.ToDTO());
        }

        [HttpPost("RefreshUnitList")]
        public async Task<ActionResult<GuildEntity.DTO>> RefreshUnitList()
        {
            JwtSecurityToken? token = JWT.ReadToken(await HttpContext.GetTokenAsync("access_token"));
            if (token == null) return Unauthorized();

            UserEntity? user = await _user.FindByID(_jwt.GetUserID(token));
            if (user == null) return Unauthorized();

            return Ok((await _guild.RefreshUnitList(user)).ToDTO());
        }

        [HttpPost("RefreshMissionList")]
        public async Task<ActionResult<GuildEntity.DTO>> RefreshMissionList()
        {
            JwtSecurityToken? token = JWT.ReadToken(await HttpContext.GetTokenAsync("access_token"));
            if (token == null) return Unauthorized();

            UserEntity? user = await _user.FindByID(_jwt.GetUserID(token));
            if (user == null) return Unauthorized();

            return Ok((await _guild.RefreshMissionList(user)).ToDTO());
        }


        public class GuildRegisterRequest
        {
            [Required(AllowEmptyStrings = false)]
            [StringLength(64, MinimumLength = 3)]
            public string Name { get; set; } = string.Empty;
        }
    }
}
