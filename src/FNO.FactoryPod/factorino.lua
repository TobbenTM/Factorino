local json = require('json')

local version = '0.0.1'
print('[factorino.lua] Booting factorino mod version ' .. version)

-- Some offsets
-- From the awesome mod Automatic-Train-Deployment:
local station_offsets = {
  [defines.direction.north] = {x = -2, y = 3},
  [defines.direction.east] = {x = -3, y = -2},
  [defines.direction.south] = {x = 2, y = -3},
  [defines.direction.west] = {x = 3, y = 2}
}
local cart_offsets = {
  [defines.direction.north] = {x = 0, y = 7},
  [defines.direction.east] = {x = -7, y = 0},
  [defines.direction.south] = {x = 0, y = -7},
  [defines.direction.west] = {x = 7, y = 0}
}
local orientation_to_direction = {
  [0] = defines.direction.north,
  [0.25] = defines.direction.east,
  [0.5] = defines.direction.south,
  [0.75] = defines.direction.west
}


-- Events not yet sent through the RCON interface
local event_buffer = {}

-- Shipment state
local rejected_shipments = {}
local accepted_shipments = {}
local incoming_shipment_queue = {}

-- Pickup train flag
local pickup_train_en_route = false

-- General event handler to buffer events
function on_event(event)
  table.insert(event_buffer, event)
end

