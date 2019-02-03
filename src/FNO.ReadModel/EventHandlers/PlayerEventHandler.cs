using System;
using System.Collections.Generic;
using FNO.Domain;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.EventSourcing;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace FNO.ReadModel.EventHandlers
{
    public sealed class PlayerEventHandler : EventHandlerBase,
        IEventHandler<PlayerCreatedEvent>,
        IEventHandler<PlayerInvitedToCorporationEvent>,
        IEventHandler<PlayerJoinedCorporationEvent>,
        IEventHandler<PlayerLeftCorporationEvent>,
        IEventHandler<PlayerRejectedInvitationEvent>
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
    }
}
