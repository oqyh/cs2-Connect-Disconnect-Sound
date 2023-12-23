# [CS2] Connect-Disconnect-Sound (1.0.1)

### Connect , Disconnect , Message , Sound , Logs

![join](https://github.com/oqyh/cs2-Connect-Disconnect-Sound/assets/48490385/174218a8-c639-4921-9418-04da92eff24b)
![log](https://github.com/oqyh/cs2-Connect-Disconnect-Sound/assets/48490385/854b2103-1b82-4bb5-842d-3d5a3b6fe4b3)


## .:[ Dependencies ]:.
[Metamod:Source (2.x)](https://www.sourcemm.net/downloads.php/?branch=master)

[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/releases)

## .:[ Configuration ]:.
```json
{
  // you can use these in Connect or Disconnect Message
  //{PLAYERNAME} == Player Who Joined
  //{STEAMID} = STEAM_0:1:122910632
  //{STEAMID64} = 76561198206086993
  //{IP} = 127.0.0.0
  //Colors Available = "{default} {white} {darkred} {green} {lightyellow}" "{lightblue} {olive} {lime} {red} {lightpurple}"
                      //"{purple} {grey} {yellow} {gold} {silver}" "{blue} {darkblue} {bluegrey} {magenta} {lightred}" "{orange}"

  "ConnectPlayers": "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {lime}Connected To The Server {STEAMID}",
  "DisconnectPlayers": "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {red}Disconnected To The Server {STEAMID}",


  // you can test any sound path ingame console type "play <soundpath>"
  // Sound Path will in  https://github.com/oqyh/cs2-Connect-Disconnect-Sound/blob/main/sounds/sounds.txt
  "ConnectSound": false,
  "ConnectSoundPath": "sounds/buttons/bell1.vsnd_c",
  "DisconnectSound": false,
  "DisconnectSoundPath": "sounds/buttons/blip1.vsnd_c",


  // If Its Enabled Logs Will Located in ../addons/counterstrikesharp/plugins/CnD_Sound/logs/
  "CnDModeLogs": false,
  // Log File Format .txt or .pdf ect...
  "LogFileFormat": ".txt",
  // Date and Time Formate
  "LogFileDateFormat": "MM-dd-yyyy",
  "LogInsideFileTimeFormat": "HH:mm:ss",

  // you can use these in Connect or Disconnect Message
  //{PLAYERNAME} == Player Who Joined
  //{STEAMID} = STEAM_0:1:122910632
  //{STEAMID64} = 76561198206086993
  //{IP} = 127.0.0.0
  "ConnectPlayersLog": "{PLAYERNAME} Connected SteamdID:{STEAMID}",
  "DisconnectPlayersLog": "{PLAYERNAME} Disconnected SteamdID:{STEAMID}",

  "ConfigVersion": 1
}
```


## .:[ Change Log ]:.
```
(1.0.1)
-Added {IP} To
"ConnectPlayers"
"DisconnectPlayers"
"ConnectPlayersLog"
"DisconnectPlayersLog"

(1.0.0)
-Initial Release
```

## .:[ Donation ]:.

If this project help you reduce time to develop, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/oQYh)
