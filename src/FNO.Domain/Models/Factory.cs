﻿using Newtonsoft.Json;
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
            get => TrainStationData == null ? null : JsonConvert.DeserializeObject<IEnumerable<string>>(TrainStationData);
            set => TrainStationData = value == null ? null : JsonConvert.SerializeObject(value);
        }

        public Guid DeedId { get; set; }
        public Deed Deed { get; set; }

        public Guid OwnerId { get; set; }
        public Player Owner { get; set; }

        public string OwnerFactorioUsername { get; set; }

        public string CurrentlyResearchingId { get; set; }
        public FactorioTechnology CurrentlyResearching { get; set; }

        // Other props useful for intermediate processing
        [NotMapped, JsonIgnore]
        public string ResourceId { get; set; }
    }
}
