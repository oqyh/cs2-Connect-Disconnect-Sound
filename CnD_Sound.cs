using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Core.Attributes;
using CnD_Sound.Config;
using CounterStrikeSharp.API.Modules.Entities;

namespace CnD_Sound;

[MinimumApiVersion(164)]
public class CnDSound : BasePlugin
{
    public override string ModuleName => "Connect Disconnect Sound (Connect , Disconnect , Country , City , Message , Sound , Logs , Discord)";
    public override string ModuleVersion => "1.0.9";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";
    internal static IStringLocalizer? Stringlocalizer;
    
    public ENetworkDisconnectionReason Reason { get; set; }
    
    
    public override void Load(bool hotReload)
    {
        string ModulePath = ModuleDirectory;
        Configs.Shared.CookiesFolderPath = ModulePath;
        Configs.Load(ModulePath);

        Stringlocalizer = Localizer;
        RegisterListener<Listeners.OnClientPutInServer>(OnClientPutInServer);
        RegisterListener<Listeners.OnMapStart>(OnMapStart);
        RegisterEventHandler<EventPlayerConnectFull>(OnEventPlayerConnectFull);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnectPre, HookMode.Pre);
        RegisterEventHandler<EventPlayerChat>(OnEventPlayerChat, HookMode.Post);
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);
    }
    private void OnMapStart(string Map)
    {
        if(Configs.GetConfigData().Log_AutoDeleteLogsMoreThanXdaysOld > 0)
        {
            string Fpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs");
            Helper.DeleteOldFiles(Fpath, "*" + ".txt", TimeSpan.FromDays(Configs.GetConfigData().Log_AutoDeleteLogsMoreThanXdaysOld));
        }
    }
    public HookResult OnEventPlayerChat(EventPlayerChat @event, GameEventInfo info)
    {
        if(string.IsNullOrEmpty(Configs.GetConfigData().InGameSoundDisableCommands) || @event == null)return HookResult.Continue;
        var eventplayer = @event.Userid;
        var eventmessage = @event.Text;
        var player = Utilities.GetPlayerFromUserid(eventplayer);

        if (player == null || !player.IsValid)return HookResult.Continue;
        var playerid = player.SteamID;

        if (string.IsNullOrWhiteSpace(eventmessage)) return HookResult.Continue;
        string trimmedMessageStart = eventmessage.TrimStart();
        string message = trimmedMessageStart.TrimEnd();
        string[] InGameSoundDisableCommand = Configs.GetConfigData().InGameSoundDisableCommands.Split(',');

        if (InGameSoundDisableCommand.Any(cmd => cmd.Equals(message, StringComparison.OrdinalIgnoreCase)))
        {
            if (!string.IsNullOrEmpty(Configs.GetConfigData().InGameAllowDisableCommandsOnlyForGroups) && !Globals.AllowedGroups.ContainsKey(playerid))
            {
                if (!string.IsNullOrEmpty(Localizer["command.not.allowed"]))
                {
                    Helper.AdvancedPrintToChat(player, Localizer["command.not.allowed"]);
                }
                return HookResult.Continue;
            }

            DateTime personDate = DateTime.Now;
            bool playerValue = CnD_SoundJson.RetrieveBoolValueById((int)playerid);
            playerValue = !playerValue;

            if (playerValue)
            {
                if (!string.IsNullOrEmpty(Localizer["command.sound.disabled"]))
                {
                    Helper.AdvancedPrintToChat(player, Localizer["command.sound.disabled"]);
                }
            }else
            {
                if (!string.IsNullOrEmpty(Localizer["command.sound.enabled"]))
                {
                    Helper.AdvancedPrintToChat(player, Localizer["command.sound.enabled"]);
                }
            }

            CnD_SoundJson.SaveToJsonFile((int)playerid, playerValue, personDate);
        }
        return HookResult.Continue;
    }
    public HookResult OnEventPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
    {
        if (@event == null)return HookResult.Continue;
        var player = @event.Userid;

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV) return HookResult.Continue;
        var playerid = player.SteamID;

        if(!string.IsNullOrEmpty(Configs.GetConfigData().InGameAllowDisableCommandsOnlyForGroups) && Helper.IsPlayerInGroupPermission(player, Configs.GetConfigData().InGameAllowDisableCommandsOnlyForGroups))
        {
            if (!Globals.AllowedGroups.ContainsKey(playerid))
            {
                Globals.AllowedGroups.Add(playerid, true);
            }
        }

        if(Configs.GetConfigData().DisableLoopConnections)
        {
            if (Globals.OnLoop.ContainsKey(playerid))
            {
                Globals.OnLoop.Remove(playerid);
            }
        }

        return HookResult.Continue;
    }
    private void OnClientPutInServer(int playerSlot)
    {
        string Fpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/");
        string Time = DateTime.Now.ToString("HH:mm:ss");
        string Date = DateTime.Now.ToString("MM-dd-yyyy");
        string fileName = DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
        string Tpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/") + $"{fileName}";

        var player = Utilities.GetPlayerFromSlot(playerSlot);
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV)return;

        var JoinPlayer = player.PlayerName;
        var steamId2 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId2 : Localizer["invalid.steamid"];
        var steamId3 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId3 : Localizer["invalid.steamid"];
        var steamId32 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId32.ToString() : Localizer["invalid.steamid"];
        var steamId64 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId64.ToString() : Localizer["invalid.steamid"];
        var GetIpAddress = NativeAPI.GetPlayerIpAddress(playerSlot);
        var ipAddress = GetIpAddress?.Split(':')[0] ?? Localizer["invalid.ipadress"];
        var Country = GetCountry(ipAddress);
        var SCountry = GetCountryS(ipAddress);
        var City = GetCity(ipAddress);
        
        if (!string.IsNullOrEmpty(Localizer["chat.message.connect"]))
        {
            Helper.AdvancedPrintToServer(Localizer["chat.message.connect"],Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, string.Empty);
        }
        
        if (!string.IsNullOrEmpty(Localizer["console.message.connect"]))
        {
            Helper.AdvancedPrintToConsoleServer(Localizer["console.message.connect"],Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, string.Empty);
        }
        
        if(Configs.GetConfigData().SendLogToText)
        {
            if(!Directory.Exists(Fpath))
            {
                Directory.CreateDirectory(Fpath);
            }

            if(!File.Exists(Tpath))
            {
                using (File.Create(Tpath)) { }
            }
            if (!string.IsNullOrEmpty(Configs.GetConfigData().Log_TextConnectMessageFormat) && File.Exists(Tpath))
            {
                var replacerlog = Helper.ReplaceMessages(Configs.GetConfigData().Log_TextConnectMessageFormat, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, string.Empty);
                try
                {
                    File.AppendAllLines(Tpath, new[]{replacerlog});
                }catch
                {

                }
            }
        }

        if (!string.IsNullOrEmpty(Configs.GetConfigData().Log_DiscordConnectMessageFormat))
        {
            var replacerlogd = Helper.ReplaceMessages(Configs.GetConfigData().Log_DiscordConnectMessageFormat, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, string.Empty);
            if(Configs.GetConfigData().Log_SendLogToDiscordOnMode == 1)
            {
                Task.Run(() =>
                {
                    _ = CnD_SoundJson.SendToDiscordWebhookNormal(Configs.GetConfigData().Log_DiscordWebHookURL, replacerlogd);
                });
                
            }else if(Configs.GetConfigData().Log_SendLogToDiscordOnMode == 2)
            {
                Task.Run(() =>
                {
                    _ = CnD_SoundJson.SendToDiscordWebhookNameLink(Configs.GetConfigData().Log_DiscordWebHookURL, replacerlogd, steamId64.ToString(), JoinPlayer);
                });
            }else if(Configs.GetConfigData().Log_SendLogToDiscordOnMode == 3)
            {
                Task.Run(() =>
                {
                    _ = CnD_SoundJson.SendToDiscordWebhookNameLinkWithPicture(Configs.GetConfigData().Log_DiscordWebHookURL, replacerlogd, steamId64.ToString(), JoinPlayer);
                });
            }
        }
        

        if (!string.IsNullOrEmpty(Configs.GetConfigData().InGameSoundConnect))
        {
            var AllPlayers = Helper.GetAllController();
            foreach(var players in AllPlayers)
            {
                var playerid = players.SteamID;
                bool playerValue = CnD_SoundJson.RetrieveBoolValueById((int)playerid);
                if (!string.IsNullOrEmpty(Configs.GetConfigData().InGameSoundDisableCommands))
                {
                    if (playerValue)
                    {
                        //skip sounds
                    }else
                    {
                        players.ExecuteClientCommand("play " + Configs.GetConfigData().InGameSoundConnect); 
                    }
                }else
                {
                    players.ExecuteClientCommand("play " + Configs.GetConfigData().InGameSoundConnect);
                }
            }
        }
    }
    private HookResult OnPlayerDisconnectPre(EventPlayerDisconnect @event, GameEventInfo info)
    {
        if (!Configs.GetConfigData().RemoveDefaultDisconnect || @event == null)return HookResult.Continue;

        info.DontBroadcast = true;

        return HookResult.Continue;
    }
    private HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        string Fpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/");
        string Time = DateTime.Now.ToString("HH:mm:ss");
        string Date = DateTime.Now.ToString("MM-dd-yyyy");
        string fileName = DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
        string Tpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/") + $"{fileName}";

        var player = @event.Userid;
        var reasonInt = @event.Reason;

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV)return HookResult.Continue;
        var playerSlot = player.Slot;
        var LeftPlayer = player.PlayerName;
        var steamId2 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId2 : Localizer["invalid.steamid"];
        var steamId3 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId3 : Localizer["invalid.steamid"];
        var steamId32 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId32.ToString() : Localizer["invalid.steamid"];
        var steamId64 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId64.ToString() : Localizer["invalid.steamid"];
        var GetIpAddress = NativeAPI.GetPlayerIpAddress(playerSlot);
        var ipAddress = GetIpAddress?.Split(':')[0] ?? Localizer["invalid.ipadress"];
        var Country = GetCountry(ipAddress);
        var SCountry = GetCountryS(ipAddress);
        var City = GetCity(ipAddress);

        if(Configs.GetConfigData().DisableLoopConnections)
        {
            if(reasonInt == 54 || reasonInt == 55 || reasonInt == 57)
            {
                if (!Globals.OnLoop.ContainsKey(player.SteamID))
                {
                    Globals.OnLoop.Add(player.SteamID, true);
                }
                if (Globals.OnLoop.ContainsKey(player.SteamID))
                {
                    return HookResult.Continue;
                }
            }
        }

        if (!string.IsNullOrEmpty(Localizer["chat.message.disconnect"]))
        {
            if (Enum.IsDefined(typeof(ENetworkDisconnectionReason), reasonInt))
            {
                string disconnectReasonString = NetworkDisconnectionReasonHelper.GetDisconnectReasonString((ENetworkDisconnectionReason)reasonInt);
                Helper.AdvancedPrintToServer(Localizer["chat.message.disconnect"],Date, Time, LeftPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, disconnectReasonString);
            }
        }
        
        if (!string.IsNullOrEmpty(Localizer["console.message.disconnect"]))
        {
            if (Enum.IsDefined(typeof(ENetworkDisconnectionReason), reasonInt))
            {
                string disconnectReasonString = NetworkDisconnectionReasonHelper.GetDisconnectReasonString((ENetworkDisconnectionReason)reasonInt);
                Helper.AdvancedPrintToConsoleServer(Localizer["console.message.disconnect"],Date, Time, LeftPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, disconnectReasonString);
            }
        }
        
        if(Configs.GetConfigData().SendLogToText)
        {
            if(!Directory.Exists(Fpath))
            {
                Directory.CreateDirectory(Fpath);
            }

            if(!File.Exists(Tpath))
            {
                using (File.Create(Tpath)) { }
            }

            if (!string.IsNullOrEmpty(Configs.GetConfigData().Log_TextDisconnectMessageFormat) && File.Exists(Tpath))
            {
                if (Enum.IsDefined(typeof(ENetworkDisconnectionReason), reasonInt))
                {
                    string disconnectReasonString = NetworkDisconnectionReasonHelper.GetDisconnectReasonString((ENetworkDisconnectionReason)reasonInt);
                    var replacerlog = Helper.ReplaceMessages(Configs.GetConfigData().Log_TextDisconnectMessageFormat, Date, Time, LeftPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, disconnectReasonString);
                    try
                    {
                        File.AppendAllLines(Tpath, new[]{replacerlog});
                    }catch
                    {

                    }
                }
            }
        }

        
        if (!string.IsNullOrEmpty(Configs.GetConfigData().Log_DiscordDisconnectMessageFormat))
        {
            if (Enum.IsDefined(typeof(ENetworkDisconnectionReason), reasonInt))
            {
                string disconnectReasonString = NetworkDisconnectionReasonHelper.GetDisconnectReasonString((ENetworkDisconnectionReason)reasonInt);
                var replacerlogd = Helper.ReplaceMessages(Configs.GetConfigData().Log_DiscordDisconnectMessageFormat, Date, Time, LeftPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, disconnectReasonString);
                if(Configs.GetConfigData().Log_SendLogToDiscordOnMode == 1)
                {
                    Task.Run(() =>
                    {
                        _ = CnD_SoundJson.SendToDiscordWebhookNormal(Configs.GetConfigData().Log_DiscordWebHookURL, replacerlogd);
                    });
                    
                }else if(Configs.GetConfigData().Log_SendLogToDiscordOnMode == 2)
                {
                    Task.Run(() =>
                    {
                        _ = CnD_SoundJson.SendToDiscordWebhookNameLink(Configs.GetConfigData().Log_DiscordWebHookURL, replacerlogd, steamId64.ToString(), LeftPlayer);
                    });
                }else if(Configs.GetConfigData().Log_SendLogToDiscordOnMode == 3)
                {
                    Task.Run(() =>
                    {
                        _ = CnD_SoundJson.SendToDiscordWebhookNameLinkWithPicture(Configs.GetConfigData().Log_DiscordWebHookURL, replacerlogd, steamId64.ToString(), LeftPlayer);
                    });
                }
            }
        }
        
        if (!string.IsNullOrEmpty(Configs.GetConfigData().InGameSoundDisconnect))
        {
            var AllPlayers = Helper.GetAllController();
            foreach(var players in AllPlayers)
            {
                var playerid = players.SteamID;
                bool playerValue = CnD_SoundJson.RetrieveBoolValueById((int)playerid);
                if (!string.IsNullOrEmpty(Configs.GetConfigData().InGameSoundDisableCommands))
                {
                    if (playerValue)
                    {
                        //skip sounds
                    }else
                    {
                        players.ExecuteClientCommand("play " + Configs.GetConfigData().InGameSoundDisconnect); 
                    }
                }else
                {
                    players.ExecuteClientCommand("play " + Configs.GetConfigData().InGameSoundDisconnect);
                }
            }
        }
        Globals.AllowedGroups.Remove(player.SteamID);
        return HookResult.Continue;
    }
    public string GetCountryS(string ipAddress)
    {
        string cookiesFilePath = Configs.Shared.CookiesFolderPath!;
        try
        {
            using (var reader = new DatabaseReader(Path.Combine(cookiesFilePath, "../../plugins/CnD_Sound/GeoLocation/GeoLite2-City.mmdb")))
            {
                var response = reader.City(ipAddress);
                return response.Country.IsoCode ?? Localizer["unknown.short.country"];
            }
        }
        catch (AddressNotFoundException)
        {
            return Localizer["unknown.short.country"];
        }
        catch
        {
            return Localizer["unknown.short.country"];
        }
    }

    public string GetCountry(string ipAddress)
    {
        string cookiesFilePath = Configs.Shared.CookiesFolderPath!;
        try
        {
            using (var reader = new DatabaseReader(Path.Combine(cookiesFilePath, "../../plugins/CnD_Sound/GeoLocation/GeoLite2-City.mmdb")))
            {
                var response = reader.City(ipAddress);
                return response.Country.Name ?? Localizer["unknown.long.country"];
            }
        }
        catch (AddressNotFoundException)
        {
            return Localizer["unknown.long.country"];
        }
        catch
        {
            return Localizer["unknown.long.country"];
        }
    }

    public string GetCity(string ipAddress)
    {
        string cookiesFilePath = Configs.Shared.CookiesFolderPath!;
        try
        {
            using (var reader = new DatabaseReader(Path.Combine(cookiesFilePath, "../../plugins/CnD_Sound/GeoLocation/GeoLite2-City.mmdb")))
            {
                var response = reader.City(ipAddress);
                return response.City.Name ?? Localizer["unknown.city"];
            }
        }
        catch (AddressNotFoundException)
        {
            return Localizer["unknown.city"];
        }
        catch 
        {
            return Localizer["unknown.city"];
        }
    }
    public void OnMapEnd()
    {
        Helper.ClearVariables();
    }
    public override void Unload(bool hotReload)
    {
        Helper.ClearVariables();
    }
}