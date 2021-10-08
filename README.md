# vBergaaBot

## version 2.0.0

This repository contains a C# bot to play Starcraft 2. The skeleton of this bot cloned from here (https://github.com/NikEyX/SC2-CSharpe-Starterkit) which is a blank bot that intergrates the ladder manager and sets up the connections to the SC2Api.

from there I stripped most of the built in methods and rewrote and restructured the logic. The current iteration of the bot beats the ingame ai on its hardest setting against all three races 90% of the time.

### To view the bot in action
First you will have to install some things, if you haven't already.
1. Download [Starcraft 2](https://starcraft2.com/)
2. Install [Visual studio](https://www.visualstudio.com/downloads/). In the installer, choose the .NET desktop development IDE.
3. Go [here](https://github.com/Blizzard/s2client-proto#downloads) and download the Season 3 map pack.
4. Extract the maps to 'C:\Program Files (x86)\StarCraft II\Maps\'. Make sure you have extracted the maps directly into the folder, if they aren't in exactly the right place the program will be unable to find them.

From there, run the project through visual studio to watch the bot play agaisnt the ai on elite.

If you wish to modify this bot, you are welcome to it. Please follow these steps to prepare the bot to the ladder manager. 

### Preparing the bot for your own use.
In this section you will prepare the bot for your own use. It is imortant that you have picked a name for your bot! You will have to rename a number of files for your bot. It is important to rename these files if you intend to use your bot on the ladder, since the bot will have to have a unique filename.

1. Clone or download this repository.
2. Open VBergaaaBot.sln in Visual Studio.
3. Open VBergaaaBot.cs in the ExampleBot project.
4. Rename the VBergaaaBot class to the name of your bot. You can rename the class by right clicking on it and selecting Rename.
5. Rename the VBergaaaBot.cs file to the name of your bot.
6. Rename the VBergaaaBot namespace.
7. Right click on the VBergaaaBot project and go to properties. In the Application tab set the Assembly name to the name of your bot. This will determine the name of your bot's .exe file.
8. Open the Program.cs file inside the Launcher project and set the botName variable to the name of your bot.
9. Right click on the Launcher Project and rename the Assembly name to <YourBot>Launcher. This name will have to be unique among existing bots, or you won't be able to upload to the [ladder](http://sc2ai.net).


### Preparing your bot for the ladder
You will need to take a number of steps to prepare your bot for the ladder.
1. Make sure all the projects are built by right clicking the ExampleBot solution and choosing 'Build Solution' (shortcut ctrl-shift-b).
2. Create a new folder with the name of your bot. This will contain the files you upload to the ladder.
3. Copy the <YourBot>Launcher.exe file generated by the BotLauncher project into the new folder. This can be found under BotLauncher/bin/Debug
4. Inside the folder, create another folder called data and inside that another folder with the name of your bot.
5. Copy the contents of ExampleBot/bin/Debug into the <YourBot>/data/<YourBot> folder you created in step 4.
6. Now start the <YourBot>Launcher.exe. If everything has been done correctly it should start a game with your bot against the Blizzard AI.
7. You can now create a zip file from the folder created at step 2 by right clicking on the folder and selecting Send to -> Compressed (zipped) folder.
8. You can upload the zipped file to the [ladder](http://sc2ai.net). You may have to create an account first.

### Playing your bot against other bots through the LadderManager
You can also run the LadderManager locally. The following instructions will allow you to do so.
1. Follow the instructions [here](https://github.com/Cryptyc/Sc2LadderServer#developer-install--compile-instructions-windows) to install the LadderManager. If you only have the .NET version of visual studio you may also need to install the C++ version. This can be done by using the visual studio installer and choosing the C++ IDE.
2. Move the contents of the folder created in 'Preparing your bot for the ladder' to the ExeDirectory.
3. Add your bot to the LadderBots.json file.
4. Add a game between your bot and another to the matchuplist.