-- Called through RCON by the server to export any events to the global event stream
function on_export()
  if #event_buffer > 0 then
    print('[factorino.lua] Exporting '..#event_buffer..' events!')
  end
  local copy = event_buffer
  event_buffer = {}
  rcon.print(json.encode(copy))
end

-- Called through RCON by the server to handle a warehouse shipment
function on_import(payload)
  local shipment = json.decode(payload.parameter)
  table.insert(incoming_shipment_queue, shipment)
  handle_shipment()
end

-- Called through RCON by the server to signal that the player has researched a tech
function on_researched(payload)
  -- TODO
end

-- Converts LuaEntity to a simpler entity, more suited for transport
function to_simple_entity(lua_entity)
  local entity = {
    -- type = lua_entity.type
    name = lua_entity.name
  }
  -- if lua_entity.name then entity.name = lua_entity.name end
  if lua_entity.backer_name then entity.backer_name = lua_entity.backer_name end
  return entity
end

-- Converts LuaItemStack to a simpler entity, more suited for transport
function to_simple_item_stack(lua_item_stack)
  local item_stack = {
    type = lua_item_stack.type,
    name = lua_item_stack.name,
    count = lua_item_stack.count
  }
  return item_stack
end

-- Converts LuaResearch to a simpler entity, more suited for transport
function to_simple_research(lua_research)
  local research = {
    name = lua_research.name,
    level = lua_research.level
  }
  return research
end

-- Handle an incoming shipment from the queue
function handle_shipment()
  -- Do we actually have shipments to handle?
  if #incoming_shipment_queue == 0 then return end

  -- Do we have the room to spawn a train?
  -- Length = 2 locomotives + x carts
  local in_station = find_station('Factorino - Incoming')
  local train_length = 2 + #incoming_shipment_queue[1].carts
  local cart_positions = calculate_cart_positions(in_station, train_length)
  -- Hopefully we'll try again once a train has changed state
  if not station_has_room(in_station, cart_positions) then return end

  -- We should be ready to handle the shipment now
  local shipment = table.remove(incoming_shipment_queue, 1)
  local result = create_train(shipment, in_station, cart_positions)
  if result then
    table.insert(accepted_shipments, shipment)
  else
    table.insert(rejected_shipments, shipment)
  end
end

-- We want to have pickup trains going every once in a while
function create_pickup_train()
  if pickup_train_en_route then return end

  pickup_train_en_route = true

  -- Do we have the room to spawn a train?
  -- Length = 2 locomotives + 5 carts
  local in_station = find_station('Factorino - Incoming')
  local train_length = 2 + 5
  local cart_positions = calculate_cart_positions(in_station, train_length)
  -- Hopefully we'll try again once a train has changed state
  if not station_has_room(in_station, cart_positions) then return end

  local shipment = {
    shipment_id = 'Pickup',
    destination_station = 'Factorino - Pickup 1',
    wait_conditions = {
      {
        type = 'inactivity',
        ticks = 2400,
        compare_type = 'or'
      },
      {
        type = 'full',
        compare_type = 'or'
      }
    },
    carts = {
      {
        cart_type = 'cargo-wagon',
        inventory = {}
      },
      {
        cart_type = 'cargo-wagon',
        inventory = {}
      },
      {
        cart_type = 'cargo-wagon',
        inventory = {}
      },
      {
        cart_type = 'cargo-wagon',
        inventory = {}
      },
      {
        cart_type = 'cargo-wagon',
        inventory = {}
      }
    }
  }
  create_train(shipment, in_station, cart_positions)
end

function create_train(shipment, station, cart_positions)
  local schedule = create_schedule(shipment)
  local force = game.forces['neutral']
  local rolling_stock = nil

  for k, pos in pairs(cart_positions) do
    if k == 1 or k == #cart_positions then
      -- First or last cart; place locomotive
      rolling_stock = station.surface.create_entity{name = 'locomotive', position = pos, direction = station.direction, force = force}

      if rolling_stock ~= nil then
        -- The new locomotive needs some fuel to survive the journey..
        rolling_stock.get_fuel_inventory().insert({name = 'coal', count = '5'})
        -- ..and some awesome colors..
        rolling_stock.color = {r = 1, g = 1, b = 1}
        -- ..and a less awesome name
        rolling_stock.backer_name = shipment.shipment_id
      end
    else
      -- Shipment carts (cargo or fluids)
      local content = shipment.carts[k-1]
      rolling_stock = station.surface.create_entity{name = content.cart_type, position = pos, direction = station.direction, force = force}
      local inventory = rolling_stock.get_inventory(defines.inventory.cargo_wagon)

      -- Insert all stacks in inventory
      for _, item_stack in pairs(content.inventory) do
        inventory.insert(item_stack)
      end
    end

    if rolling_stock == nil then
      print('[ERR] [factorino.lua] Train creation failed! Shipment id: '..shipment.shipment_id)
      return false
    end

    -- Setting some flags to prevent the train from being looted or destroyed
    rolling_stock.minable = false
    rolling_stock.operable = false
    rolling_stock.rotatable = false
    rolling_stock.destructible = false
  end

  -- Set the trains schedule and mode
  local train = rolling_stock.train
  train.schedule = schedule
  train.manual_mode = false

  station.surface.print('Train from warehouse incoming, headed to '..shipment.destination_station..'!')
  return true
end

function create_schedule(shipment)
  local destination = shipment.destination_station
  local conditions = shipment.wait_conditions
  local schedule = {
    current = 1,
    records = {
      { station = 'Factorino - Incoming', wait_conditions = { { type = "time", ticks = 120, compare_type = "or" } } },
      { station = destination, wait_conditions = conditions },
      { station = 'Factorino - Outgoing', wait_conditions = { { type = "circuit", compare_type = "or" } } }
    }
  }
  return schedule
end

function find_station(station_name)
  local stops = game.surfaces[1].find_entities_filtered{ name = 'train-stop', force = 'neutral' }
  for k, v in pairs(stops) do
    if v.backer_name == station_name then return v end
  end
  return nil
end

function station_has_room(station, cart_positions)
  local surface = station.surface
  for k, pos in pairs(cart_positions) do
    if not surface.can_place_entity{name = 'cargo-wagon', position = pos} then return false end
  end
  return true
end

function calculate_cart_positions(station, length)
  local cart_offset = cart_offsets[station.direction]
  local station_offset = station_offsets[station.direction]
  local current_position = { x = station.position.x + station_offset.x, y = station.position.y + station_offset.y }
  local positions = { current_position }
  for i = 2, length, 1
  do
    local x_offset = (i - 1) * cart_offset.x
    local y_offset = (i - 1) * cart_offset.y
    table.insert(positions, {
      x = current_position.x + x_offset,
      y = current_position.y + y_offset
    })
  end
  return positions
end

-- We need to handle any trains coming into the outgoing station
script.on_event(defines.events.on_train_changed_state, function(event)
  local train = event.train

  -- Validate train
  if not train or not train.valid then return end

  if train.state == defines.train_state.wait_station then
    -- Ensure first carriage is a loco
    if train.carriages[1].name ~= 'locomotive' then return end

    local station = train.station
    if station.backer_name == 'Factorino - Outgoing' and station.force == game.forces['neutral'] then
      local inventory = train.get_contents()
      print('[factorino.lua] Outgoing train has inventory: ' .. json.encode(inventory))

      local inventory_stacks = {}
      for item_name, item_count in pairs(inventory) do
        table.insert(inventory_stacks, {
          name = item_name,
          count = item_count,
        })
      end

      on_event({
        type = 'factorino_outgoing_train',
        tick = event.tick,
        train_name = train.carriages[1].backer_name,
        inventory = inventory_stacks,
      })

      if train.carriages[1].backer_name == 'Pickup' then
        pickup_train_en_route = false
      end

      for _, cart in pairs(train.carriages) do
        cart.destroy()
      end

      create_pickup_train()
    else
      -- This is a good time to check if we have waiting shipments
      handle_shipment()
    end
  end
end)


-- Add custom commands (RCON)

commands.add_command('factorino_export', 'n/a', on_export)
commands.add_command('factorino_import', 'n/a', on_import)
commands.add_command('factorino_researched', 'n/a', on_researched)


-- Register handler for periodically creating pickup trains

script.on_nth_tick(600, create_pickup_train)


-- Bind all interesting events to simplified data

script.on_event(defines.events.on_built_entity, function(event)
  on_event({
    type = 'on_built_entity',
    tick = event.tick,
    player_name = game.players[event.player_index].name,
    entity = to_simple_entity(event.created_entity),
  })
end)
script.on_event(defines.events.on_entity_died, function(event)
  on_event({
    type = 'on_entity_died',
    tick = event.tick,
    player_name = game.players[event.player_index].name,
    entity = to_simple_entity(event.entity),
  })
end)
script.on_event(defines.events.on_player_joined_game, function(event)
  local player = game.players[event.player_index]
  on_event({
    type = 'on_player_joined_game',
    tick = event.tick,
    player_name = player.name,
  })
  player.print('Welcome to Factorino! (Version: ' .. version .. ')')
end)
script.on_event(defines.events.on_player_mined_item, function(event)
  on_event({
    type = 'on_player_mined_item',
    tick = event.tick,
    player_name = game.players[event.player_index].name,
    item_stack = to_simple_item_stack(event.item_stack),
  })
end)
script.on_event(defines.events.on_player_mined_entity, function(event)
  on_event({
    type = 'on_player_mined_entity',
    tick = event.tick,
    player_name = game.players[event.player_index].name,
    entity = to_simple_entity(event.entity),
  })
end)
script.on_event(defines.events.on_player_died, function(event)
  on_event({
    type = 'on_player_died',
    tick = event.tick,
    player_name = game.players[event.player_index].name,
  })
end)
script.on_event(defines.events.on_player_left_game, function(event)
  on_event({
    type = 'on_player_left_game',
    tick = event.tick,
    player_name = game.players[event.player_index].name,
  })
end)
script.on_event(defines.events.on_console_chat, function(event)
  -- 'Chat' events from console should not be logged
  if event.player_index == nil then return end
  on_event({
    type = 'on_console_chat',
    tick = event.tick,
    player_name = game.players[event.player_index].name,
    message = event.message,
  })
end)
script.on_event(defines.events.on_research_started, function(event)
  on_event({
    type = 'on_research_started',
    tick = event.tick,
    technology = to_simple_research(event.research),
  })
end)
script.on_event(defines.events.on_research_finished, function(event)
  on_event({
    type = 'on_research_finished',
    tick = event.tick,
    technology = to_simple_research(event.research),
  })
end)
script.on_event(defines.events.on_rocket_launched, function(event)
  on_event({
    type = 'on_rocket_launched',
    tick = event.tick,
    entity = to_simple_entity(event.rocket),
  })
end)
