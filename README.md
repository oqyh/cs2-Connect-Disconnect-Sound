# [CS2] Connect-Disconnect-Sound (1.0.2)

### Connect , Disconnect , Country , City , Message , Sound , Logs , Discord

![new](https://github.com/oqyh/cs2-Connect-Disconnect-Sound/assets/48490385/d91ed87b-15f6-412e-bb82-e1262fa5573e)
![new2](https://github.com/oqyh/cs2-Connect-Disconnect-Sound/assets/48490385/2fc00ccb-0454-47ad-ab5c-0b9b1c2fd4fb)
![new3](https://github.com/oqyh/cs2-Connect-Disconnect-Sound/assets/48490385/f7af3325-7675-4103-ba78-2a7d681ba3c2)


## .:[ Dependencies ]:.
[Metamod:Source (2.x)](https://www.sourcemm.net/downloads.php/?branch=master)

[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/releases)

[GeoLite2-City.mmdb](https://github.com/P3TERX/GeoLite.mmdb) (Country, City, databases) [[Must Be inside ../addons/counterstrikesharp/plugins/CnD_Sound/GeoLocation/]]

[MaxMind.Db](https://www.nuget.org/packages/MaxMind.Db) (Geo Locations)

[MaxMind.GeoIP2](https://www.nuget.org/packages/MaxMind.GeoIP2) (Geo Locations)

[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json) (Discord WebHook)



## .:[ Configuration ]:.
```json
{
  // you can use these in Connect or Disconnect Message
  //{PLAYERNAME} == Player Who Joined
  //{LONGCOUNTRY} == ex: United Arab Emirates
  //{SHORTCOUNTRY} == ex: AE
  //{CITY} == ex: AE
  //{STEAMID} = STEAM_0:1:122910632
  //{STEAMID64} = 76561198206086993
  //{IP} = 127.0.0.0
  //Colors Available = "{default} {white} {darkred} {green} {lightyellow}" "{lightblue} {olive} {lime} {red} {lightpurple}"
                      //"{purple} {grey} {yellow} {gold} {silver}" "{blue} {darkblue} {bluegrey} {magenta} {lightred}" "{orange}"
  //TO DISABLE MAKE IT "" empty
  "ConnectPlayers": "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {lime}Connected {SHORTCOUNTRY} {CITY}",
  "DisconnectPlayers": "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {red}Disconnected {SHORTCOUNTRY} {CITY}",


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
  //{LONGCOUNTRY} == ex: United Arab Emirates
  //{SHORTCOUNTRY} == ex: AE
  //{CITY} == ex: AE
  //{STEAMID} = STEAM_0:1:122910632
  //{STEAMID64} = 76561198206086993
  //{IP} = 127.0.0.0
  //TO DISABLE MAKE IT "" empty
  "ConnectPlayersLog": "[Playername:{PLAYERNAME}] CONNECTED TO THE SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]",
  "DisconnectPlayersLog": "[Playername:{PLAYERNAME}] DISCONNECTED FROM SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]",
  //Send Log To Discord Via WebHookURL
  "SendLogToWebHook": false,
  "WebHookURL": "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",

  "ConfigVersion": 1
}
```


## .:[ Change Log ]:.
```
(1.0.2)
-Added {LONGCOUNTRY}  {SHORTCOUNTRY} {CITY} To
"ConnectPlayers"
"DisconnectPlayers"
"ConnectPlayersLog"
"DisconnectPlayersLog"
-Added "SendLogToWebHook"
-Added "WebHookURL"

(1.0.1)
-Added {IP} To
"ConnectPlayers"
"DisconnectPlayers"
"ConnectPlayersLog"
"DisconnectPlayersLog"

-Fixed if "ConnectPlayers" / "DisconnectPlayers" / "ConnectPlayersLog" / "DisconnectPlayersLog"
empty then disable it

(1.0.0)
-Initial Release
```

## .:[ Donation ]:.

If this project help you reduce time to develop, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/oQYh)
