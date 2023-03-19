﻿using api.noxy.io.Interface;
using api.noxy.io.Models;
using api.noxy.io.Models.Game;
using api.noxy.io.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace api.noxy.io.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GameHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IGuildRepository _guildRepository;

        public GameHub(IUserRepository userRepository, IGuildRepository guildRepository)
        {
            _userRepository = userRepository;
            _guildRepository = guildRepository;
        }

        public async Task Load()
        {
            UserEntity user = await GetUser((ClaimsIdentity)Context.User!.Identity!);
            GuildEntity? guild = await _guildRepository.FindByUser(user);
            if (guild == null)
            {
                await Clients.Caller.SendAsync("Load", null);
            }
            else
            {
                await Clients.Caller.SendAsync("Load", guild.ToDTO());
            }
        }

        public async Task CreateGuild(string name)
        {
            UserEntity user = await GetUser((ClaimsIdentity)Context.User!.Identity!);
            GuildEntity? guild = await _guildRepository.FindByUser(user);
            if (guild == null)
            {
                guild = await _guildRepository.Create(name, user);
                await Clients.Caller.SendAsync("CreateGuild", guild.ToDTO());
            }
            else
            {
                throw new HubException("Forbidden");
            }

        }

        private async Task<UserEntity> GetUser(ClaimsIdentity identity)
        {
            return await _userRepository.FindByID(new JWT(identity).UserID) ?? throw new HubException("Unauthorized");
        }
    }
}