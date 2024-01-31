# [CS2] Connect-Disconnect-Sound (1.0.8)

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
  //{TIME} == Time Formate "LogInsideFileTimeFormat"
  //{DATE} == Date Formate "LogFileDateFormat"
  //{PLAYERNAME} == Player Who Joined
  //{LONGCOUNTRY} == ex: United Arab Emirates
  //{SHORTCOUNTRY} == ex: AE
  //{CITY} == ex: Abu Dhabi
  //{STEAMID} = STEAM_0:1:122910632
  //{STEAMID3} = U:1:245821265
  //{STEAMID32} = 245821265
  //{STEAMID64} = 76561198206086993
  //{IP} = 127.0.0.0
  //{REASON} = Disconnect Reason
  //Colors Available = {default} {white} {darkred} {green} {lightyellow} {lightblue} {olive} {lime} {red} {lightpurple}
                      //{purple} {grey} {yellow} {gold} {silver} {blue} {darkblue} {bluegrey} {magenta} {lightred} {orange}
  
  //-----------------------------------------------------------------------------------------
  
  //Add Many Commands You Like And To Disable Any Make It Empty Like This ""
  "InGameSoundDisableCommands": "!stopsound,!stopsounds",
  //Delete Inactive Players Older Than X Days (Save Cookies in ../addons/counterstrikesharp/plugins/CnD_Sound/Cookies/)
  "RemovePlayerCookieOlderThanXDays": 7,
  // you can test any sound path ingame console type "play <soundpath>"
  // Sound Path will in https://github.com/oqyh/cs2-Connect-Disconnect-Sound/blob/main/sounds/sounds.txt
  //To Disable Any Make It Empty Like This ""
  "InGameSoundConnect": "sounds/buttons/blip1.vsnd_c",
  "InGameSoundDisconnect": "sounds/player/taunt_clap_01.vsnd_c",
  
  //-----------------------------------------------------------------------------------------
  
  // If Its Enabled Logs Will Located in ../addons/counterstrikesharp/plugins/CnD_Sound/logs/
  "SendLogToText": false,
  // Log File Format .txt or .pdf ect...
  "LogFileFormat": ".txt",
  // Date and Time Formate
  "LogFileDateFormat": "MM-dd-yyyy",
  "LogInsideFileTimeFormat": "HH:mm:ss",
  //To Disable Any Make It Empty Like This ""
  "LogTextFormatConnect": "[{DATE} - {TIME}] {PLAYERNAME} Connected [{SHORTCOUNTRY} - {CITY}] [{STEAMID} - {IP}]",
  "LogTextFormatDisconnect": "[{DATE} - {TIME}] {PLAYERNAME} Disconnected [{SHORTCOUNTRY} - {CITY}] [{STEAMID64}] [{STEAMID} - {IP}] [{REASON}]",
  //Auto Delete Logs If More Than X (Days) Old
  "AutoDeleteLogsMoreThanXdaysOld": 0,
  
  //-----------------------------------------------------------------------------------------
  
  //Send Log To Discord Via WebHookURL
  //SendLogToWebHook (0) = Disable
  //SendLogToWebHook (1) = Text Only
  //SendLogToWebHook (2) = Text With + Name + Hyperlink To Steam Profile
  //SendLogToWebHook (3) = Text With + Name + Hyperlink To Steam Profile + Profile Picture
  "SendLogToWebHook": 0,
  //If SendLogToWebHook (2) or SendLogToWebHook (3) How Would You Side Color Message To Be Check (https://www.color-hex.com/) For Colors
  "SideColorMessage": "00FFFF",
  //Discord WebHook
  "WebHookURL": "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  //To Disable Any Make It Empty Like This ""
  "LogDiscordChatFormatConnect": "{PLAYERNAME} Connected [{SHORTCOUNTRY} - {CITY}]",
  "LogDiscordChatFormatDisconnect": "{PLAYERNAME} Disconnected [{SHORTCOUNTRY} - {CITY}] [{REASON}]",
  
  //-----------------------------------------------------------------------------------------
  
  "SendLogToServerConsole": false,
  //To Disable Any Make It Empty Like This ""
  "LogServerConsoleFormatConnect": "Gold KingZ | {PLAYERNAME} Connected [{SHORTCOUNTRY} - {CITY}]",
  "LogServerConsoleFormatDisconnect": "Gold KingZ | {PLAYERNAME} Disconnected [{SHORTCOUNTRY} - {CITY}] [{REASON}]",
  
  //-----------------------------------------------------------------------------------------
  "ConfigVersion": 1
}
```

![298700262-4035e186-58f5-43ed-a50a-be189a21daaa](https://github.com/oqyh/cs2-Connect-Disconnect-Sound/assets/48490385/d8123288-e157-4fb2-87b3-8a10e4cf6f6e)


## .:[ Language ]:.
```json
{
	//==========================
	//        Colors
	//==========================
	//{Yellow} {Gold} {Silver} {Blue} {DarkBlue} {BlueGrey} {Magenta} {LightRed}
	//{LightBlue} {Olive} {Lime} {Red} {Purple} {Grey}
	//{Default} {White} {Darkred} {Green} {LightYellow}
	//==========================
	// Message_Connect/Disconnect
	//==========================
	//Date = {0}
	//Time = {1}
	//PlayerName = {2}
	//SteamID [ex: STEAM_0:1:122910632] = {3}
	//SteamID3 [ex: U:1:245821265] = {4}
	//SteamID32 [ex: 245821265] = {5}
	//SteamID64 [ex: 76561198206086993] = {6}
	//IpAddress = {7}
	//LongCountry [ex: United Arab Emirates] = {8}
	//ShortCountry [ex: AE] = {9}
	//City [ex: Abu Dhabi] = {10}
	//Disconnect Reason = {11}
	//==========================
	"InGame_Message_Connect": "{green}Gold KingZ {grey}| {purple}{2} {lime}Connected [{9} - {10}]",
	"InGame_Message_Disconnect": "{green}Gold KingZ {grey}| {purple}{2} {red}Disconnected [{11}]",

	"InGame_Command_Enabled": "{green}Gold KingZ {grey}| Connect/Disconnect Sounds Has Been {lime}Enabled",
	"InGame_Command_Disabled": "{green}Gold KingZ {grey}| Connect/Disconnect Sounds Has Been {darkred}Disabled"
}
```

## .:[ Change Log ]:.
```
(1.0.8)
-Fix Some Bugs
-Fix InGameSoundDisableCommands
-Added "RemovePlayerCookieOlderThanXDays" (Save Cookies in ../addons/counterstrikesharp/plugins/CnD_Sound/Cookies/)

