using System.Linq;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.Domain.Models;
using FNO.FactoryPod.Exceptions;
using FNO.FactoryPod.Models;

namespace FNO.FactoryPod
{
    internal static class FactorioEventFactory
    {
        internal static IEvent TransformEvent(FactoryPodConfiguration config, PodEventDTO evnt)
        {
            var factoryId = config.Factorino.FactoryId;
            switch (evnt.Type)
            {
                case "on_built_entity":
                    return new FactoryBuiltEntityEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        Entity = evnt.Entity,
                    };
                case "on_entity_died":
                    return new FactoryEntityDiedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        Entity = evnt.Entity,
                    };
                case "on_player_joined_game":
                    return new FactoryPlayerJoinedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                    };
                case "on_player_mined_item":
                    return new FactoryPlayerMinedItemEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        ItemStack = evnt.ItemStack,
                    };
                case "on_player_mined_entity":
                    return new FactoryPlayerMinedEntityEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        Entity = evnt.Entity,
                    };
                case "on_player_died":
                    return new FactoryPlayerDiedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                    };
                case "on_player_left_game":
                    return new FactoryPlayerLeftEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                    };
                case "on_console_chat":
                    return new FactoryChatEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        Message = evnt.Message,
                    };
                case "on_research_started":
                    return new FactoryResearchStartedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        Technology = evnt.Technology,
                    };
                case "on_research_finished":
                    return new FactoryResearchFinishedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        Technology = evnt.Technology,
                    };
                case "on_rocket_launched":
                    return new FactoryRocketLaunchedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        Rocket = evnt.Entity,
                    };
                case "factorino_outgoing_train":
                    return new FactoryOutgoingTrainEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        TrainName = evnt.TrainName,
                        Inventory = evnt.Inventory.Select(i => (LuaItemStack)i).ToArray(),
                    };
                default:
                    throw new EventUnknownException($"Could not transform event with type {evnt.Type}!");
            }
        }

        internal static IEvent[] TransformEvents(FactoryPodConfiguration config, params PodEventDTO[] events)
        {
            return events.Select(e => TransformEvent(config, e)).ToArray();
        }
    }
}
