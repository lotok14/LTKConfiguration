# LTK Configuration
This is an `Ultimate Chicken Horse`  mod that adds a bunch of ways to customise how some parts of the game work.
> [!WARNING]
> This has only been tested in local play, It probably doesn't work online.

# Table Of Contents
- [Setup Guide](#setup-guide)
- [Features](#features)
  * [Beehive Points](#beehive-points)
      - [showcase](#showcase)
      - [config](#config)
  * [Collapsing Block Repair](#collapsing-block-repair)
      - [showcase](#showcase-1)
      - [config](#config-1)
  * [Jetpack Fuel](#jetpack-fuel)
      - [showcase](#showcase-2)
      - [config](#config-2)
  * [Stopwatch Custom Values](#stopwatch-custom-values)
      - [showcase](#showcase-3)
      - [config](#config-3)
- [Credits](#credits)

# Setup Guide
1. Ensure `bepinex` is downloaded first
2. Head over to the [latest release](https://github.com/lotok14/LTKConfiguration/releases/latest) and download `LTKConfiguration.zip`
3. Unpack the zip file. You should have something like this:
 - LTKConfiguration
     - Assets
       - [...]
      - LTKConfiguration.dll
4. Drag the LTKConfiguration folder with its contents into `ultimate chicken horse/BepInEx/Plugins`
5. Run the game once and then close it
6. Open `ultimate chicken horse/BepInEx/config/LTK.uch.LTKConfiguration.cfg` and edit it to your preferences

# Features
## Beehive Points
#### showcase
![Bee Points Gif](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/beePoints.gif)
#### config
| name                   | description                                                                        | default value |
|------------------------|------------------------------------------------------------------------------------|---------------|
| Beehive Points Enabled | When set to true, gives you points whenever you finish while being chased by bees  | false         |
| Beehive Points Amount  | Determines how many points you get for finishing with a hive (value * coin points) | 0.6           |
## Collapsing Block Repair
#### showcase
![Repairing Platform Gif](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/repairingPlatform.gif)
#### config
| name         | description                                                           | default value |
|--------------|-----------------------------------------------------------------------|---------------|
| Repair       | When set to true, repairs the magnet block after a set amount of time | false         |
| Repair Delay | Determines how many seconds before it gets repaired                   | 7             |
## Jetpack Fuel
#### showcase
![Jetpack Fuel Gif](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/JetpackFuel.gif)
#### config
| name        | description                                                             | default value |
|-------------|-------------------------------------------------------------------------|---------------|
| Use fuel    | When set to true, players lose their jetpacks when they run out of fuel | false         |
| Fuel Amount | Determines how many seconds a player can use their jetpack for          | 1             |
## Stopwatch Custom Values
#### showcase
![Custom Stopwatch Gif](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/customStopwatch.gif)
#### config
| name               | description                                                           | default value |
|--------------------|-----------------------------------------------------------------------|---------------|
| Stopwatch speed    | the number that time gets multiplied by when a stopwatch is picked up | 0.5           |
| Stopwatch duration | how many seconds the stopwatch lasts for                              | 6             |
| Always respawn     | When set to true, the stopwatch will respawn after the round          | false         |

# Credits
- [Clever Endeavour Games](https://www.cleverendeavourgames.com/)
- [BepInEx](https://github.com/BepInEx/BepInEx) team
- [Harmony](https://github.com/pardeike/Harmony) by Andreas Pardeike