(1.0.7)
-Fix Some Bugs
-Fix AutoDeleteLogsMoreThanXdaysOld
-Added "InGameMessageFormatConnect" To lang "InGame_Message_Connect" 
-Added "InGameMessageFormatDisconnect" To lang "InGame_Message_Disconnect" 
-Added Disconnect Reason {REASON}
-Added "InGameSoundDisableCommands"  Disable Enable Sound

(1.0.6)
-Fix Some Bugs

(1.0.5)
-Added "AutoDeleteLogsMoreThanXdaysOld"
-Added "SendLogToWebHook" Mode 1/2/3
-Added "SideColorMessage"
-Fix Some Bugs
-Fix Connect / Disconnect Lag On Discord WebHook

(1.0.4)
-Added "SendLogToServerConsole"
-Added "LogDiscordChatFormatDisconnect" 
-Added "LogServerConsoleFormatDisconnect"
-Added "ConnectSoundPath" and "DisconnectSound" good sounds and not annoying
-Fix Some Bugs
-Fix {TIME} and {DATE} Swapped
-Fix Discord message now better style with link to steam

(1.0.3)
-Added "LogDiscordChatFormatConnect"
-Added "LogDiscordChatFormatDisconnect" 

-Added {TIME} {DATE} {STEAMID3} {STEAMID32} To
"LogDiscordChatFormatConnect"
"LogDiscordChatFormatDisconnect" 
"ConnectPlayers"
"DisconnectPlayers"
"ConnectPlayersLog"
"DisconnectPlayersLog"

-Fix CnDModeLogs Error If has no Permissions To Write [CnD_Sound.dll]
-Fix Discord Message Lag Spike Game Server Task.WaitAll To Task.Run

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
