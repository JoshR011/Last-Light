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
* The player can walk, sprint, slide, jump, and dash. Moving creates noise that can alert nearby enemies, and sprinting increases the noise radius.
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

* ~~Integrate enemy AI into the new enemy model (test sight-based chasing, obstacle navigation, and stopping distance).~~
* ~~Add a player character model.~~
* ~~Refine walking and jumping (improve ground detection and consistent jumps and landings).~~
* ~~Add dashing.~~
* ~~Implement AR and Burst SMG.~~
* ~~Build the overall outside terrain.~~
* ~~Create and texture a crate model (connect to the existing crate puzzle and hints).~~
* ~~Implement the keypad puzzle with code entry and a warehouse.~~
* ~~Create a health-pickup model (connect to the existing healing trigger).~~
* ~~Add a gun inventory (track guns and allow weapon switching).~~

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

The pistol is the first implemented weapon. An automatic rifle (AR) and a burst-fire SMG are the two additional weapons, bringing the weapon lineup to three.

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

The player uses a non-kinematic Rigidbody with gravity enabled. Pressing Space applies an upward impulse while the collision-based grounded flag is set. Walking, looking, and jumping establish the movement foundation for this checkpoint; sprinting, sliding, and dashing are planned for later deliverables.

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

<a href="https://imgur.com/a/lO9nhMJ"><img src="https://i.imgur.com/SOk4KWt.png" alt="Zombies detecting and chasing the player in the map" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credit: [3D Characters Zombie City Streets Lowpoly Pack - Lite](https://assetstore.unity.com/packages/3d/characters/humanoids/3d-characters-zombie-city-streets-lowpoly-pack-lite-277409) by CatBorg Studio, used for the zombie enemy model.

**Player Model: Character and Current Pose**

For this iteration, we added a 3D player character model from the Low Poly Character Pack and are using it as the player's body in the game.

Player animations have not been set up yet, so the character's arms and hands currently remain in a T-pose. For now, the gun is positioned in front of the player at the center of the first-person view. This is a temporary setup while animations and the weapon-holding pose are unfinished.

<a href="https://ibb.co/DH9pFt19"><img src="https://i.ibb.co/0RCtLcnC/image.png" alt="Player model showing the T-posed arms and hands and the gun centered in the first-person view" width="650" /></a>

Asset credit: [Low Poly Character Pack](https://assetstore.unity.com/packages/3d/characters/humanoids/low-poly-character-pack-357288) by Floreswa, used for the player character model.

**Movement: Refined Walking and Jumping**

For this iteration, we refined `PlayerMovement.cs` so walking, turning, jumping, and landing work more consistently with the player's Rigidbody. WASD movement follows the player's facing direction, while mouse input turns the body horizontally and tilts the first-person camera vertically. Physics movement and body rotation run in `FixedUpdate`, and jump presses are retained until the next physics step so an input is not lost between frames. The Rigidbody's rotation is constrained so collisions cannot tip the player over.

Ground detection now uses a downward raycast based on the player's collider bounds. The check ignores trigger colliders and requires an upward-facing surface, preventing health pickups and steep walls from being treated as ground. A jump requires this ground check to pass, and an additional upward-velocity check prevents another jump boost while the player is already rising.

We also reduced the jump impulse from 10 to 5 and removed vertical Rigidbody drag to reduce the floating feeling. Gravity remains at 9.81 units per second squared, while separate horizontal drag slows walking and dash momentum without slowing the fall. This gives the player a shorter jump with a more natural descent and landing.

**Movement: Dash**

For this iteration, we added `PlayerDash.cs` and a `Dash` action bound to Left Shift. Pressing the dash button requests a single impulse during the next physics step. The dash follows the direction of the player's movement input relative to the player's orientation. If no movement key is held, the player dashes straight forward. Diagonal input is normalized so a diagonal dash is not stronger than a forward or sideways dash.

The dash does not require the player to be grounded, allowing it to be used during a jump to cross gaps or reposition in combat. The current settings use an impulse of 15 and a one-second cooldown. The impulse adds to the player's existing momentum, while its horizontal direction allows gravity to continue controlling the jump and fall. Another dash becomes available after the cooldown, including while the player is still airborne.

<a href="https://i.imgur.com/N5vici9.mp4">Watch the dash demonstration (video)</a> · <a href="https://imgur.com/a/wqAwMmw">View on Imgur</a>

**Additional Weapons: Automatic Rifle and Burst SMG**

For this iteration, we added an automatic rifle (AR) and a burst-fire SMG alongside the existing pistol, completing the three-weapon lineup. Each weapon has a first-person model and its own Bullet Spawn transform, allowing the projectile to originate from the weapon's muzzle. These additions provide three firing patterns for the gun inventory.

The rifle uses `AutomaticWeapon.cs` to fire continuously while the left mouse button is held. Its current fire rate is 10 shots per second, with the time of the next allowed shot tracked so firing does not depend on how many frames are rendered. The SMG uses `BurstWeapon.cs` to fire three projectiles per mouse press, with 0.08 seconds between shots and a 0.3-second cooldown after the burst. A new burst cannot begin while the current burst is firing or cooling down.

Both weapons reuse the bullet prefab, Rigidbody launch, and `ContactDamager.cs` damage system from the pistol. Each projectile currently deals 25 damage and is removed on contact or after its three-second lifetime. The weapon models and firing scripts are connected to the inventory so switching weapons changes both the visible gun and its firing behavior.

<a href="https://imgur.com/a/kLVxyAs"><img src="https://i.imgur.com/mn1d5LX.png" alt="Automatic rifle and burst SMG in the player's first-person view, including their projectile trails" width="650" /></a>
<a href="https://imgur.com/a/2DZKxuk"><img src="https://i.imgur.com/RlGvimi.png" alt="Automatic rifle and burst SMG in the player's first-person view, including their projectile trails" width="650" /></a>
<a href="https://imgur.com/a/3Xuwya9"><img src="https://i.imgur.com/Cjml7oz.png" alt="Automatic rifle and burst SMG in the player's first-person view, including their projectile trails" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credit: [Low Poly Character Series 1 SWAT](https://assetstore.unity.com/packages/3d/characters/humanoids/low-poly-character-series-1-swat-404966) by CASTLE BRAVO, used for the pistol, AR, and SMG models.

**Map: Outdoor Terrain and Environment**

For this iteration, we developed the outdoor environment in `Assets/Scenes/Map.unity`, expanding the earlier layout design into a 3D scene. The environment includes forest ground, dirt paths and clearings, mountain surfaces, trees, and a perimeter boundary wall. Earth, moss, and exposed-stone surfaces give the terrain visual variety, while vegetation and other scenery establish the outdoor setting around the warehouse.

The map also includes the warehouse and its loading and storage areas, providing a destination for exploration. Ground and scenery collision support movement through the environment, and baked navigation data provides walkable routes for the enemies. Crates, health pickups, and zombies are placed in the scene so the existing mechanics can be used within the larger environment. Further houses, keypad clues, and visibility effects remain part of the later development plan.

<a href="https://imgur.com/a/87GX9I7"><img src="https://i.imgur.com/RMmKX0P.png" alt="Outdoor map overview and player-level view showing the terrain, paths, forest, and warehouse approach" width="650" /></a>

**Crate Model: Wooden Crates**

The crate-search puzzle was already implemented in the previous iteration. For this iteration, we connected the existing puzzle behavior to the imported `Wooden Crate` prefab from the Low Poly Wooden Crate asset pack. Multiple instances of the textured wooden model are placed in the map, giving the puzzle objects a recognizable appearance in the environment.

The prefab uses a wood-textured material and two levels of detail, allowing Unity to display a simpler mesh when the crate appears smaller on screen. A convex MeshCollider provides collision for the crate's shape, while a Rigidbody with gravity enabled lets it respond to physics. The `Life` and `Crate` components are attached to the same root object so projectile hits can damage the crate and trigger its puzzle response.

Each crate starts with 50 health. When its health reaches zero, `Crate.cs` checks whether it is the winning crate selected by `CrateManager.cs`. Breaking an incorrect crate creates a hint pointing toward the winning crate for four seconds, while breaking the winning crate prints a success message. The broken crate is then removed from the scene.

<a href="https://imgur.com/a/byxy2xD"><img src="https://i.imgur.com/FpwaFeF.png" alt="Wooden crate model in the map and its directional hint when broken" width="650" /></a>
<a href="https://imgur.com/a/8HJRmkx"><img src="https://i.imgur.com/KGFvevJ.png" alt="Wooden crate model in the map and its directional hint when broken" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credit: [Low Poly Wooden Crate](https://assetstore.unity.com/packages/3d/props/industrial/low-poly-wooden-crate-308305) by Aegis, used for the wooden crate model and textures.

**Puzzle: Keypad and Warehouse Access**

For this iteration, we added an interactive keypad to the `Warehouse` prefab and connected it to the entrance door. The keypad uses the imported Keypad FREE asset, giving the player a physical panel for entering the access code.

`KeypadInteractionFPV.cs` detects left-clicks on the keypad's buttons, while `KeypadButton.cs` animates each press and sends its value to `Keypad.cs`. The entered digits appear on the display, and pressing the keypad's Enter button checks the combination. An incorrect code produces a red display, a denied message, and a sound before clearing the input for another attempt. A correct code produces a green display, a granted message, and a confirmation sound.

The keypad's access-granted event calls `CabinDoor.OpenDoor()` on the warehouse door. The script rotates the door and its collider together through 90 degrees, opening the entrance and keeping it open after access is granted.

The clues for the keypad code have not been spread across the map yet. We plan to place them once the map is more fleshed out so we can choose better hiding spots and make finding the clues part of exploring the environment.

Clicking the keypad buttons can be difficult right now because we have not added a crosshair yet. We have not covered the UI portion of the course, so the crosshair is planned for a later iteration.

<a href="https://imgur.com/a/6-5zmXftx"><img src="https://i.imgur.com/iL0pAeC.png" alt="Keypad beside the warehouse entrance and the door opening after the correct code is entered" width="650" /></a>
<a href="https://imgur.com/a/OLM64ps"><img src="https://i.imgur.com/zVbmC6l.png" alt="Keypad beside the warehouse entrance and the door opening after the correct code is entered" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credits: [Cabin Environment](https://assetstore.unity.com/packages/3d/environments/cabin-environment-98014) by Gregory Seguru, used for the warehouse building and entrance door; [Keypad FREE](https://assetstore.unity.com/packages/3d/props/electronics/keypad-free-262151) by Navarone, used for the interactive keypad puzzle.

**Health-Pickup Model: Pill Bottle**

For this iteration, we connected the existing healing behavior to a recognizable 3D pickup using the imported `PillBottle` model from the HouseholdItems asset pack. The `Heal` prefab reuses the model and its material, adjusts its scale, and adds a BoxCollider configured as a trigger. This lets the player collect the item by touching it without the pickup acting as a solid obstacle.

`HealCollision.cs` checks that the entering object is tagged `Player` and has a `Life` component. The current prefab restores 20 health, capped at 100. For example, a player at 90 health receives only the remaining 10 health. The pickup is consumed after a successful heal, but remains available when the player is already at full health. This connects the new model to the healing trigger while keeping the health amount configurable in the Inspector.

<a href="https://imgur.com/a/aczkHvT"><img src="https://i.imgur.com/XyXXC2V.png" alt="Pill-bottle health pickup in the map and Inspector views showing the player's health before and after collecting it" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

**Gun Inventory: Weapon Switching**

For this iteration, we added a slot-based gun inventory using `InventoryManager.cs`. The script builds its inventory from the direct children of the Inventory Manager object, including inactive weapons. Their order in the Hierarchy determines their slot order. At startup, it disables all inventory items and activates the selected starting weapon, which defaults to the first slot.

The slot order is deliberately dynamic: weapons are assigned by their position in the inventory rather than permanently tied to a specific slot number. This design allows us to add a UI in a future iteration where the player can move weapons between slots and rearrange their inventory. For now, the starting order is set through the Hierarchy; rearranging weapons through a UI is planned for later.

The player can press 1, 2, or 3 to select a weapon slot through the `Slot1`, `Slot2`, and `Slot3` input actions. When switching to an available slot, the inventory disables the previous weapon and activates the new one. This keeps one weapon equipped at a time. Selecting an empty slot or pressing the number for the currently equipped weapon leaves the selection unchanged.

The current inventory uses weapons assigned in the Hierarchy at the start of the scene. Making weapons drop from crates and allowing the player to collect them are planned for a later iteration.

<a href="https://i.imgur.com/FrE65eK.mp4">Watch the gun inventory demonstration (video)</a> · <a href="https://imgur.com/a/At1ghao">View on Imgur</a>

**Instructions for Testing the Project**

All Part 2 features can be tested in `Assets/Scenes/Map.unity`. Open the scene in Unity, enter Play mode, and click the Game view to give it input focus. Use WASD to move, the mouse to look, Space to jump, and the left mouse button to shoot.

* **Keypad and warehouse door:** From the spawn area, the warehouse with the keypad is behind and to the right of the main warehouse. Click the keypad buttons to enter the configured access code, then press the keypad's Enter button to open the door.
* **Guns and inventory:** The player starts with all three guns already in the inventory. Press 1, 2, or 3 to switch between them and test their firing behavior. Gun pickups and drops are planned for the next iteration.
* **Healing:** During Play mode, select the player in the Hierarchy and lower Current Health on its `Life` component in the Inspector to a value below 100, such as 50. Return to the Game view and walk over a pill-bottle health pickup. Check the Inspector again to confirm that health increased and the pickup was consumed.
* **Crate puzzle:** Shoot the crate in front of the player to break it and test the puzzle. Breaking an incorrect crate creates a temporary hint pointing toward the correct crate. Breaking the correct crate prints `You found the right crate!` in Unity's Console.
* **Dashing:** Press Left Shift to dash. Try it while moving and while jumping to test both ground and midair dashes.
