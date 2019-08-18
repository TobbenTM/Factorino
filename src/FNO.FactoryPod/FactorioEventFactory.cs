using System;
using System.Collections.Generic;
using System.Linq;
using FNO.Domain.Events;
using FNO.Domain.Events.Factory;
using FNO.Domain.Events.Shipping;
using FNO.Domain.Models;
using FNO.FactoryPod.Exceptions;
using FNO.FactoryPod.Models;

namespace FNO.FactoryPod
{
    internal static class FactorioEventFactory
    {
        internal static IEnumerable<IEvent> TransformEvent(FactoryPodConfiguration config, PodEventDTO evnt)
        {
            var factoryId = config.Factorino.FactoryId;
            switch (evnt.Type)
            {
                case "on_built_entity":
                    yield return new FactoryBuiltEntityEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        Entity = evnt.Entity,
                    };
                    break;
                case "on_entity_died":
                    yield return new FactoryEntityDiedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        Entity = evnt.Entity,
                    };
                    break;
                case "on_player_joined_game":
                    yield return new FactoryPlayerJoinedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                    };
                    break;
                case "on_player_mined_item":
                    yield return new FactoryPlayerMinedItemEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        ItemStack = evnt.ItemStack,
                    };
                    break;
                case "on_player_mined_entity":
                    yield return new FactoryPlayerMinedEntityEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        Entity = evnt.Entity,
                    };
                    break;
                case "on_player_died":
                    yield return new FactoryPlayerDiedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                    };
                    break;
                case "on_player_left_game":
                    yield return new FactoryPlayerLeftEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                    };
                    break;
                case "on_console_chat":
                    yield return new FactoryChatEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        PlayerName = evnt.PlayerName,
                        Message = evnt.Message,
                    };
                    break;
                case "on_research_started":
                    yield return new FactoryResearchStartedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        Technology = evnt.Technology,
                    };
                    break;
                case "on_research_finished":
                    yield return new FactoryResearchFinishedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        Technology = evnt.Technology,
                    };
                    break;
                case "on_rocket_launched":
                    yield return new FactoryRocketLaunchedEvent(factoryId, evnt.Type, evnt.Tick)
                    {
                        Rocket = evnt.Entity,
                    };
                    break;
                case "factorino_outgoing_train":
                    if (evnt.Inventory.Length > 0)
                    {
                        yield return new FactoryOutgoingTrainEvent(factoryId, evnt.Type, evnt.Tick)
                        {
                            TrainName = evnt.TrainName,
                            Inventory = evnt.Inventory.Select(i => (LuaItemStack)i).ToArray(),
                        };
                    }
                    if (Guid.TryParse(evnt.TrainName, out var shipmentId))
                    {
                        yield return new ShipmentCompletedEvent(shipmentId, config.Factorino.FactoryId, null)
                        {
                            ReturningCargo = evnt.Inventory.Select(i => (LuaItemStack)i).ToArray(),
                        };
                    }
                    break;
                default:
                    throw new EventUnknownException($"Could not transform event with type {evnt.Type}!");
            }
        }

        internal static IEvent[] TransformEvents(FactoryPodConfiguration config, params PodEventDTO[] events)
        {
            return events.SelectMany(e => TransformEvent(config, e)).ToArray();
        }
    }
}
