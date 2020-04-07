using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNO.Domain.Models
{
    public class Deed
    {
        public Guid DeedId { get; set; }

        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public Player Owner { get; set; }

        [InverseProperty(nameof(MapChunk.BelongsToDeed))]
        public IEnumerable<MapChunk> ChunksOwned { get; set; }
    }
}
