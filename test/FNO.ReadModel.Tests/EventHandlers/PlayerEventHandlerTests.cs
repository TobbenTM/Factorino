﻿using FNO.Domain.Events.Corporation;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FNO.ReadModel.Tests.EventHandlers
{
    public class PlayerEventHandlerTests : EventHandlerTestBase
    {
        [Fact]
        public async Task ShouldAddPlayer()
        {
            // Arrange
            var expectedPlayer = new Player
            {
                PlayerId = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                SteamId = Guid.NewGuid().ToString(),
            };

            // Act
            await When(new PlayerCreatedEvent(expectedPlayer));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Players);
                var player = dbContext.Players.First();
                Assert.Equal(expectedPlayer.PlayerId, player.PlayerId);
                Assert.Equal(expectedPlayer.Name, player.Name);
                Assert.Equal(expectedPlayer.SteamId, player.SteamId);
            }
        }

        [Fact]
        public async Task ShouldAddCorporationInvitation()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var expectedInvitation = new CorporationInvitation
            {
                InvitationId = Guid.NewGuid(),
                PlayerId = playerId,
                CorporationId = Guid.NewGuid(),
            };

            // Act
            await When(new PlayerCreatedEvent(new Player { PlayerId = playerId }));
            await When(new PlayerInvitedToCorporationEvent(expectedInvitation.PlayerId, expectedInvitation.CorporationId, expectedInvitation.InvitationId, null));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.CorporationInvitations);
                var invitation = dbContext.CorporationInvitations.First();
                Assert.Equal(expectedInvitation.InvitationId, invitation.InvitationId);
                Assert.Equal(expectedInvitation.PlayerId, invitation.PlayerId);
                Assert.Equal(expectedInvitation.CorporationId, invitation.CorporationId);
            }
        }
        
        [Fact]
        public async Task ShouldRejectInvitation()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var expectedInvitation = new CorporationInvitation
            {
                InvitationId = Guid.NewGuid(),
                PlayerId = playerId,
                CorporationId = Guid.NewGuid(),
            };

            // Act
            await When(new PlayerCreatedEvent(new Player { PlayerId = playerId }));
            await When(new PlayerInvitedToCorporationEvent(expectedInvitation.PlayerId, expectedInvitation.CorporationId, expectedInvitation.InvitationId, null));
            await When(new PlayerRejectedInvitationEvent(expectedInvitation.PlayerId, expectedInvitation.InvitationId, null));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.CorporationInvitations);
                var invitation = dbContext.CorporationInvitations.First();
                Assert.True(invitation.Completed);
                Assert.False(invitation.Accepted);
            }
        }

        [Fact]
        public async Task ShouldAcceptInvitation()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var corporationId = Guid.NewGuid();
            var expectedInvitation = new CorporationInvitation
            {
                InvitationId = Guid.NewGuid(),
                PlayerId = playerId,
                CorporationId = corporationId,
            };

            // Act
            await When(new PlayerCreatedEvent(new Player { PlayerId = playerId }));
            await When(new CorporationCreatedEvent(new Corporation { CorporationId = corporationId, CreatedByPlayerId = playerId }, null));
            await When(new PlayerInvitedToCorporationEvent(expectedInvitation.PlayerId, expectedInvitation.CorporationId, expectedInvitation.InvitationId, null));
            await When(new PlayerJoinedCorporationEvent(expectedInvitation.PlayerId, expectedInvitation.CorporationId, null, expectedInvitation.InvitationId));

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.CorporationInvitations);
                var invitation = dbContext.CorporationInvitations.First();
                Assert.True(invitation.Completed);
                Assert.True(invitation.Accepted);
                var player = dbContext.Players.First();
                Assert.Equal(corporationId, player.CorporationId);
            }
        }

        [Fact]
        public async Task ShouldUpdateBalance()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var expectedBalance = new Random().Next();

            // Act
            await When(new PlayerCreatedEvent(new Player { PlayerId = playerId }));
            await When(new PlayerBalanceChangedEvent(playerId, null)
            {
                BalanceChange = expectedBalance,
            });

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Players);
                var player = dbContext.Players.First();
                Assert.Equal(expectedBalance, player.Credits);
            }
        }

        [Fact]
        public async Task ShouldUpdateInventory()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var expectedItem = Domain.Seed.EntityLibrary.Data()[0];
            var expectedBalance = new Random().Next();

            // Act
            await When(new PlayerCreatedEvent(new Player { PlayerId = playerId }));
            await When(new PlayerInventoryChangedEvent(playerId, null)
            {
                InventoryChange = new[]
                {
                    new LuaItemStack
                    {
                        Name = expectedItem.Name,
                        Count = expectedBalance,
                    },
                },
            });

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Players);
                var player = dbContext.Players
                    .Include(p => p.WarehouseInventory)
                    .First();
                Assert.Equal(expectedBalance, player.WarehouseInventory.Single().Quantity);
            }
        }

        [Fact]
        public async Task ShouldUpdateFactorioId()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var expectedUsername = Guid.NewGuid().ToString();

            // Act
            await When(new PlayerCreatedEvent(new Player { PlayerId = playerId }));
            await When(new PlayerFactorioIdChangedEvent(playerId, null)
            {
                FactorioId = expectedUsername,
            });

            // Assert
            using (var dbContext = GetInMemoryDatabase())
            {
                Assert.NotEmpty(dbContext.Players);
                var player = dbContext.Players.Single();
                Assert.Equal(expectedUsername, player.FactorioId);
            }
        }
    }
}
