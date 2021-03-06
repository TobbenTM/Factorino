﻿using System;
using System.Collections.Generic;
using System.Linq;
using FNO.Domain.Models;
using FNO.Domain.Models.Shipping;

namespace FNO.Domain.Extensions
{
    public static class CartExtensions
    {
        public static LuaItemStack[] Reduce(this IEnumerable<Cart> carts)
        {
            var stacks = new Dictionary<string, LuaItemStack>();
            foreach (var cart in carts)
            {
                foreach (var stack in cart.Inventory)
                {
                    if (string.IsNullOrEmpty(stack.Name))
                    {
                        throw new InvalidOperationException($"LuaItemStack is malformed!");
                    }

                    if (stacks.ContainsKey(stack.Name))
                    {
                        stacks[stack.Name].Count += stack.Count;
                    }
                    else
                    {
                        stacks[stack.Name] = new LuaItemStack
                        {
                            Name = stack.Name,
                            Count = stack.Count,
                        };
                    }
                }
            }
            return stacks.Values.ToArray();
        }

        public static LuaItemStack[] Negate(this LuaItemStack[] stacks)
        {
            foreach (var stack in stacks)
            {
                stack.Count *= -1;
            }
            return stacks;
        }
    }
}
