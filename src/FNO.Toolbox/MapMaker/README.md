# Factorino World Creation

The world creation in Factorino is something that needs to be done before starting the universe, and is not that complicated.

In general there will be a couple of steps, some automated, some not.

The general idea is the following:

1. Use the CartographerTool to generate the world
    - This tool will use our own procedural landscape and biome generation
    - And more importantly, lets ut create deeds for the world and connect these through virtual railways
    - The output from this tool will be a data package that the cartographer Factorio mod can read, to create the saves necessary for the deeds
1. Use the toolbox to automate mapmaking
    - This will create a generic savegame with the correct dimensions for the deeds
    - It will then spin up this generic savegame in a headless Factorio server with rcon communication
    - And then for each deed create a save game using the tiles from the data package

# The CartographerTool

Simple SPA tool written in Typescript + React, which has all the functionality needed to create maps and shows previews, etc.

To start, open the "cartographer-tool" directory and run `yarn start`

# The cartographer mod
