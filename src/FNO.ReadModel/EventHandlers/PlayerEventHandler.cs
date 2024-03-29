﻿using System.Linq;
using System.Threading.Tasks;
using FNO.Domain;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FNO.ReadModel.EventHandlers
{
    public sealed class PlayerEventHandler : EventHandlerBase,
        IEventHandler<PlayerCreatedEvent>,
        IEventHandler<PlayerInvitedToCorporationEvent>,
        IEventHandler<PlayerJoinedCorporationEvent>,
        IEventHandler<PlayerLeftCorporationEvent>,
        IEventHandler<PlayerRejectedInvitationEvent>,
        IEventHandler<PlayerBalanceChangedEvent>,
        IEventHandler<PlayerInventoryChangedEvent>,
        IEventHandler<PlayerFactorioIdChangedEvent>
    {
        private readonly ReadModelDbContext _dbContext;

        public PlayerEventHandler(ReadModelDbContext dbContext, ILogger logger) : base(logger)
        {
            _dbContext = dbContext;
        }

        public Task Handle(PlayerCreatedEvent evnt)
        {
            _dbContext.Players.Add(new Player
            {
                PlayerId = evnt.EntityId,
                Name = evnt.Name,
                SteamId = evnt.SteamId,
                ProfileURL = evnt.ProfileURL,
                Avatar = evnt.Avatar,
                AvatarMedium = evnt.AvatarMedium,
                AvatarFull = evnt.AvatarFull,
            });
            return Task.CompletedTask;
        }

        public Task Handle(PlayerInvitedToCorporationEvent evnt)
        {
            _dbContext.CorporationInvitations.Add(new CorporationInvitation
            {
                InvitationId = evnt.InvitationId,
                PlayerId = evnt.EntityId,
                CorporationId = evnt.CorporationId,
            });
            return Task.CompletedTask;
        }

        public Task Handle(PlayerJoinedCorporationEvent evnt)
        {
            var player = _dbContext.Players.FirstOrDefault(p => p.PlayerId == evnt.EntityId);
            if (player != null)
            {
                player.CorporationId = evnt.CorporationId;
                if (evnt.InvitationId != null)
                {
                    var invitation = _dbContext.CorporationInvitations.FirstOrDefault(i => i.InvitationId == evnt.InvitationId.Value);
                    if (invitation != null)
                    {
                        invitation.Accepted = true;
                        invitation.Completed = true;
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task Handle(PlayerLeftCorporationEvent evnt)
        {
            var player = _dbContext.Players.FirstOrDefault(p => p.PlayerId == evnt.EntityId);
            if (player != null && player.CorporationId == evnt.CorporationId)
            {
                player.CorporationId = null;
            }
            return Task.CompletedTask;
        }

        public Task Handle(PlayerRejectedInvitationEvent evnt)
        {
            var invitation = _dbContext.CorporationInvitations.FirstOrDefault(i => i.InvitationId == evnt.InvitationId);
            if (invitation != null)
            {
                invitation.Accepted = false;
                invitation.Completed = true;
            }
            return Task.CompletedTask;
        }

        public Task Handle(PlayerBalanceChangedEvent evnt)
        {
            var player = _dbContext.Players.FirstOrDefault(p => p.PlayerId == evnt.EntityId);
            if (player != null)
            {
                player.Credits += evnt.BalanceChange;
            }
            return Task.CompletedTask;
        }

        public Task Handle(PlayerInventoryChangedEvent evnt)
        {
            var player = _dbContext.Players
                .Include(p => p.WarehouseInventory)
                .FirstOrDefault(p => p.PlayerId == evnt.EntityId);
            if (player != null)
            {
                foreach (var stack in evnt.InventoryChange)
                {
                    var existingInventory = player.WarehouseInventory
                        .FirstOrDefault(i => i.ItemId == stack.Name);

                    if (existingInventory != null)
                    {
                        existingInventory.Quantity += stack.Count;
                    }
                    else
                    {
                        player.WarehouseInventory.Add(new WarehouseInventory
                        {
                            ItemId = stack.Name,
                            Quantity = stack.Count,
                        });
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task Handle(PlayerFactorioIdChangedEvent evnt)
        {
            var player = _dbContext.Players.FirstOrDefault(p => p.PlayerId == evnt.EntityId);
            if (player != null)
            {
                player.FactorioId = evnt.FactorioId;
            }
            return Task.CompletedTask;
        }
    }
}
