using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenId.Steam;
using FNO.Domain.Events;
using FNO.Domain.Events.Player;
using FNO.Domain.Models;
using FNO.Domain.Repositories;
using FNO.EventSourcing;
using FNO.WebApp.Filters;
using FNO.WebApp.Models.Steam;
using FNO.WebApp.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace FNO.WebApp.Controllers
{
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        private readonly IPlayerRepository _repo;
        private readonly IEventStore _eventStore;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AuthenticationController(
            IPlayerRepository repo,
            IEventStore eventStore,
            IConfiguration configuration,
            ILogger logger)
        {
            _repo = repo;
            _eventStore = eventStore;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("login")]
        public Task SignIn()
        {
            return HttpContext.ChallengeAsync(SteamAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = "/auth/register",
            });
        }

        [HttpGet("register")]
        [EnsureConsumerConsistency]
        public async Task<IActionResult> RegisterSteamUser()
        {
            var authResult = await HttpContext.AuthenticateAsync(AuthenticationConfiguration.SteamCookieScheme);
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

            // TODO: Refactor this into a dedicated client for easier testing
            using (var client = new HttpClient())
            {
                try
                {
                    var key = _configuration["Authentication:Steam:AppKey"];
                    var url = $"api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={steamId.Replace("https://steamcommunity.com/openid/id/", "")}";
                    var response = await client.GetAsync("https://" + url);
                    var body = await response.Content.ReadAsStringAsync();
                    var payload = JsonConvert.DeserializeObject<SteamApiResponseWrapper<GetPlayerSummariesResponse>>(body);
                    var steamplayer = payload.Response.Players.FirstOrDefault();
                    player.ProfileURL = steamplayer.ProfileURL;
                    player.Avatar = steamplayer.Avatar;
                    player.AvatarFull = steamplayer.AvatarFull;
                    player.AvatarMedium = steamplayer.AvatarMedium;
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Could not fetch player summary for player {steamId}! Error: {e.Message}");
                    // TODO: Should probably add an error response here
                    return RedirectToAction("Index", "Home");
                }
            }

            var evnt = new PlayerCreatedEvent(player);
            HttpContext.Items[nameof(IEvent)] = evnt;
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
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign out of the intermediate Steam session..
            await HttpContext.SignOutAsync(AuthenticationConfiguration.SteamCookieScheme);
            // ..and into the more permanent cookie scheme
            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Home");
        }
    }
}
