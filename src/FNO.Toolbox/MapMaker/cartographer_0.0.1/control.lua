require 'mod-gui'

local button_name = 'cartographer-map-button'

local function create_button(_)
  for i, player in pairs(game.players) do
     local bf = mod_gui.get_button_flow(player)
     if not bf[button_name] then
        bf.add{
           type = "button",
           name = button_name,
           style = mod_gui.button_style,
           caption = "Create map.json"
        }
     end
  end
end

local function map_surface()
  local surface = game.surfaces[1]
  local player = game.players[1]
  local force = player.force
  local chunks = {}
  for chunk in surface.get_chunks() do
    if force.is_chunk_charted(surface, { chunk.x, chunk.y }) then
      local mappedChunk = {
        chunk = chunk,
        tiles = {}
      }

      for y = chunk.area.left_top.y, chunk.area.right_bottom.y, 1 do
        local row = {}
        for x = chunk.area.left_top.x, chunk.area.right_bottom.x, 1 do
          local tile = surface.get_tile(x, y)
          if not tile.valid then
            print("Invalid tile at x: " .. x .. ", y: " .. y)
          else
            table.insert(row, tile.name)
          end
        end
        table.insert(mappedChunk.tiles, row)
      end

      print("Done mapping a total of " .. #chunks .. " chunks..")
      table.insert(chunks, mappedChunk)
    end
  end

  game.print('Writing map to file...')
  game.write_file('map.json', game.table_to_json(chunks))
  game.print('Wrote map to map.json!')
end

script.on_event({defines.events.on_gui_click}, function(event)
  if(event.element.name == button_name) then
     map_surface()
  end
end)

script.on_configuration_changed(create_button)
script.on_event(defines.events.on_player_created, create_button)
script.on_event(defines.events.on_player_joined_game, create_button)
