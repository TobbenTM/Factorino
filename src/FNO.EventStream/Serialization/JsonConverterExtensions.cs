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

        public static IEvent DeserializeEvent(string evnt)
        {
            return DeserializeEvent<IEvent>(evnt);
        }

        public static TEvent DeserializeEvent<TEvent>(string evnt) where TEvent : IEvent
        {
            return JsonConvert.DeserializeObject<TEvent>(evnt, CreateSettings());
        }

        private static JsonSerializerSettings CreateSettings() => new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
        };
    }
}
