# Last Light

## Team Members

[Joshua Antonio-Rodriguez](https://github.com/JoshR011), [Jacob Krinsky](https://github.com/krin-j), [Qingzhe Song](https://github.com/Qingzhe-Song)

## Game Summary

*Last Light* is a first-person survival-horror shooter set across a dark outdoor zone and an abandoned warehouse. The player must explore the map, fight enemies, and complete three puzzles to collect the objects needed to unlock the warehouse. Limited visibility and scarce supplies make every encounter dangerous, while fast movement gives the player the tools to survive.

The warehouse contains the final boss and an optional generator. Restoring power makes the battle easier by illuminating the arena, but a player who reaches the warehouse without activating the generator can still attempt the fight in darkness. Defeating the boss completes the game's main objective and permanently unlocks the warehouse as a safe point.

## Backstory

The protagonist is a reporter investigating a scientist's attempts to reanimate the dead and the disappearance of other reporters who followed the same story. Their investigation leads to the warehouse, where they are captured and confined in a small room on the property. They wake up surrounded by zombies, and a voice over the intercom warns that they should have stopped investigating. The voice claims to be watching from inside the warehouse.

The warehouse is the center of the reanimation operation and contains the research, evidence of the missing reporters, and the source of the threat. Reaching it gives the protagonist a chance to confront the operation and stop the experiments. Newspaper clippings around the map reveal the scientist's work, missing reporters, and monster sightings at the rural testing ground.

## Genres

* First-person shooter
* Survival horror
* Puzzle

## Inspiration

### Left 4 Dead

*Left 4 Dead* is a major influence on the game's survival-horror atmosphere and objective-based exploration. Its dark environments, constant enemy pressure, and emphasis on moving through dangerous spaces inspire the tension we want to create in *Last Light*. Our game will build on these ideas by asking the player to explore hostile areas, locate resources, and search for puzzle components while remaining vulnerable to nearby enemies.

<img src="https://www.co-optimus.com/images/upload/image/l4d_horde_hallway.jpg" alt="Left 4 Dead gameplay inspiration" width="500"/>

[Left 4 Dead Steam Page](https://store.steampowered.com/app/500/Left_4_Dead/)

### Lethal Company

*Lethal Company* is the main inspiration for the flashlight system and the tension created by limited visibility. Its handheld flashlight makes light feel like a valuable tool while the player explores dark, unfamiliar environments. *Last Light* will adapt this idea by giving the flashlight limited battery power and placing replacement batteries throughout the map. The player must decide when light is worth consuming, making the flashlight an important resource rather than only a visual effect.

<img src="https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1966720/ss_08fa3ef83b6eb70313119096f82285fa411f02e5.1920x1080.jpg?t=1775380053" alt="Lethal Company flashlight inspiration" width="500"/>

[Lethal Company Steam Page](https://store.steampowered.com/app/1966720/Lethal_Company/)

### Quake III Arena

*Quake III Arena* is the main inspiration for the game's fast movement, hip-fire combat, and map design. Its arenas use vertical spaces, open combat areas, and interconnected paths to keep players moving and create multiple ways to approach an encounter. *Last Light* will adapt these ideas to the outdoor zone and warehouse, giving the player room to sprint, slide, and dash while repositioning during combat. Most weapons will be designed for hip-fire instead of aiming down sights, while movement-based crosshair bloom will provide an accuracy tradeoff at high speeds. The screenshot below represents the visual direction for the map's industrial, arena-like spaces.

<img src="https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/2200/0000000283.1920x1080.jpg?t=1782164859" alt="Quake III Arena map design inspiration" width="500"/>

[Quake III Arena Steam Page](https://store.steampowered.com/app/2200/Quake_III_Arena/)

## Gameplay

* The player wakes up in a small room on the property, enters the outdoor zone, and must find a way into the warehouse.
* Three puzzles and challenges are spread throughout the map. Paper clues guide the player to each starting location, and completing the puzzles rewards the player with the objects needed to unlock the warehouse door.
  * Puzzle 1: A keypad that requires information found throughout the map to determine the correct access code.
  * Puzzle 2: A parkour course that the player must complete without falling.
  * Puzzle 3: Several crates are placed around the map, but only one contains the correct object. Breaking an incorrect crate causes a small light to point toward the correct one.
* The player can walk, sprint, slide, jump, and dash in midair. Moving creates noise that can alert nearby enemies, and sprinting increases the noise radius.
* Combat uses first-person aiming and shooting. Crosshair bloom reduces accuracy while the player is moving.
* A flashlight helps the player navigate dark areas but has limited battery power. Replacement batteries can be collected throughout the map.
* Health items restore lost health, while perks can provide upgrades such as life steal or increased weapon damage.
* The player can activate a generator to illuminate the warehouse before fighting the final boss. Skipping the generator is possible, but it makes the encounter more difficult.
* Defeating the boss completes the game's main objective and permanently unlocks the warehouse as a safe point.
* The game uses keyboard-and-mouse controls: WASD movement and mouse aiming, with additional inputs for jumping, sprinting, sliding, dashing, interacting, and toggling the flashlight.
* The interface will display a crosshair along with the player's health and remaining flashlight battery.

Graphics:

* First-person perspective with low-poly visuals inspired by classic shooters.
* A dark, low-visibility environment in which the flashlight and weapon fire provide important sources of light.
* Strong lighting contrast between the unpowered warehouse and the illuminated arena after the generator is activated.

## Development Plan

### Project Checkpoint 1-2: Basic Mechanics and Scripting (Ch 5-9)

* ~~Implement one weapon (think of list of others; three total).~~
* ~~One item: e.g., health buff (think of list of others; three total).~~
* ~~Puzzle solving framework + one puzzle (think of others; three total).~~
* ~~Start with fixed distance chasing of enemies (save sound-based chase mechanic for later).~~
* ~~Focus on jumping mechanic for next deliverable (think about other movement types for later deliverables).~~
* ~~Design a basic layout of the map (doesn't need to be implemented yet).~~
* ~~Postpone flashlight smoke/visibility aspects until later.~~
* ~~Flesh out back story: why is the protagonist here? Why is the warehouse important?~~

### Additions

* We connected the enemy's health event to a method that removes the enemy when its health reaches zero, allowing the weapon and health systems to work together.
* We added collision layers so projectiles can hit enemies and scenery while ignoring the player and held weapon.

### Project Part 2: 3D Scenes and Models (Ch 3+4, 10)

* Integrate enemy AI into the new enemy model (test sight-based chasing, obstacle navigation, and stopping distance).
* Refine walking and jumping (improve ground detection and consistent jumps and landings).
* Add midair dashing.
* Implement a second weapon: shotgun (close-range firing, damage, and first-person model).
* Build the overall outside terrain.
* Create and texture a crate model (connect to the existing crate puzzle and hints).
* Implement the keypad puzzle with code entry and a warehouse.
* Create a health-pickup model (connect to the existing healing trigger).
* Add a gun inventory (track guns and allow weapon switching).

### Project Part 3: Visual Effects

* Add fog effects (reduce visibility and build the horror atmosphere).
* Improve crate effects (add visual feedback when crates break).
* Add more houses around the map.
* Spread clues around the map for the keypad puzzle.
* Make weapons drop from crates (allow the player to collect them).
* Add a flashlight (help the player explore dark areas).
* Make zombie damage player when close.

## Development

### Project Checkpoint 1-2:

Our work for this deliverable consists of these main components:

* Basic Combat and Puzzle Function (pistol shooting, damage, enemy removal, crate search, and puzzle hints)
* First Person Movement and World Interaction (free look, walking, jumping, health pickups, enemy chasing, map layout, and backstory)

#### Basic Combat and Puzzle Function

**Weapon: Pistol**

The pistol is held under the player's first-person camera so it follows the player's view. Pressing the left mouse button creates a bullet at the assigned Bullet Spawn transform. `Weapon.cs` launches the bullet forward using its Rigidbody and removes it after its lifetime expires if it has not already hit something.

`ContactDamager.cs` handles projectile hits. Each bullet removes 25 health from an object with a `Life` component, then destroys itself. Enemies start with 100 health, so four successful hits reduce their health to zero. The enemy's `On Break` event calls `Life.DestroyObject()` to remove the enemy and its children. The PlayerBullet and Weapon layers prevent bullets from hitting the held pistol or player.

The pistol is the first implemented weapon. A shotgun for close-range combat and a knife for melee combat are planned as the two additional weapons, for three total.

<a href="https://imgur.com/a/UeTCRna"><img src="https://i.imgur.com/UHfacnR.png" alt="Pistol in the player's view, shooting, and enemy removal" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>


**Puzzle Framework: Crate Search**

The puzzle framework uses a shared manager, breakable objects, and health-based events. `CrateManager.cs` gathers the active crates and selects the correct crate when one has not already been assigned. Each crate connects its `Life.On Break` event to the puzzle response in `Crate.cs`.

Breaking an incorrect crate creates a temporary hint line pointing from that crate toward the correct one. The incorrect crate disappears, and the hint remains for four seconds. Breaking the correct crate reports that the player has found the right crate. This provides the first puzzle's search, feedback, and success behavior, which can support the later warehouse-access objective.

The crate search is the first implemented puzzle. The two additional puzzles are a keypad whose access code is assembled from clues around the map and a parkour course that requires jumping between platforms without falling, for three total.

<a href="https://imgur.com/a/F435G1l"><img src="https://i.imgur.com/KIsWkVE.png" alt="Crate puzzle, hint pointing toward the correct crate, and success message" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

<a href="https://imgur.com/a/6fqxAd6"><img src="https://i.imgur.com/W74xaq8.png" alt="Crate puzzle, hint pointing toward the correct crate, and success message" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>


#### First Person Movement and World Interaction

**Movement: Walking and Jumping**

`PlayerMovement.cs` handles movement, mouse look, and jumping through Unity's Input System. Movement applies force relative to the player's orientation, horizontal mouse input turns the body, and vertical input rotates the camera. The camera's vertical angle is limited to prevent it from turning completely upside down.

The player uses a non-kinematic Rigidbody with gravity enabled. Pressing Space applies an upward impulse while the collision-based grounded flag is set. Walking, looking, and jumping establish the movement foundation for this checkpoint; sprinting, sliding, and midair dashing are planned for later deliverables.

**Item: Full-Health Pickup**

The player has a `Life` component with 100 starting health. Health pickups use a trigger collider and `HealCollision.cs` to detect an object tagged `Player`. When the player is below full health, touching the pickup restores health to 100 and consumes the pickup. At full health, the pickup remains available. Full restoration is the intended behavior for this item. A temporary weapon-damage buff and a life-steal perk that restores health through combat are planned as the two additional items, for three total.

<a href="https://imgur.com/a/rrU8V1P"><img src="https://i.imgur.com/1mPRkdR.png" alt="Player health before and after collecting a health pickup" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>


**Enemies: Fixed-Distance Chasing**

Enemies use `Sight.cs` to search for the player within a fixed range and viewing angle. An obstacle check prevents detection through objects on the obstacle layers. `EnemyFSM.cs` switches between stopping and moving toward the detected player using a NavMeshAgent. The current settings use a detection distance of 10 units, a viewing angle of 60 degrees from forward, and a stopping distance of 1.5 units. Enemies stop when they lose sight of the player or get close enough. Sound-based chasing, including a larger noise radius while sprinting, is planned for a later deliverable.

<a href="https://imgur.com/a/VaYWcuT"><img src="https://i.imgur.com/G41b7C4.png" alt="Enemy detection, chasing, and stopping behavior" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>


**Map: Basic Layout Design**

The map design centers on an outdoor exploration zone leading to the locked warehouse. The outdoor zone provides routes between enemy encounters, health pickups, and the three planned puzzle areas. The warehouse contains the generator and final boss arena, making it both the destination of the outdoor objectives and the center of the story.

For this checkpoint, the layout is a design deliverable; the complete environment does not need to be implemented yet. The current prototype provides a space for testing the core mechanics before they are placed into the finished map.

<a href="https://imgur.com/a/Yo69G7V"><img src="https://i.imgur.com/Hq10rMr.png" alt="Map layout showing the starting area, routes, puzzles, warehouse entrance, generator, and boss arena" width="650" /></a>

**Story: Protagonist and Warehouse Backstory**

See the [Backstory](#backstory) above for the protagonist's background and the warehouse's importance.

**Deferred Features: Flashlight, Smoke, and Visibility**

The flashlight, battery pickups, smoke, and limited-visibility effects are postponed until later deliverables. The generator, final boss encounter, warehouse safe-point behavior, player damage and death behavior, and health, crosshair, and flashlight-battery interface are also planned for later development.

#### Running Instructions

Open the project in Unity, load `Assets/Scenes/Main.unity`, enter Play mode, and click the Game view to give it input focus.

Use WASD to move, the mouse to look, Space to jump, and the left mouse button to fire. Shoot crates to test the puzzle hints and shoot enemies to test damage and removal.

To demonstrate healing, lower the player's `Life` amount below 100 in the Inspector during Play mode, then touch a health pickup.

### Project Part 2: 3D Scenes and Models

**Enemy AI: Sight-Based Chasing**

The enemy AI was already implemented in the previous iteration. For this iteration, we transferred the existing detection and chasing behavior to a 3D enemy model using the `Zombie` prefab.

The map now includes multiple instances of the `Zombie` prefab. Each zombie has an AI child containing `Sight.cs` and `EnemyFSM.cs`, while a NavMeshAgent on the parent controls movement across the baked navigation mesh.

`Sight.cs` checks for the player within 10 units and a viewing cone extending 60 degrees to either side of the enemy's forward direction. It searches the Player layer and checks for obstacles on the Default layer between the sensor and the player's collider center. The player must be within range, inside the viewing cone, and unobstructed for the enemy to detect them.

`EnemyFSM.cs` switches between `Stop` and `MoveToPlayer`. When the player is visible and more than 1.5 units away, the enemy updates its navigation destination to follow the player's current position. The NavMeshAgent handles pathfinding along walkable routes at a configured speed of 3.5 units per second. The enemy stops when the player is close enough or leaves its line of sight, and resumes chasing when the player is visible and outside the stopping distance again.

*[Image placeholder: Add a screenshot showing the zombies detecting and chasing the player in the map.]*

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

**Crate Model: Wooden Crates**

The crate-search puzzle was already implemented in the previous iteration. For this iteration, we connected the existing puzzle behavior to the imported `Wooden Crate` prefab from the Low Poly Wooden Crate asset pack. Multiple instances of the textured wooden model are placed in the map, giving the puzzle objects a recognizable appearance in the environment.

The prefab uses a wood-textured material and two levels of detail, allowing Unity to display a simpler mesh when the crate appears smaller on screen. A convex MeshCollider provides collision for the crate's shape, while a Rigidbody with gravity enabled lets it respond to physics. The `Life` and `Crate` components are attached to the same root object so projectile hits can damage the crate and trigger its puzzle response.

Each crate starts with 50 health. When its health reaches zero, `Crate.cs` checks whether it is the winning crate selected by `CrateManager.cs`. Breaking an incorrect crate creates a hint pointing toward the winning crate for four seconds, while breaking the winning crate prints a success message. The broken crate is then removed from the scene.

*[Image placeholder: Add a screenshot showing the wooden crate model in the map and its directional hint when broken.]*

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

**Puzzle: Keypad and Warehouse Access**

For this iteration, we added an interactive keypad to the `Warehouse` prefab and connected it to the entrance door. The keypad uses the imported Keypad FREE asset, giving the player a physical panel for entering the access code.

`KeypadInteractionFPV.cs` detects left-clicks on the keypad's buttons, while `KeypadButton.cs` animates each press and sends its value to `Keypad.cs`. The entered digits appear on the display, and pressing the keypad's Enter button checks the combination. An incorrect code produces a red display, a denied message, and a sound before clearing the input for another attempt. A correct code produces a green display, a granted message, and a confirmation sound.

The keypad's access-granted event calls `CabinDoor.OpenDoor()` on the warehouse door. The script rotates the door and its collider together through 90 degrees, opening the entrance and keeping it open after access is granted.

The clues for the keypad code have not been spread across the map yet. We plan to place them once the map is more fleshed out so we can choose better hiding spots and make finding the clues part of exploring the environment.

Clicking the keypad buttons can be difficult right now because we have not added a crosshair yet. We have not covered the UI portion of the course, so the crosshair is planned for a later iteration.

*[Image placeholder: Add a screenshot showing the keypad beside the warehouse entrance and the door opening after the correct code is entered.]*

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

**Gun Inventory: Weapon Switching**

For this iteration, we added a slot-based gun inventory using `InventoryManager.cs`. The script builds its inventory from the direct children of the Inventory Manager object, including inactive weapons. Their order in the Hierarchy determines their slot order. At startup, it disables all inventory items and activates the selected starting weapon, which defaults to the first slot.

The player can press 1, 2, or 3 to select a weapon slot through the `Slot1`, `Slot2`, and `Slot3` input actions. When switching to an available slot, the inventory disables the previous weapon and activates the new one. This keeps one weapon equipped at a time. Selecting an empty slot or pressing the number for the currently equipped weapon leaves the selection unchanged.

The current inventory uses weapons assigned in the Hierarchy at the start of the scene. Making weapons drop from crates and allowing the player to collect them are planned for a later iteration.

*[Image placeholder: Add screenshots showing the player switching between weapons using the inventory slots.]*

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>
