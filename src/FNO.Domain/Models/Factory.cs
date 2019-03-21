using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNO.Domain.Models
{
    public class Factory
    {
        [Key]
        public Guid FactoryId { get; set; }
        public string Name { get; set; }

        public FactoryState State { get; set; }
        public int Port { get; set; }
        public long LastSeen { get; set; }
        public int PlayersOnline { get; set; }

        // TODO: Refactor this hack please
        [JsonIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TrainStationData { get; set; }
        [NotMapped]
        public IEnumerable<string> TrainStations
        {
            get => JsonConvert.DeserializeObject<IEnumerable<string>>(TrainStationData);
            set => TrainStationData = JsonConvert.SerializeObject(value);
        }

        // The location seed is kept on entity as well (location might change, seed will not)
        public string Seed { get; set; }

        public Guid LocationId { get; set; }
        public FactoryLocation Location { get; set; }

        public Guid OwnerId { get; set; }
        public Player Owner { get; set; }

        public string CurrentlyResearchingId { get; set; }
        public FactorioTechnology CurrentlyResearching { get; set; }

        // Other props useful for intermediate processing
        [NotMapped, JsonIgnore]
        public string ResourceId { get; set; }
    }
}
