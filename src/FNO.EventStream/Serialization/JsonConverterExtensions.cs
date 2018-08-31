using FNO.Domain.Events;
using Newtonsoft.Json;

namespace FNO.EventStream.Serialization
{
    public static class JsonConverterExtensions
    {
        public static string SerializeEvent(IEvent evnt)
        {
            return JsonConvert.SerializeObject(evnt, CreateSettings());
        }

        public static TEvent DeserializeEvent<TEvent>(string evnt) where TEvent : IEvent
        {
            return JsonConvert.DeserializeObject<TEvent>(evnt, CreateSettings());
        }

        public static JsonSerializerSettings CreateSettings() => new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            // NullValueHandling = NullValueHandling.Ignore,
        };
    }
}
