using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNO.Domain.Models
{
    public class Player
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public string SteamId { get; set; }
        public string FactorioId { get; set; }

        public Guid? CorporationId { get; set; }
        public Corporation Corporation { get; set; }

        [InverseProperty("Player")]
        public IEnumerable<CorporationInvitation> Invitations { get; set; }
    }
}
