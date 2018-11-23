using FNO.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace FNO.WebApp.Models
{
    public class PlayerUser : IdentityUser
    {
        public Player Player { get; set; }
    }
}
