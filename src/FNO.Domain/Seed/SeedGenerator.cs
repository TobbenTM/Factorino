using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FNO.Domain.Models;
using NLua;

namespace FNO.Domain.Seed
{
    public class SeedGenerator
    {
        private readonly Dictionary<object, object> _data;
        private readonly Lua _state;

        public SeedGenerator(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException($"Could not find the file at {filePath}!");
            }

            var content = File.ReadAllText(filePath);
            var fixedContent = content.Replace("Script @__DataRawSerpent__/data-final-fixes.lua:1: ", "data = ");
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, fixedContent);

            _state = new Lua();
            _state.DoFile(tempFile);
            _data = _state.GetTableDict(_state.GetTable("data"));
        }

        public FactorioEntity[] GetAllEntities()
        {
            var entityKeys = new[]
            {
                "ammo",
                "armor",
                "capsule",
                "fluid",
                "gun",
                "item",
                "module",
            };

            return entityKeys.SelectMany(key => LoadEntities(key)).ToArray();
        }

        private IEnumerable<FactorioEntity> LoadEntities(string key)
        {
            if (!_data.ContainsKey(key))
            {
                yield break;
            }

            var entities = _state.GetTableDict((LuaTable)_data[key]);
            foreach (var entityTable in entities.Values.Select(f => _state.GetTableDict((LuaTable)f)))
            {
                yield return TableToEntity(entityTable);
            }
        }

        private FactorioEntity TableToEntity(IDictionary<object, object> table)
        {
            var name = (string)table["name"];
            var type = (string)table["type"];
            var icon = table.ContainsKey("icon") ? (string)table["icon"] : string.Empty;
            return new FactorioEntity
            {
                Name = name,
                Type = type,
                Icon = icon.Replace("__base__/", ""),
                StackSize = table.ContainsKey("stack_size") ? (int)((long)(table["stack_size"])) : default,
                Subgroup = table.ContainsKey("subgroup") ? (string)(table["subgroup"]) : null,
                Fluid = type == "fluid",
            };
        }
    }
}
