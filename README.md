# SeeloewenCraft

SeeloewenCraft is a simple 2D sandbox/survival game. You can create and save worlds, break and place blocks to build whatever you like, gather resources to craft new items and much more. There's also a lot more planned!\
This game is developped as a fun/learning project and is not meant to be publicly available. It was just made for me to learn C# and WPF a little bit better. This also means that it might not be actively maintained and development may stop at some point.
This repository is not used to distribute it, but rather for collaboration and to have it stored in a save place.

# Features
Here's an overview of some of the most important features. This does of course not include everything. There is much more to discover than you can see at a first glance, so I recommend that you jump right in and find out for yourself!

## Random World Generation
![image](https://github.com/user-attachments/assets/51d70b9e-f96d-46cf-a010-355813e3bc6e)

The world generation is based on a randomized seed, meaning you will most likely never get the same world twice. Besides random terrain generation, there are also a bunch of structures with randomized loot. There is also no theoretical limit on how many chunks you can generate, there may be some issues though if you go too far out. The world being seeded also means that you can generate specific worlds if you have the seed for it.

## Crafting
![image](https://github.com/user-attachments/assets/24a0d04a-131e-45b4-bb9e-6db8d12492e2)

Using the materials that you can gather with tools or exploration, you can craft new items that will be helpful in your journey.

## Multiplayer
![image](https://github.com/user-attachments/assets/42c7df65-5b3e-45a6-94f8-8671cfd06fa2)

That's right! Using TCP you can play with your friends! One person has to host the server and open the port (5000 by default) and the others can connect. You may also use third-party-software like VPNs to create virtual networks.\
Please note that Multiplayer is currently very unstable and insecure. Using it, especially with the wrong people, may put your computer at a risk. The creators of the game are not liable for any damage caused by Multiplayer or the game in general. Use at your own risk.

## Credits
The game has been created by Seeloewen and CDLemmi.

Special thanks to Finni for helping me find many issues by testing developmen versions and also for giving a lot of ideas.\
Special thanks to Axogurkel for helping me create some crafting recipes.

Third-party libraries used are https://github.com/mzboray/HighPrecisionTimer and https://github.com/JamesNK/Newtonsoft.Json.
