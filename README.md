## .:[ Join Our Discord For Support ]:.
<a href="https://discord.com/invite/U7AuQhu"><img src="https://discord.com/api/guilds/651838917687115806/widget.png?style=banner2"></a>

***
# [CS2] Connect-Disconnect-Sound (1.0.9)

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

> [!CAUTION]
> Config Located In ..\addons\counterstrikesharp\plugins\CnD_Sound\config\config.json                                           
>

```json
{
  //Disable Looping Connections To Anti Spam Chat
  "DisableLoopConnections": true,
  
  //Remove Default Disconnect Message 
  "RemoveDefaultDisconnect": true,
  
  //Sound Path Of Connect Players To Disable Make it ""
  "InGameSoundConnect": "sounds/buttons/blip1.vsnd_c",
  //Sound Path Of Disconnect Players To Disable Make it ""
  "InGameSoundDisconnect": "sounds/player/taunt_clap_01.vsnd_c",
  //Allow These Group Only To Toggle On/Off Sounds "" Means Anyone
  "InGameAllowDisableCommandsOnlyForGroups": "",
  //Command Toggle On/Off To Disable This Make it ""
  "InGameSoundDisableCommands": "!stopsound,!stopsounds",
  
  //Delete Inactive Players Older Than X Days (Save Cookies in ../addons/counterstrikesharp/plugins/CnD_Sound/Cookies/)
  "RemovePlayerCookieOlderThanXDays": 7,
  
  
  //==========================
  // Message Connect/Disconnect
  //==========================
  //  {DATE} = Date
  //  {TIME} = Time
  //  {PLAYERNAME} = PlayerName
  //  {STEAMID} = SteamID [ex: STEAM_0:1:122910632]
  //  {STEAMID3} = SteamID3 [ex: U:1:245821265]
  //  {STEAMID32} = SteamID32 [ex: 245821265]
  //  {STEAMID64} = SteamID64 [ex: 76561198206086993]
  //  {IP} = IpAddress
  //  {LONGCOUNTRY} = LongCountry [ex: United Arab Emirates]
  //  {SHORTCOUNTRY} = ShortCountry [ex: AE]
  //  {CITY} = City [ex: Abu Dhabi]
  //  {REASON} = Disconnect Reason 
  //==========================
  
  // If Its Enabled Logs Will Located in ../addons/counterstrikesharp/plugins/CnD_Sound/logs/
  "SendLogToText": false,
  
  //How Message Look Like To Disable Make it ""
  "Log_TextConnectMessageFormat": "[{DATE} - {TIME}] {PLAYERNAME} Connected [{SHORTCOUNTRY} - {CITY}] [{STEAMID} - {IP}]",
  "Log_TextDisconnectMessageFormat": "[{DATE} - {TIME}] {PLAYERNAME} Disconnected [{SHORTCOUNTRY} - {CITY}] [{STEAMID64}] [{STEAMID} - {IP}] [{REASON}]",
  
  //Auto Delete Logs If More Than X (Days) Old
  "Log_AutoDeleteLogsMoreThanXdaysOld": 7,
  
  //Send Log To Discord Via WebHookURL
  //SendLogToWebHook (0) = Disable
  //SendLogToWebHook (1) = Text Only
  //SendLogToWebHook (2) = Text With + Name + Hyperlink To Steam Profile
  //SendLogToWebHook (3) = Text With + Name + Hyperlink To Steam Profile + Profile Picture
  "Log_SendLogToDiscordOnMode": 0,
  
  //If SendLogToWebHook (2) or SendLogToWebHook (3) How Would You Side Color Message To Be Check (https://www.color-hex.com/) For Colors
  "Log_DiscordSideColor": "00FFFF",
  
  //Discord WebHook
  "Log_DiscordWebHookURL": "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  
  //How Message Look Like To Disable Make it ""
  "Log_DiscordConnectMessageFormat": "{PLAYERNAME} Connected [{LONGCOUNTRY} - {CITY}]",
  "Log_DiscordDisconnectMessageFormat": "{PLAYERNAME} Disconnected [{LONGCOUNTRY} - {CITY}] [{REASON}]",
  
  //If Log_SendLogToDiscordOnMode (3) And Player Doesn't Have Profile Picture Which Picture Do You Like To Be Replaced
  "Log_DiscordUsersWithNoAvatarImage": "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/b5/b5bd56c1aa4644a474a2e4972be27ef9e82e517e_full.jpg",
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
	//        Other
	//==========================
	//{nextline} = Print On Next Line
	//==========================
	// Message Connect/Disconnect
	//==========================
	//  {0} = Date
	//  {1} = Time
	//  {2} = PlayerName
	//  {3} = SteamID [ex: STEAM_0:1:122910632]
	//  {4} = SteamID3 [ex: U:1:245821265]
	//  {5} = SteamID32 [ex: 245821265]
	//  {6} = SteamID64 [ex: 76561198206086993]
	//  {7} = IpAddress
	//  {8} = LongCountry [ex: United Arab Emirates]
	//  {9} = ShortCountry [ex: AE]
	//  {10} = City [ex: Abu Dhabi]
	//  {11} = Disconnect Reason 
	//==========================
	
	"chat.message.connect": "{green}Gold KingZ {grey}| {purple}{2} {lime}Connected [{9} - {10}]",
	"chat.message.disconnect": "{green}Gold KingZ {grey}| {purple}{2} {red}Disconnected [{11}]",

	"console.message.connect": "Gold KingZ | {2} Connected [{9} - {10}]",
	"console.message.disconnect": "Gold KingZ | {2} Disconnected [{9} - {10}] [{11}]",

	"command.not.allowed": "{green}Gold KingZ {grey}| {darkred}Toggle Connect/Disconnect Sounds For Vips",
	"command.sound.enabled": "{green}Gold KingZ {grey}| Connect/Disconnect Sounds Has Been {lime}Enabled",
	"command.sound.disabled": "{green}Gold KingZ {grey}| Connect/Disconnect Sounds Has Been {darkred}Disabled",

	"invalid.steamid": "InvalidSteamID",
	"invalid.ipadress": "InValidIpAddress",
	"unknown.short.country": "U/C",
	"unknown.long.country": "Unknown Country",
	"unknown.city": "Unknown City"
}
```

## .:[ Change Log ]:.
```
(1.0.9)
-Fix Some Bugs
-Added DisableLoopConnections
-Added RemoveDefaultDisconnect
-Added InGameAllowDisableCommandsOnlyForGroups
-Added Log_DiscordUsersWithNoAvatarImage
-Added Lang console.message.connect
-Added Lang console.message.disconnect
-Added Lang command.not.allowed
-Added Lang invalid.steamid
-Added Lang invalid.ipadress
-Added Lang unknown.short.country
-Added Lang unknown.long.country
-Added Lang unknown.city

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
