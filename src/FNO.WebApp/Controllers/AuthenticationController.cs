using AspNet.Security.OpenId.Steam;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FNO.WebApp.Controllers
{
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        private readonly IPlayerRepository _repo;
        private readonly IEventStore _eventStore;

        public AuthenticationController(IPlayerRepository repo, IEventStore eventStore)
        {
            _repo = repo;
            _eventStore = eventStore;
        }

        [HttpGet, Route("login")]
        public Task SignIn()
        {
            return HttpContext.ChallengeAsync(SteamAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = "/auth/register",
            });
        }

        [HttpGet, Route("register")]
        public async Task<IActionResult> RegisterSteamUser()
        {
            var authResult = await HttpContext.AuthenticateAsync("steam");
            if (!authResult.Succeeded)
            {
                // TODO: Should probably add an error here
                return RedirectToAction("Index", "Home");
            }

            var steamId = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var player = await _repo.GetPlayer(steamId);

            if (player != null)
            {
                return await SignInSteamUser(player);
            }
            
            player = new Player
            {
                SteamId = steamId,
                PlayerId = Guid.NewGuid(),
                Name = authResult.Principal.FindFirstValue(ClaimTypes.Name),
            };
            var evnt = new PlayerCreatedEvent(player);
            var result = await _eventStore.ProduceAsync(evnt);
            return await SignInSteamUser(player);
        }

        private async Task<IActionResult> SignInSteamUser(Player player)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, player.Name),
                new Claim(ClaimTypes.NameIdentifier, player.PlayerId.ToString()),
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Home");
        }
    }
}
