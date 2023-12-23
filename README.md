# [CS2] Connect-Disconnect-Sound (1.0.0)

### Connect , Disconnect , Message , Sound , Logs


## .:[ Dependencies ]:.
[Metamod:Source (2.x)](https://www.sourcemm.net/downloads.php/?branch=master)

[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/releases)

## .:[ Configuration ]:.
```json
{
  //----------------------------------------------------------------------------------------------------------------//
  // you can use these in Connect or Disconnect Message
  //{PLAYERNAME} == Player Who Joined
  //{STEAMID} = STEAM_0:1:122910632
  //{STEAMID64} = 76561198206086993

  "ConnectPlayers": "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {lime}Connected To The Server {STEAMID}",
  "DisconnectPlayers": "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {red}Disconnected To The Server {STEAMID}",
  //----------------------------------------------------------------------------------------------------------------//


  // you can test any sound path ingame console type "play <soundpath>"
  // Sound Path will in  https://github.com/oqyh/cs2-Connect-Disconnect-Sound/sounds/sounds.txt
  "ConnectSound": false,
  "ConnectSoundPath": "sounds/buttons/bell1.vsnd_c",
  "DisconnectSound": false,
  "DisconnectSoundPath": "sounds/buttons/blip1.vsnd_c",
  //----------------------------------------------------------------------------------------------------------------//


  // Enable Logs Will be in ../addons/counterstrikesharp/plugins/CnD_Sound/logs/
  "CnDModeLogs": false,
  // Log File Format .txt or .pdf ect...
  "LogFileFormat": ".txt",
  // Date Time Formate
  "LogFileDateFormat": "MM-dd-yyyy",
  "LogInsideFileTimeFormat": "HH:mm:ss",
  // Connect or Disconnect Message Inside Log
  "ConnectPlayersLog": "{PLAYERNAME} Connected SteamdID:{STEAMID}",
  "DisconnectPlayersLog": "{PLAYERNAME} Disconnected SteamdID:{STEAMID}",
  //----------------------------------------------------------------------------------------------------------------//
  "ConfigVersion": 1
}
```


## .:[ Change Log ]:.
```
(1.0.0)
-Initial Release
```

## .:[ Donation ]:.

If this project help you reduce time to develop, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/oQYh)
