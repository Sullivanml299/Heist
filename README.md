# Multiplayer Heist Prototype
This is a school project I made that prototyped in Unity as a concept for a networked competitive heist game! The goal of the game is to loot as much value from the house and return to the green square at the entrance without getting caught.

## Networked using MirrorNetworking

<img src="./gifs/network.gif" width=50% height=5% />

Networking was implemnted using the open-source MirrorNetworking library for Unity.

## Cloaking Shader

<img src="./gifs/cloak.gif" width=50% height=5% />

The cloaking ability can be used to hide from other players and guards. The cloaking effect was made using ShaderGraph.

## Loot

<img src="./gifs/loot.gif" width=50% height=5% />

Loot items from highlighted containers.

## Patroling Guard

<img src="./gifs/patrol.gif" width=50% height=5% />

Guard pathing was implemented using NavMesh. Line of sight was implemented using raycasts to  procedurally update a mesh.

<img src="./gifs/catch.gif" width=50% height=5% />

Getting caught sends the player back to spawn and resets your stolen loot.

<img src="./gifs/line-of-sight.gif" width=50% height=5% />

<img src="./gifs/cloak-hide.gif" width=50% height=5% />

The guard only follows to the last-known location. Break line of sight by quickly turning corners or using the cloak.




