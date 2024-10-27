# LTK Configuration
This is an `Ultimate Chicken Horse`  mod that adds a bunch of ways to customise how some parts of the game work.
> [!WARNING]
> This has only been tested in local play, It might not work online.

# Table Of Contents
- [Setup Guide](#setup-guide)
- [Features](#features)
  * [Beehive Points](#beehive-points)
  * [Collapsing Block Repair](#collapsing-block-repair)
  * [Hockey Indicator](#hockey-indicator)
  * [Jetpack Fuel](#jetpack-fuel)
  * [Stopwatch Custom Values](#stopwatch-custom-values)
  * [Double Teleporter](#double-teleporter)
- [Other](#other)
  * [Changing the assets](#changing-the-assets)
  * [Adding custom points](#adding-custom-points)
- [Credits](#credits)

# Setup Guide
1. Ensure [bepinex](https://docs.bepinex.dev/articles/user_guide/installation/index.html) is downloaded first
2. Head over to the [latest release](https://github.com/lotok14/LTKConfiguration/releases/latest) and download `LTKConfiguration.zip`
> [!TIP]
> You can also download `LTKConfiguration.cfg` to use the recommended config. The automatically generated one is fully vanilla by default.
3. Unpack the zip file. You should have something like this:
 - LTKConfiguration
     - Assets
       - [...]
      - LTKConfiguration.dll
4. Drag the LTKConfiguration folder with its contents into `ultimate chicken horse/BepInEx/Plugins`
5. Run the game once and then close it
6. Open `ultimate chicken horse/BepInEx/config/LTK.uch.LTKConfiguration.cfg` and edit it to your preferences
 - if you've downloaded `LTKConfiguration.cfg` in step 2, you can just replace the file

# Features
## Beehive Points
Gives the player who finished with bees chasing them points
#### showcase
![Bee Points Gif](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/beePoints.gif)
#### config
| name                        | description                                                                        | default value |
|-----------------------------|------------------------------------------------------------------------------------|---------------|
| Beehive Points Enabled      | When set to true, gives you points whenever you finish while being chased by bees  | false         |
| Beehive Points Amount       | Determines how many points you get for finishing with a hive (value * coin points) | 0.6           |
| Beehive Points Always Award | Whether beehive points should always be awarded                                    | true          |

## Collapsing Block Repair
Repairs the magnet platform a few seconds after it has collapsed
#### showcase
![Repairing Platform Gif](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/repairingPlatform.gif)
#### config
| name         | description                                                           | default value |
|--------------|-----------------------------------------------------------------------|---------------|
| Repair       | When set to true, repairs the magnet block after a set amount of time | false         |
| Repair Delay | Determines how many seconds before it gets repaired                   | 7             |

## Hockey Indicator
Adds an icon on the top left that shows when hockey shooters are going to shoot
#### showcase
![Hockey Indicator](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/HockeyIndicator.gif)
#### config
| name                  | description                                                  | default value |
|-----------------------|--------------------------------------------------------------|---------------|
| Show Hockey Indicator | When set to true, shows an icon when hockey is about to fire | false         |


## Jetpack Fuel
Gives jetpacks a limited amount of fuel that they can use before the players lose them
#### showcase
![Jetpack Fuel Gif](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/JetpackFuel.gif)
#### config
| name        | description                                                             | default value |
|-------------|-------------------------------------------------------------------------|---------------|
| Use fuel    | When set to true, players lose their jetpacks when they run out of fuel | false         |
| Fuel Amount | Determines how many seconds a player can use their jetpack for          | 1             |

## Stopwatch Custom Values
Allows for the user to customize the stopwatch a little more
#### showcase
![Custom Stopwatch Gif](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/customStopwatch.gif)
#### config
| name               | description                                                           | default value |
|--------------------|-----------------------------------------------------------------------|---------------|
| Stopwatch speed    | the number that time gets multiplied by when a stopwatch is picked up | 0.5           |
| Stopwatch duration | how many seconds the stopwatch lasts for                              | 6             |
| Always respawn     | When set to true, the stopwatch will respawn after the round          | false         |

# Double Teleporter
Allows the user to make the portals always spawn in pairs
### showcase
![Teleporter png](https://github.com/lotok14/LTKConfiguration/blob/main/github%20media/Teleporter.png)
#### config
| name               | description                                 | default value |
|--------------------|---------------------------------------------|---------------|
| Double Teleporters | Makes the teleporters always spawn in pairs | false         |

# Other
 ## Changing the assets
  It's described on the wiki [here](https://github.com/lotok14/LTKConfiguration/wiki/Custom-assets)
 ## Adding custom points
  It's described on the wiki [here](https://github.com/lotok14/LTKConfiguration/wiki/Creating-a-custom-pointBlock)

# Credits
- [Clever Endeavour Games](https://www.cleverendeavourgames.com/)
- [BepInEx](https://github.com/BepInEx/BepInEx) team
- [Harmony](https://github.com/pardeike/Harmony) by Andreas Pardeike
