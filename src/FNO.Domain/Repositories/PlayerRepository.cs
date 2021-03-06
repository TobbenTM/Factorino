﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FNO.Domain.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ReadModelDbContext _dbContext;

        public PlayerRepository(ReadModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Player> GetPlayer(string steamId)
        {
            return _dbContext.Players
                .FirstOrDefaultAsync(p => p.SteamId.Equals(steamId));
        }

        public Task<Player> GetPlayer(Guid playerId)
        {
            return _dbContext.Players
                .FirstOrDefaultAsync(p => p.PlayerId.Equals(playerId));
        }

        public Task<Player> GetPlayer(ClaimsPrincipal user)
        {
            var id = Guid.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return GetPlayer(id);
        }

        public Task<List<Player>> GetWealthiestPlayers()
        {
            return _dbContext.Players
                .OrderByDescending(p => p.Credits)
                .Take(50)
                .ToListAsync();
        }

        public async Task<IEnumerable<CorporationInvitation>> GetInvitations(ClaimsPrincipal user)
        {
            var player = await GetPlayer(user);
            return player.Invitations;
        }

        // TODO: Refactor this to not await the task, just return it
        public async Task<IEnumerable<WarehouseInventory>> GetInventory(Player player)
        {
            return await _dbContext.WarehouseInventories
                .Include(i => i.Item)
                .Where(i => i.OwnerId == player.PlayerId)
                .ToListAsync();
        }
    }
}
