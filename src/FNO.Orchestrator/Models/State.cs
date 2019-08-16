using FNO.Domain.Models;
using System;
using System.Collections.Generic;

namespace FNO.Orchestrator.Models
{
    public class State
    {
        private readonly Dictionary<Guid, Factory> _factories;
        private readonly Dictionary<Guid, string> _playerUsernames;

        public IEnumerable<Factory> Factories => _factories.Values;

        public State()
        {
            _factories = new Dictionary<Guid, Factory>();
            _playerUsernames = new Dictionary<Guid, string>();
        }

        public State(Dictionary<Guid, Factory> initialState)
        {
            _factories = initialState;
        }

        public Factory GetFactory(Guid factoryId)
        {
            return _factories[factoryId];
        }

        public void AddFactory(Factory factory)
        {
            _factories[factory.FactoryId] = factory;
        }

        public string GetUsername(Guid playerId)
        {
            if (_playerUsernames.ContainsKey(playerId))
            {
                return _playerUsernames[playerId];
            }
            return null;
        }

        public void SetUsername(Guid playerId, string username)
        {
            _playerUsernames[playerId] = username;
        }
    }
}
