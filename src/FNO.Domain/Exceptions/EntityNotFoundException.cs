using System;
using System.Runtime.Serialization;

namespace FNO.Domain.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Guid entityId) : base($"Could not find entity with ID {entityId}!")
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
