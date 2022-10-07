# FG-22 Worms project.

### Description
A small/beginner Unity project I made for FG-22.

#### Hotkeys
WASD: Move.  
Space: Jump.  

0-9: Change Weapon. (0 being none)  
Left Mouse: Use Weapon.  

X: Next Turn.  

#### Criteria
1. General  
- (G) Only play scene is required.  
- ~~(VG, small) Add main menu (start) scene and game over scene.~~  
- ~~(VG, medium) Implement Pause menu and settings menu.~~  
2. Turn based game  
- (G) You can have two players using the same input device taking turns.  
The InputManager gets the current active player and forwards the inputs to that specific player. This in turn allows me to limit inputs to one player at a time.
- (VG, large) Support up to 4 players (using the same input device taking turns).  
The system supports several a countless amount of teams with just as many players in them. However, since there's no UI, I made the amount random between 2 to 4 teams.  
- ~~(VG, large) Implement a simple AI opponent.~~  
3. Terrain  
- (G) Basic Unity terrain or primitives will suffice for a level.  
The terrain was quickly throw together in Blender, and there's no code to prevent the players from spawning inside of the obstacles. At one point I was experimenting with more procedural terrain so that it could be destroyed, but I had to shelve that functionality. (You can see it if you enabled the Procedural Terrain GameObject).
- ~~(VG, large) Destructible terrain (You can use Unity's built in terrain or your own custom solution).~~  
4. Player  
- (G) A player only controls one worm.  
- (G) Use the built in Character Controller. Add jumping.  
- (G) Has hit points.  
Every player has health, and it's displayed through a custom shader. The health is represented using a fluid inside of the player, and as the players health trickles down, so does the fill amount of the fluid.
- ~~(VG, small) Implement a custom character controller to control the movement of the worm.~~  
- ~~(VG, small) A worm can only move a certain range.~~  
- (VG, medium) A player controls a team of (multiple worms).  
5. Camera  
- (G) Focus camera on active player.  
The PlayerCamera class is what is used to track the players. THe PlayerCamera has a target player that it will follow, and this can be set through other scripts.
- (VG, small) Camera movement.  
Although this project doesn't have any fancy camera movements, you are able to still rotate the camera around the player freely.
6. Weapon system  
- (G) Minimum of two different weapons/attacks, can be of similar functionality, can be bound to an individual button, like weapon 1 is left mouse button and weapon 2 is right mouse button.  
There are two weapons, a richet rifle and a bomb bag. Both weapons and their respective projectiles are inherited from abstract classes to try and build a more modular system.  
- ~~(VG, small) a weapon can have ammo and needs to reload.~~  
- (VG, medium) The two types of weapons/attacks must function differently, I.E a pistol and a hand grenade. The player can switch between the different weapons and using the active weapon on for example left mouse button.  
The ricochet rifle shoots a bullet which can bounce up to 5 times, and the bomb bag drops a bomb that will explode a few seconds after impact.  
7. (VG, medium) Pickups  
- ~~Spawning randomly on the map during the play session.~~  
- ~~Gives something to the player picking it up, I.E health, extra ammo, new weapon, armour etc.~~  
8. Cheat functionalities  
- ~~(VG, medium) Two different cheats, I.E Invincible, all weapons on start etc.~~  
9. Miscellaneous  
- ~~(VG, medium) Battle royal, danger zones that move around on the map after a set amount of time.~~  
- ~~(VG, medium) High score that is persistent across game sessions.~~  
