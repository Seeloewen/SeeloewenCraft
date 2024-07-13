# Texturepack Documentation

Since version Alpha 1.1.3 the game also supports changing the vanilla textures to whatever you want! You can create your own texturepacks or download other ones to modify all of the game's blocks and items and also parts of the GUI.

## How are texturepacks structured?
The root of a texturepack is a basic folder. In there are two components: The 'textures' folder and the 'pack.txt' file.

The textures folder contains all the textures of the game. Those are also categorised into subfolders. The exact folder structure can be seen when going into the 'Resources' folder in the root of this repository, that's basically the texture folder for internal usage. Make sure your folder structure and also file names match the one provided here.

The pack.txt file contains all the general information about the texturepack. You can find an example structure with the detailed explanation down below.

## How is pack.txt structured?

Line 1: This line gets ignored by the game. It can contain the name, author or some other information.\
Line 2: This line contains the texturepackVersion in the format 'texturepackVersion=1'. The texturepack version gets changed when there are massive changes to the texturepack structure or the textures in general. The addition of new textures does generally not change the version.

**Example:**
```
//Debug Texturepack, made by Seeloewen for Alpha 1.1.6
texturepackVersion=1
```

## How can texturepacks be installed?
Head into the game folder which is located in Appdata > Roaming > SeeloewenCraft. In there is a texturepacks folder. Simply put your texturepack into that folder and it will show up ingame.

Ingame you can open the settings and select the texturepack from the list to apply it. You may need to restart your game for the textures to show up correctly.
