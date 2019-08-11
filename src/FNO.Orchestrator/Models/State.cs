using FNO.Domain.Models;
using System;
using System.Collections.Generic;

namespace FNO.Orchestrator.Models
{
    public class State
    {
        private readonly Dictionary<Guid, Factory> _factories;

        public IEnumerable<Factory> Factories => _factories.Values;

        public State()
        {
            _factories = new Dictionary<Guid, Factory>();
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
    }
}
