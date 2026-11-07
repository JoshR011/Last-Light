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

* ~~Add fog effects (reduce visibility and build the horror atmosphere).~~
* ~~Reduce player visiblity.~~
* ~~Improve crate effects (add visual feedback when crates break).~~
* ~~Add more houses around the map.~~
* ~~Spread clues around the map for the keypad puzzle.~~
* ~~Make weapons drop from crates (allow the player to collect them).~~
* ~~Add a flashlight (help the player explore dark areas).~~
* ~~Make zombie damage the player when close.~~
* ~~Add hit effect when shooting enemies.~~
* ~~Add bullet effect when shooting.~~
* ~~Make the keypad bigger so its colliders are easy to hit.~~
* ~~Add crouch and slide.~~
* ~~Add healing effect.~~
* ~~Add effect when taking damage.~~

### Project Checkpoint 3-4: Sound, UI, and Animation

* Implement an inventory UI that displays collected weapons and highlights the equipped weapon.
* Add a health UI that clearly shows the player's current health and updates when taking damage or healing.
* Add cooldown indicators so the player can see when movement abilities are ready to use again.
* Add an ammunition UI to display the remaining bullet count for the equipped weapon.
* Add footstep sounds that match the player's walking pace.
* Add crate-breaking sounds to reinforce the impact of destroying crates.
* Add zombie sounds to make nearby enemies easier to hear and build tension during encounters.
* Add shooting sounds for each weapon to give combat clear, distinct audio feedback.
* Add a healing sound that plays when a health pickup successfully restores health.
* Add zombie animations for movement and attacks to make enemy encounters feel more lifelike.
* Add a player walking animation that matches movement speed for smoother, more natural motion.
* Add a dashing animation that emphasizes the burst of speed and gives clear feedback when dashing.

## Development

### Project Checkpoint 1-2:

Our work for this deliverable consists of these main components:

* Basic Combat and Puzzle Function (pistol shooting, damage, enemy removal, crate search, and puzzle hints)
* First Person Movement and World Interaction (free look, walking, jumping, health pickups, enemy chasing, map layout, and backstory)

#### Basic Combat and Puzzle Function

**Weapon: Pistol**

The pistol is held under the player's first-person camera so it follows the player's view. Pressing the left mouse button creates a bullet at the assigned Bullet Spawn transform. `PistolWeapon.cs` launches the bullet forward using its Rigidbody and removes it after its lifetime expires if it has not already hit something.

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

