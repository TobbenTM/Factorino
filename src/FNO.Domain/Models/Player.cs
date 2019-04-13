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

        public int Credits { get; set; }

        #region Steam props
        public string ProfileURL { get; set; }
        public string Avatar { get; set; }
        public string AvatarMedium { get; set; }
        public string AvatarFull { get; set; }
        #endregion

        [InverseProperty("Owner")]
        public IList<WarehouseInventory> WarehouseInventory { get; set; }

        public Guid? CorporationId { get; set; }
        public Corporation Corporation { get; set; }

        [InverseProperty("Player")]
        public IEnumerable<CorporationInvitation> Invitations { get; set; }

        [InverseProperty("Owner")]
        public IEnumerable<Factory> Factories { get; set; }

        [InverseProperty("Owner")]
        public IEnumerable<Shipment> Shipments { get; set; }

        [InverseProperty("Owner")]
        public IEnumerable<MarketOrder> Orders { get; set; }
    }
}
