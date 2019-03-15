# Factorino

Factorino is a metagame, or a layer on top of the awesome game [Factorio](https://factorio.com/)! It extends the Factorio factory simulation, by providing interaction between factories in the form of transactions, marketplace orders and more.

WARNING: This is still in very early development, lots of things are still going to change!

## Development environment

You need a kafka cluster, which can always be reset (or created) using the `reset.sh` script.

You need at least the factory pod image, if you don't create one using the `build.sh` script, it will be pulled from Docker hub.

## Handy debugging tools

For debugging kafka connectivity from inside Docker: 

`docker run --rm -it --network factorino wurstmeister/kafka bash`

Then

`bin/kafka-console-consumer.sh --bootstrap-server kafka:29092 --topic factorino_events --partition 0 --offset 0`