<a href="https://ibb.co/39GQPgVR"><img src="https://i.ibb.co/fzBLTjZD/image.png" alt="Zombies detecting and chasing the player in the map" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credit: [3D Characters Zombie City Streets Lowpoly Pack - Lite](https://assetstore.unity.com/packages/3d/characters/humanoids/3d-characters-zombie-city-streets-lowpoly-pack-lite-277409) by CatBorg Studio, used for the zombie enemy model.

**Player Model: Character and Current Pose**

For this iteration, we added a 3D player character model from the Low Poly Character Pack and are using it as the player's body in the game.

Player animations have not been set up yet, so the character's arms and hands currently remain in a T-pose. For now, the gun is positioned in front of the player at the center of the first-person view. This is a temporary setup while animations and the weapon-holding pose are unfinished.

<a href="https://ibb.co/DH9pFt19"><img src="https://i.ibb.co/0RCtLcnC/image.png" alt="Player model showing the T-posed arms and hands and the gun centered in the first-person view" width="650" /></a>

Asset credit: [Low Poly Character Pack](https://assetstore.unity.com/packages/3d/characters/humanoids/low-poly-character-pack-357288) by Floreswa, used for the player character model.

**Movement: Refined Walking and Jumping**

For this iteration, we refined `PlayerMovement.cs` so walking, turning, jumping, and landing work more consistently with the player's Rigidbody. WASD movement follows the player's facing direction, while mouse input turns the body horizontally and tilts the first-person camera vertically. Physics movement and body rotation run in `FixedUpdate`, and jump presses are retained until the next physics step so an input is not lost between frames. The Rigidbody's rotation is constrained so collisions cannot tip the player over.

Ground detection now tracks collision contacts beneath the player's collider and requires an upward-facing surface. Trigger pickups do not generate these contacts, and steep walls fail the surface-normal check, preventing either from being treated as ground. Jump buffering retains a press briefly before landing, while coyote time allows a jump briefly after walking off an edge. Each jump consumes both timing windows, and lingering launch contacts cannot immediately enable another jump. Before applying the upward impulse, the script resets vertical velocity so falling or moving along a slope does not weaken the jump.

We also reduced the jump impulse from 10 to 5 and removed vertical Rigidbody drag to reduce the floating feeling. Gravity remains at 9.81 units per second squared, while separate horizontal drag slows walking and dash momentum without slowing the fall. This gives the player a shorter jump with a more natural descent and landing.

**Movement: Dash**

For this iteration, we added `PlayerDash.cs` and a `Dash` action bound to Left Shift. Pressing the dash button requests a single impulse during the next physics step. The dash follows the direction of the player's movement input relative to the player's orientation. If no movement key is held, the player dashes straight forward. Diagonal input is normalized so a diagonal dash is not stronger than a forward or sideways dash.

The dash does not require the player to be grounded, allowing it to be used during a jump to cross gaps or reposition in combat. The current settings use an impulse of 15 and a one-second cooldown. The impulse adds to the player's existing momentum, while its horizontal direction allows gravity to continue controlling the jump and fall. Another dash becomes available after the cooldown, including while the player is still airborne.

<a href="Recordings/Dashing.mp4">Watch the dash demonstration (video)</a>

**Additional Weapons: Automatic Rifle and Burst SMG**

For this iteration, we added an automatic rifle (AR) and a burst-fire SMG alongside the existing pistol, completing the three-weapon lineup. Each weapon has a first-person model and its own Bullet Spawn transform, allowing the projectile to originate from the weapon's muzzle. These additions provide three firing patterns for the gun inventory.

The rifle uses `AutomaticWeapon.cs` to fire continuously while the left mouse button is held. Its current fire rate is 10 shots per second, with the time of the next allowed shot tracked so firing does not depend on how many frames are rendered. The SMG uses `BurstWeapon.cs` to fire three projectiles per mouse press, with 0.08 seconds between shots and a 0.3-second cooldown after the burst. A new burst cannot begin while the current burst is firing or cooling down.

Both weapons reuse the bullet prefab, Rigidbody launch, and `ContactDamager.cs` damage system from the pistol. Each projectile currently deals 25 damage and is removed on contact or after its three-second lifetime. The weapon models and firing scripts are connected to the inventory so switching weapons changes both the visible gun and its firing behavior.

<a href="https://ibb.co/GvTF54zX"><img src="https://i.ibb.co/SXrJ34F2/image.png" alt="Automatic rifle and burst SMG in the player's first-person view, including their projectile trails" width="650" /></a>
<a href="https://ibb.co/zVKhLNqB"><img src="https://i.ibb.co/YBv7KbH1/image.png" alt="Automatic rifle and burst SMG in the player's first-person view, including their projectile trails" width="650" /></a>
<a href="https://ibb.co/Y42XzVYb"><img src="https://i.ibb.co/dsBtdH17/image.png" alt="Automatic rifle and burst SMG in the player's first-person view, including their projectile trails" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credit: [Low Poly Character Series 1 SWAT](https://assetstore.unity.com/packages/3d/characters/humanoids/low-poly-character-series-1-swat-404966) by CASTLE BRAVO, used for the pistol, AR, and SMG models.

**Map: Outdoor Terrain and Environment**

For this iteration, we developed the outdoor environment in `Assets/Scenes/Map.unity`, expanding the earlier layout design into a 3D scene. The environment includes forest ground, dirt paths and clearings, mountain surfaces, trees, and a perimeter boundary wall. Earth, moss, and exposed-stone surfaces give the terrain visual variety, while vegetation and other scenery establish the outdoor setting around the warehouse.

The map also includes the warehouse and its loading and storage areas, providing a destination for exploration. Ground and scenery collision support movement through the environment, and baked navigation data provides walkable routes for the enemies. Crates, health pickups, and zombies are placed in the scene so the existing mechanics can be used within the larger environment. Further houses, keypad clues, and visibility effects remain part of the later development plan.

<a href="https://ibb.co/r2pghrG0"><img src="https://i.ibb.co/QvHZVwFQ/image.png" alt="Outdoor map overview and player-level view showing the terrain, paths, forest, and warehouse approach" width="650" /></a>

Asset credits for the environment models and their accompanying textures:

* [Low-Poly Simple Nature Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153) by JustCreate, used for trees, bushes, grass, stumps, and rocks.
* [Low Poly Nature - FREE Vegetation](https://assetstore.unity.com/packages/3d/vegetation/low-poly-nature-free-vegetation-134006) by Elcanetay, used for additional vegetation.
* [Poly_Low_Trees_Pack_FREE](https://assetstore.unity.com/packages/3d/vegetation/trees/poly-low-trees-pack-free-204089) by DinV Studio, used for fir trees, dead trees, and their color texture.
* [Low Poly Storage Pack](https://assetstore.unity.com/packages/3d/environments/urban/low-poly-storage-pack-101732) by Broken Vector, used for storage crates, barrels, pallets, and racks.
* [Low-poly Industrial Pack](https://assetstore.unity.com/packages/3d/props/low-poly-industrial-pack-397642) by Donthrukat, used for industrial scenery and its texture atlas.
* [Wooden House - Free - Low Poly](https://assetstore.unity.com/packages/3d/environments/wooden-house-free-low-poly-270889) by Acronym, used for the wooden house model and texture.

**Crate Model: Wooden Crates**

The crate-search puzzle was already implemented in the previous iteration. For this iteration, we connected the existing puzzle behavior to the imported `Wooden Crate` prefab from the Low Poly Wooden Crate asset pack. Multiple instances of the textured wooden model are placed in the map, giving the puzzle objects a recognizable appearance in the environment.

The prefab uses a wood-textured material and two levels of detail, allowing Unity to display a simpler mesh when the crate appears smaller on screen. A convex MeshCollider provides collision for the crate's shape, while a Rigidbody with gravity enabled lets it respond to physics. The `Life` and `Crate` components are attached to the same root object so projectile hits can damage the crate and trigger its puzzle response.

Each crate starts with 50 health. When its health reaches zero, `Crate.cs` checks whether it is the winning crate selected by `CrateManager.cs`. Breaking an incorrect crate creates a hint pointing toward the winning crate for four seconds, while breaking the winning crate prints a success message. The broken crate is then removed from the scene.

<a href="https://ibb.co/xqQ47MdR"><img src="https://i.ibb.co/LdfGZCH2/image.png" alt="Wooden crate model in the map and its directional hint when broken" width="650" /></a>
<a href="https://ibb.co/hFfzqh6k"><img src="https://i.ibb.co/s9HYBkfh/image.png" alt="Wooden crate model in the map and its directional hint when broken" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credit: [Low Poly Wooden Crate](https://assetstore.unity.com/packages/3d/props/industrial/low-poly-wooden-crate-308305) by Aegis, used for the wooden crate model and textures.

**Puzzle: Keypad and Warehouse Access**

For this iteration, we added an interactive keypad to the `Warehouse` prefab and connected it to the entrance door. The keypad uses the imported Keypad FREE asset, giving the player a physical panel for entering the access code.

`KeypadInteractionFPV.cs` detects left-clicks on the keypad's buttons, while `KeypadButton.cs` animates each press and sends its value to `Keypad.cs`. The entered digits appear on the display, and pressing the keypad's Enter button checks the combination. An incorrect code produces a red display, a denied message, and a sound before clearing the input for another attempt. A correct code produces a green display, a granted message, and a confirmation sound.

The keypad's access-granted event calls `CabinDoor.OpenDoor()` on the warehouse door. The script rotates the door and its collider together through 90 degrees, opening the entrance and keeping it open after access is granted.

At this stage, the clues for the keypad code had not been spread across the map yet. We planned to place them once the map was more fleshed out so we could choose better hiding spots and make finding the clues part of exploring the environment. Their placement is covered in Project Part 3 below.

Clicking the keypad buttons can be difficult right now because we have not added a crosshair yet. We have not covered the UI portion of the course, so the crosshair is planned for a later iteration.

<a href="https://ibb.co/bjYPrBsK"><img src="https://i.ibb.co/LDB8CtxZ/image.png" alt="Keypad beside the warehouse entrance and the door opening after the correct code is entered" width="650" /></a>
<a href="https://ibb.co/93YLnfCj"><img src="https://i.ibb.co/x850hcRQ/image.png" alt="Keypad beside the warehouse entrance and the door opening after the correct code is entered" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credits: [Cabin Environment](https://assetstore.unity.com/packages/3d/environments/cabin-environment-98014) by Gregory Seguru, used for the warehouse building and entrance door; [Keypad FREE](https://assetstore.unity.com/packages/3d/props/electronics/keypad-free-262151) by Navarone, used for the interactive keypad puzzle.

**Health-Pickup Model: Pill Bottle**

For this iteration, we connected the existing healing behavior to a recognizable 3D pickup using the imported `PillBottle` model from the HouseholdItems asset pack. The `Heal` prefab reuses the model and its material, adjusts its scale, and adds a BoxCollider configured as a trigger. This lets the player collect the item by touching it without the pickup acting as a solid obstacle.

`HealCollision.cs` checks that the entering object is tagged `Player` and has a `Life` component. The current prefab restores 20 health, capped at 100. For example, a player at 90 health receives only the remaining 10 health. The pickup is consumed after a successful heal, but remains available when the player is already at full health. This connects the new model to the healing trigger while keeping the health amount configurable in the Inspector.

<a href="https://ibb.co/bjX7Ystb"><img src="https://i.ibb.co/p6dnNfqh/image.png" alt="Pill-bottle health pickup in the map and Inspector views showing the player's health before and after collecting it" width="650" /></a>

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credit: [Low Poly Basic Items Pack - Household Items](https://assetstore.unity.com/packages/3d/props/low-poly-basic-items-pack-household-items-249507) by Kabungus, used for the pill-bottle health-pickup model and its color-palette texture.

**Gun Inventory: Weapon Switching**

For this iteration, we added a slot-based gun inventory using `InventoryManager.cs`. The script builds its inventory from the direct children of the Inventory Manager object, including inactive weapons. Their order in the Hierarchy determines their slot order. At startup, it disables all inventory items and activates the selected starting weapon, which defaults to the first slot.

The slot order is deliberately dynamic: weapons are assigned by their position in the inventory rather than permanently tied to a specific slot number. This design allows us to add a UI in a future iteration where the player can move weapons between slots and rearrange their inventory. For now, the starting order is set through the Hierarchy; rearranging weapons through a UI is planned for later.

The player can press 1, 2, or 3 to select a weapon slot through the `Slot1`, `Slot2`, and `Slot3` input actions. When switching to an available slot, the inventory disables the previous weapon and activates the new one. This keeps one weapon equipped at a time. Selecting an empty slot or pressing the number for the currently equipped weapon leaves the selection unchanged.

At the Part 2 checkpoint, the inventory used weapons assigned in the Hierarchy at the start of the scene. Part 3 extends this system with crate drops and collection: the current player starts with only the pistol and adds the other weapons to slots in collection order.

<a href="Recordings/Switching%20Guns.mp4">Watch the gun inventory demonstration (video)</a>

**Instructions for Testing the Project**

All Part 2 features can be tested in `Assets/Scenes/Map.unity`. Open the scene in Unity, enter Play mode, and click the Game view to give it input focus. Use WASD to move, the mouse to look, Space to jump, and the left mouse button to shoot.

* **Keypad and warehouse door:** From the spawn area, the warehouse with the keypad is behind and to the right of the main warehouse. Click the keypad buttons to enter the access code `5567`, then press the keypad's Enter button to open the door.
* **Guns and inventory:** In the current project, the player starts with only the pistol. Shoot crates and walk over the dropped weapons to collect the AR and Burst SMG, then press 1, 2, or 3 to switch between available slots and test their firing behavior. The Part 2 checkpoint originally provided all three weapons at startup; the current collection behavior is described in Part 3.
* **Healing:** During Play mode, select the player in the Hierarchy and lower Current Health on its `Life` component in the Inspector to a value below 100, such as 50. Return to the Game view and walk over a pill-bottle health pickup. Check the Inspector again to confirm that health increased and the pickup was consumed.
* **Crate puzzle:** Shoot the crate in front of the player to break it and test the puzzle. Breaking an incorrect crate creates a temporary hint pointing toward the correct crate. Breaking the correct crate prints `You found the right crate!` in Unity's Console.
* **Dashing:** Press Left Shift to dash. Try it while moving and while jumping to test both ground and midair dashes.

### Project Part 3: Visual Effects

For this iteration, we expanded the map with farm buildings and dock houses with separate interiors, added environmental and combat effects, and updated weapon collection and movement. We also added shared damage and healing feedback to make changes in health easier to see. The completed features below follow the order of the crossed-out items in the Part 3 development plan.

**Environment: Fog Effects**

For this iteration, we enabled dense, blue-gray fog in `Assets/Scenes/Map.unity` to reduce visibility across the outdoor environment. The scene uses exponential-squared fog with a density of 0.1, causing distant objects to disappear into the mist more quickly than nearby objects. This makes it harder to see enemies and landmarks from far away and supports the game's horror atmosphere.

We also added `FlashlightFogRendererFeature.cs` and `Flashlight Fog.shader` to make the fog respond to the player's flashlight. The renderer uses scene depth to calculate distance fog and reduces its density within the flashlight's cone. Distant scenery blends into the sky background, helping prevent tree and building silhouettes from remaining visible through the fog.

<!-- Replace the src below with a screenshot showing nearby scenery and distant objects fading into fog. -->
<img src="https://i.ibb.co/ZzJc50sN/image.png" alt="Image placeholder: outdoor scenery and distant objects fading into the blue-gray fog" width="650" />

**Environment: Reduced Player Visibility**

For this iteration, we changed the environment's visual presentation so the player cannot clearly see the entire map at once. The scene uses the `Horror Moonless Sky` material, with a reduced exposure of 0.4 and a dark, cool tint. Combined with the fog, this gives the outdoor area a darker appearance and makes distant routes and threats harder to identify.

The flashlight provides a clearer view in the direction the player is looking, while the surrounding environment remains obscured. This encourages the player to explore nearby areas and aim the light toward possible paths or enemies instead of relying on a clear view across the map.

<!-- Replace the src below with a screenshot showing the dark environment outside the flashlight beam. -->
<img src="https://i.ibb.co/NnNm8gDr/image.png" alt="Image placeholder: dark outdoor environment showing the player's limited visibility beyond the flashlight beam" width="650" />

<img src="https://i.ibb.co/ZzNBFZfR/image.png" alt="Image placeholder: outdoor scenery and distant objects fading into the blue-gray fog" width="650" />

Asset credit: [Fantasy Skybox FREE](https://assetstore.unity.com/packages/2d/textures-materials/sky/fantasy-skybox-free-18353) by Render Knight, used for the moonless night sky texture in the customized `Horror Moonless Sky` material.

**Crates: Break Effects**

For this iteration, we added a break effect so destroying a crate produces visible feedback instead of only removing the model. `Crate.cs` listens to the crate's `Life.onBreak` event and creates the assigned `Crate Explosion` effect at the crate's collider center. The effect is separate from the crate, allowing its particles and rubble to remain visible after the broken crate disappears.

The crate prefab scales the effect to fit the model and allows it to remain for 20 seconds before cleanup. The same break response also handles weapon drops and the existing puzzle result: an incorrect crate produces a four-second directional hint, while the winning crate completes the search. A guard prevents repeated health events from creating multiple break effects or drops for one crate.

<a href="Recordings/Crate%20Breaking.mp4">Watch the crate breaking demonstration (video)</a>

Asset credits: [Simple FX - Cartoon Particles](https://assetstore.unity.com/packages/vfx/particles/simple-fx-cartoon-particles-67834) by Synty Studios, used for the explosion and rubble effect adapted into `Crate Explosion`; [Low Poly Wooden Crate](https://assetstore.unity.com/packages/3d/props/industrial/low-poly-wooden-crate-308305) by Aegis, used for the breakable crate model and textures.

**Environment: More Houses Around the Map**

For this iteration, we added a farmhouse and surrounding farm buildings to `Assets/Scenes/Map.unity`, expanding the outdoor environment with another recognizable area to explore. We also added four waterfront dock houses to give the map more buildings and destinations along the water.

Each dock house connects to a separate interior scene: `HouseOne.unity`, `HouseTwo.unity`, `HouseThree.unity`, and `HouseFour.unity`. `HoldDoorInteraction.cs` lets the player enter by holding E for two seconds within two units of the entrance. Inside, the player releases E and holds it again near the door to return outside. The farmhouse and other farm buildings are part of the outdoor scene; the hold-E scene transitions are connected to the dock houses.

`DockHouseTravel.cs` loads the selected interior additively, temporarily disables the outdoor scene's active world objects, and moves the existing player to the interior's `EntranceSpawn`. This preserves the player's health and inventory along with the outdoor scene's state. Leaving restores the outdoor objects, returns the player to their previous position and rotation, and unloads the interior.

<!-- Replace the src below with a screenshot showing the added farmhouse and surrounding farm buildings. -->
<img src="https://i.ibb.co/MxNPNwwJ/image.png" alt="Image placeholder: added farmhouse and surrounding farm buildings in the outdoor map" width="650" />

<a href="Recordings/Room%20Changing.mp4">Watch the dock-house room changing demonstration (video)</a>

Asset credit: [Low-Poly Farm Pack](https://assetstore.unity.com/packages/3d/props/low-poly-farm-pack-395940) by Aletheia, used for the farmhouse and surrounding farm buildings added to the outdoor map.

**Puzzle: Keypad Clues Around the Map**

For this iteration, we spread four paper clues around `Assets/Scenes/Map.unity` to make finding the warehouse's keypad code part of exploring the environment. Each clue uses the `PosterNote` prefab, with a paper surface and TextMesh Pro text that the player can read in the world. The notes are placed at different locations around the map so the player must search for the information before returning to the keypad.

Each note reveals one digit in its position within the four-digit code: `5 0 0 0`, `0 5 0 0`, `0 0 6 0`, and `0 0 0 7`. Combining the revealed digits gives `5567`. The player reads the notes directly and enters the completed code using the existing keypad buttons, connecting exploration to opening the warehouse door.

<!-- Replace the src below with a screenshot showing a paper clue and its code fragment in the map. -->
<img src="https://i.ibb.co/twmZT52T/image.png" alt="Image placeholder: paper clue placed around the map showing one digit of the warehouse keypad code" width="650" />
<img src="https://i.ibb.co/KCdJw3K/image.png" alt="Image placeholder: paper clue placed around the map showing one digit of the warehouse keypad code" width="650" />
<img src="https://i.ibb.co/qYs3nW1z/image.png" alt="Image placeholder: paper clue placed around the map showing one digit of the warehouse keypad code" width="650" />
<img src="https://i.ibb.co/xKbGfgv7/image.png" alt="Image placeholder: paper clue placed around the map showing one digit of the warehouse keypad code" width="650" />

**Weapons: Crate Drops and Collection**

For this iteration, we connected the crate system to collectible weapons. When a configured crate breaks, `Crate.cs` randomly selects one of its assigned weapon pickups. The current crate prefab includes AR and Burst SMG pickups. A downward raycast finds a suitable surface beneath the crate so the dropped weapon appears just above the floor with a randomized horizontal rotation.

`WeaponPickup.cs` uses a trigger to detect the player and passes the weapon type to `InventoryManager.TryCollectWeapon()`. The player now starts with only the pistol, and collected weapons are added to the inventory in collection order. Pressing 1, 2, or 3 switches between available slots. Collecting a duplicate consumes the pickup without adding another slot or changing the equipped weapon; a successful pickup disappears from the ground.

<a href="Recordings/Weapon%20Drop.mp4">Watch the weapon drop and collection demonstration (video)</a>

Asset credit: [Low Poly Character Series 1 SWAT](https://assetstore.unity.com/packages/3d/characters/humanoids/low-poly-character-series-1-swat-404966) by CASTLE BRAVO, used for the AR and SMG models reused as collectible weapon pickups.

**Exploration: Flashlight**

For this iteration, we added the `Player Flashlight` prefab under the first-person camera so the held model follows the player's view. `FlashlightController.cs` starts the light in its enabled state and uses the Flashlight input action, bound to F, to toggle it. Switching the flashlight also changes the bulb's emission so its visible glow matches whether the light is on.

The beam comes from a spotlight at the flashlight head and aims toward a point 12 units ahead of the camera. The current prefab uses a 35-unit range, a 48-degree outer cone, and a 32-degree inner cone. The custom fog renderer adds a visible glow and reduces fog density inside the beam to 20 percent of its normal value, helping the player inspect nearby scenery. Battery consumption and replacement batteries remain future work.

<!-- Replace the src below with a comparison showing the same area with the flashlight on and off. -->
<img src="https://i.ibb.co/qMhLCcs0/image.png" alt="Image placeholder: first-person flashlight model and a comparison of the beam switched on and off" width="650" />

<img src="https://i.ibb.co/Y7GcT8KN/image.png" alt="Image placeholder: first-person flashlight model and a comparison of the beam switched on and off" width="650" />

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credit: [Flashlight](https://assetstore.unity.com/packages/3d/props/electronics/flashlight-18972) by RRFreelance, used for the held flashlight model and textures adapted into the `Player Flashlight` prefab.

**Enemies: Close-Range Player Damage**

For this iteration, we extended `EnemyFSM.cs` with an `Attack` state so zombies can damage the player after approaching them. The zombie still uses sight-based detection and NavMesh navigation, but now stops and attacks when a visible player is within its stopping distance, with a small tolerance for the agent's final position. The current zombie prefab uses a stopping distance of 1.7 units.

Each attack deals 10 damage through the player's `Life` component, with a one-second interval between attacks. The script checks for obstacles again at the moment of impact to prevent damage through walls, and leaving and reentering attack range does not reset the cooldown. Dead enemies and players cannot continue fighting. When the player's health reaches zero, the configured `Life.PlayerDied()` response logs `you died` and removes the player object.

<!-- Replace the src below with a screenshot showing a nearby zombie and the player's reduced health in the Inspector. -->
<img src="https://i.ibb.co/RGjDmbLq/image.png" alt="Image placeholder: zombie attacking at close range and the player's health decreasing in the Inspector" width="650" />

<sub>This screenshot cannot show the full motion and behavior of the feature. Please test it in-game.</sub>

Asset credit: [3D Characters Zombie City Streets Lowpoly Pack - Lite](https://assetstore.unity.com/packages/3d/characters/humanoids/3d-characters-zombie-city-streets-lowpoly-pack-lite-277409) by CatBorg Studio, used for the zombie enemy model with the updated attack behavior.

**Combat: Enemy Hit Effects**

For this iteration, we added `ZombieDamageEffects.cs` to give successful shots a visible impact response. `ContactDamager.cs` checks the projectile's traveled path and reports the hit point and surface normal through `Life.TakeProjectileDamage()`. The zombie's effect script listens for this projectile-hit event and creates a blood spray just outside the struck surface, facing outward from the impact.

When a zombie's health reaches zero, its depletion event calls `ZombieDamageEffects.Explode()`. This creates a burst of chunks around the visible body's center and removes the zombie. Both effects are independent of the enemy object, so they can finish after it disappears, and they are removed after four seconds. The death effect is protected against repeated calls.

<a href="Recordings/Enemy%20Hit%20Effect.mp4">Watch the enemy hit effect demonstration (video)</a>

Asset credit: [Blood Gush](https://assetstore.unity.com/packages/vfx/particles/blood-gush-73426) by RRFreelance, used for the blood-spray and chunk particle effects adapted into `Zombie Blood Spray` and `Zombie Death Chunks`, with materials adjusted for URP.

**Combat: Bullet and Muzzle Effects**

For this iteration, we added muzzle flashes to the pistol, automatic rifle, and Burst SMG. Each firing script calls the shared `WeaponMuzzleFlash.Play()` helper when it launches a projectile. The helper creates the assigned effect at the weapon's Bullet Spawn transform and parents it to the muzzle so it follows the barrel while the player moves or aims.

The effect uses the weapon's layer and is removed after half a second, including when switching weapons hides its parent. The shared bullet prefab also has a TrailRenderer that leaves a short trail behind each projectile, making its direction of travel easier to see. Together, the muzzle flash and projectile trail provide visual feedback for single shots, automatic fire, and each shot in a burst.

<a href="Recordings/Muzzle%20Flash.mp4">Watch the muzzle flash demonstration (video)</a>

Asset credit: [War FX](https://assetstore.unity.com/packages/vfx/particles/war-fx-5669) by Jean Moreno, used for the muzzle-flash prefab and textures adapted into `Weapon Muzzle Flash`, with a custom shader and materials for URP.

**Puzzle: Larger Keypad and Button Colliders**

For this iteration, we enlarged the keypad attached to the warehouse entrance to make its buttons easier to click. The keypad instance in `Warehouse.prefab` uses a uniform local scale of 1.822. Scaling the panel also enlarges the child button colliders, giving the player's interaction ray larger targets.

The keypad keeps its existing code-entry behavior. `KeypadInteractionFPV.cs` casts a ray from the camera through the mouse position when the player clicks, and a hit on a `KeypadButton` presses that button. The larger panel improves access to the digits and Enter button while preserving the code check and warehouse-door opening response.

<!-- Replace the src below with a screenshot showing the enlarged keypad beside the warehouse door. -->
<img src="https://i.ibb.co/ymMDhDXt/image.png" alt="Image placeholder: enlarged keypad beside the warehouse entrance with its larger clickable buttons" width="650" />

Asset credits: [Keypad FREE](https://assetstore.unity.com/packages/3d/props/electronics/keypad-free-262151) by Navarone, used for the keypad model, buttons, and interaction scripts; [Cabin Environment](https://assetstore.unity.com/packages/3d/environments/cabin-environment-98014) by Gregory Seguru, used for the warehouse building and entrance door beside the enlarged keypad.

**Movement: Crouch and Slide**

For this iteration, we added `PlayerCrouchSlide.cs` to let the player crouch and slide. Pressing C toggles crouching, which halves the player's capsule collider height, keeps the feet at ground level, and smoothly lowers the camera. Crouched movement is limited to 3 units per second. Before standing, the script checks for headroom so the player stays low beneath an obstacle and stands once there is enough space.

Pressing Left Ctrl while standing on the ground and holding a movement direction starts a slide. The player immediately crouches and slides in the current direction of travel, using the input direction when starting from rest. The current player prefab uses a starting speed of at least 25 units per second and reduces it over time until it reaches 10 units per second. The slide then ends and returns the player to their selected crouch or standing state, subject to available headroom. A one-second cooldown prevents consecutive slides. While standing on the ground, pressing Left Ctrl without directional input crouches instead. While crouched, pressing Left Ctrl requests standing, even with a movement direction held, and does not start a slide. C still toggles crouching, and both controls respect the headroom check.

<a href="Recordings/Crouch%20And%20Slide.mp4">Watch the crouch and slide demonstration (video)</a>

**Player Feedback: Healing Effect**

For this iteration, we added a green screen-edge flash when the player collects a health pickup. `HealCollision.cs` restores the configured health amount, capped at 100, and calls `PlayerDamageFeedback.ShowHealFlash()` after a successful heal. The current health-pickup prefab restores 20 health. A pickup is consumed only when it restores health, so it remains available when the player is already at full health.

We also enlarged the healing bottle to give it a larger pickup collider, making it easier for the player to collect by walking over it.

The healing effect is saved in `Assets/Post Processing/Player Healing Profile.asset`, a URP Volume Profile containing a green Vignette override with intensity 0.8 and smoothness 0.22. Both player prefabs reference this asset through the `Heal Profile` field on `PlayerDamageFeedback`. A successful heal switches the feedback volume to this profile, enables it, and fades its blend weight over 0.7 seconds using unscaled time before disabling it. A new heal restarts the fade; taking damage switches to the red damage profile. Changing `Life.currentHealth` directly in the Inspector changes health without triggering either flash.

<a href="Recordings/Healing%20Effect.mp4">Watch the healing effect demonstration (video)</a>

Asset credit: [Low Poly Basic Items Pack - Household Items](https://assetstore.unity.com/packages/3d/props/low-poly-basic-items-pack-household-items-249507) by Kabungus, used for the enlarged pill-bottle health-pickup model and its color-palette texture.

**Player Feedback: Taking Damage**

For this iteration, we added `PlayerDamageFeedback.cs` to show when the player takes damage. The script listens to the `Life.Damaged` event and flashes a red URP post-processing vignette. The effect is strongest at the edges, keeping the player's central view clear during combat.

The red effect is saved in `Assets/Post Processing/Player Damage Profile.asset`, with Vignette intensity 0.8 and smoothness 0.22. Both player prefabs reference it through the `Damage Profile` field. Taking a hit switches to this profile, enables the feedback volume at full blend weight, and restarts the smooth 0.7-second fade. The fade uses unscaled time so it can finish even if gameplay time is paused or slowed. Only positive damage accepted by `Life.TakeDamage()` triggers the red flash. A successful health pickup switches the same volume to the green healing profile.

`PlayerDamageFeedback.cs` enables post-processing on the player's child camera and includes the player's layer in its volume mask. It creates a temporary `Player Feedback Volume` under the player and toggles between the two saved profiles without creating, changing, or destroying profile assets at runtime. Color, intensity, and smoothness can be edited directly in each profile's Vignette override; `fadeDuration` remains configurable on the player component. Disabling the component clears the effect, and destroying it removes the temporary volume.

<a href="Recordings/Taking%20Damage.mp4">Watch the taking damage demonstration (video)</a>

**Instructions for Testing the Project**

Open `Assets/Scenes/Map.unity` in Unity with the house interior scenes closed, enter Play mode, and click the Game view to give it input focus. Start house-travel tests from Map rather than opening an interior directly. Use WASD to move, the mouse to look, Space to jump, Left Shift to dash, C to toggle crouching, Left Ctrl to crouch, stand, or slide depending on stance and movement input, F to toggle the flashlight, 1, 2, or 3 to select collected weapons, and the left mouse button to shoot. Hold E near a dock-house door to enter or leave.

* **Fog and reduced visibility:** Look across the outdoor area and compare nearby scenery with distant trees and buildings. Turn the flashlight off with F to inspect visibility outside its beam.
* **Crate effects:** Shoot a crate until it breaks. Observe the explosion and rubble continuing after the crate disappears. An incorrect crate shows a temporary hint toward the winning crate; breaking the winning crate logs `You found the right crate!` in the Console.
* **Added houses and dock-house interiors:** Explore the farmhouse and surrounding farm buildings, then approach one of the four waterfront dock-house entrances. Hold E for two seconds within two units of the entrance to enter its separate interior. Holding E for less than two seconds or moving out of range should cancel progress. Release E after entering, then hold it again for two seconds near the interior door to return to the previous outdoor position and rotation. Check that health, collected weapons, and changes such as broken crates remain preserved. Repeat with the other dock houses and confirm that continuing to hold E through a transition does not immediately send the player back.
* **Keypad clues around the map:** Find the four paper notes and combine their code fragments, `5 0 0 0`, `0 5 0 0`, `0 0 6 0`, and `0 0 0 7`, to obtain `5567` for the warehouse keypad.
* **Weapon drops and collection:** The player starts with only the pistol. Break crates and walk over dropped AR or Burst SMG pickups to collect them. Press 1, 2, or 3 to select available slots, which fill in collection order. Check that collecting a duplicate consumes the pickup without adding another slot or switching the equipped weapon.
* **Flashlight:** Press F to toggle the light and look around to check that the beam follows the camera. Compare the same scenery with the flashlight on and off.
* **Zombie attacks:** Let a zombie detect and approach the player, and inspect the player's `Life.currentHealth` to confirm each attack removes 10 health at one-second intervals. Move out of reach or behind an obstacle to stop attacks. Leaving and reentering range should not bypass the attack cooldown.
* **Enemy hit effects:** Shoot a zombie and check that blood sprays from the impact point. Defeat it to see the death-chunk effect continue after the zombie disappears, then clear after about four seconds.
* **Bullet and muzzle effects:** Fire the pistol and collected weapons to inspect their muzzle flashes and projectile trails. Move or aim while firing to check that the flash follows the barrel, and switch weapons to check that flashes do not remain stuck on screen.
* **Larger keypad:** Approach the warehouse keypad, click its buttons to enter `5567`, and press the keypad's Enter button to open the door.
* **Crouch and slide:** Press C to crouch and stand, checking the camera height and slower movement. While standing on the ground with no directional input, press Left Ctrl to crouch, then press it again to stand. Repeat while holding a direction after crouching and confirm Left Ctrl stands the player up without starting a slide. Try standing beneath a low obstacle with either control to check the headroom protection. While standing on the ground, hold a movement direction and press Left Ctrl to slide, then observe the slowdown and return to the selected stance. Check that another slide requires the one-second cooldown.
* **Healing effect:** Take damage, or set the player's `Life.currentHealth` to 50 in the Inspector during Play mode. Editing health directly should not trigger a flash. Return to the Game view and walk over an enlarged healing bottle: health should rise to 70, the bottle should disappear, and a green vignette should fade over about 0.7 seconds. Repeat at 90 health to confirm healing stops at 100. At 100 health, the bottle should remain available without producing a flash.
* **Taking damage:** Let a zombie hit the player and check that each health loss produces a red vignette fading over about 0.7 seconds. The saved damage and healing profiles both use intensity 0.8 and smoothness 0.22. To check that a fresh hit restarts an active fade, temporarily increase the player's `PlayerDamageFeedback.fadeDuration` above the zombie's one-second attack interval during Play mode. Collect a bottle while the red flash is visible to check that green replaces it; another hit should replace green with red. The shared effect should restart its fade each time and clear completely afterward. Stop Play mode to discard temporary component settings.
* **Saved feedback profiles:** In the Project window, open `Assets/Post Processing/Player Damage Profile.asset` and `Player Healing Profile.asset` to inspect their red and green Vignette overrides. Check that both player prefabs have their `Damage Profile` and `Heal Profile` references assigned. During Play mode, select the player's `Player Feedback Volume` child and observe its profile switching on damage and healing, its weight fading to zero, and the Volume component disabling after the flash. Check that the saved profiles' intensity remains 0.8 throughout the fade.
