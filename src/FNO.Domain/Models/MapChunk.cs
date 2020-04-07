using System;

namespace FNO.Domain.Models
{
    public class MapChunk
    {
        public Guid MapChunkId { get; set; }

        public long X { get; set; }

        public long Y { get; set; }

        public string PreviewBase64 { get; set; }

        public string ResourceName { get; set; }

        public bool HasStation { get; set; }

        public Guid BelongsToDeedId { get; set; }

        public Deed BelongsToDeed { get; set; }
    }
}
