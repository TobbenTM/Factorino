using System;
using System.ComponentModel.DataAnnotations;

namespace FNO.Domain.Models
{
    public class Factory
    {
        [Key]
        public Guid FactoryId { get; set; }
        public string Name { get; set; }

        public int Port { get; set; }
        public long LastSeen { get; set; }
        public int PlayersOnline { get; set; }

        public Guid OwnerId { get; set; }
        public Player Owner { get; set; }

        public string CurrentlyResearchingId { get; set; }
        public FactorioTechnology CurrentlyResearching { get; set; }
    }
}
