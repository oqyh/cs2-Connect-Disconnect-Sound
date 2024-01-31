using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using System.Text;
using System.Drawing;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Modules.Commands;
using Newtonsoft.Json;

namespace CnD_Sound;

public class CnDSoundConfig : BasePluginConfig
{
    [JsonPropertyName("InGameSoundDisableCommands")] public string InGameSoundDisableCommands { get; set; } = "!stopsound,!stopsounds";
    [JsonPropertyName("RemovePlayerCookieOlderThanXDays")] public int RemovePlayerCookieOlderThanXDays { get; set; } = 7;
    [JsonPropertyName("InGameSoundConnect")] public string InGameSoundConnect { get; set; } = "sounds/buttons/blip1.vsnd_c";
    [JsonPropertyName("InGameSoundDisconnect")] public string InGameSoundDisconnect { get; set; } = "sounds/player/taunt_clap_01.vsnd_c";


    [JsonPropertyName("SendLogToText")] public bool SendLogToText { get; set; } = false;
    [JsonPropertyName("LogFileFormat")] public string LogFileFormat { get; set; } = ".txt";
    [JsonPropertyName("LogFileDateFormat")] public string LogFileDateFormat { get; set; } = "MM-dd-yyyy";
    [JsonPropertyName("LogInsideFileTimeFormat")] public string LogInsideFileTimeFormat { get; set; } = "HH:mm:ss";
    [JsonPropertyName("LogTextFormatConnect")] public string LogTextFormatConnect { get; set; } = "[{DATE} - {TIME}] {PLAYERNAME} Connected [{SHORTCOUNTRY} - {CITY}] [{STEAMID} - {IP}]";
    [JsonPropertyName("LogTextFormatDisconnect")] public string LogTextFormatDisconnect { get; set; } = "[{DATE} - {TIME}] {PLAYERNAME} Disconnected [{SHORTCOUNTRY} - {CITY}] [{STEAMID64}] [{STEAMID} - {IP}] [{REASON}]";
    [JsonPropertyName("AutoDeleteLogsMoreThanXdaysOld")] public int AutoDeleteLogsMoreThanXdaysOld { get; set; } = 0;



    [JsonPropertyName("SendLogToWebHook")] public int SendLogToWebHook { get; set; } = 0;
    [JsonPropertyName("SideColorMessage")] public string SideColorMessage { get; set; } = "00FFFF";
    [JsonPropertyName("WebHookURL")] public string WebHookURL { get; set; } = "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
    [JsonPropertyName("LogDiscordChatFormatConnect")] public string LogDiscordChatFormatConnect { get; set; } = "{PLAYERNAME} Connected [{LONGCOUNTRY} - {CITY}]";
    [JsonPropertyName("LogDiscordChatFormatDisconnect")] public string LogDiscordChatFormatDisconnect { get; set; } = "{PLAYERNAME} Disconnected [{LONGCOUNTRY} - {CITY}] [{REASON}]";



	[JsonPropertyName("SendLogToServerConsole")] public bool SendLogToServerConsole { get; set; } = false;
    [JsonPropertyName("LogServerConsoleFormatConnect")] public string LogServerConsoleFormatConnect { get; set; } = "Gold KingZ | {PLAYERNAME} Connected [{SHORTCOUNTRY} - {CITY}]";
    [JsonPropertyName("LogServerConsoleFormatDisconnect")] public string LogServerConsoleFormatDisconnect { get; set; } = "Gold KingZ | {PLAYERNAME} Disconnected [{SHORTCOUNTRY} - {CITY}] [{REASON}]";
}

public class CnDSound : BasePlugin, IPluginConfig<CnDSoundConfig>
{
    public override string ModuleName => "Connect Disconnect Sound";
    public override string ModuleVersion => "1.0.8";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "Connect , Disconnect , Country , City , Message , Sound , Logs , Discord";
    internal static IStringLocalizer? Stringlocalizer;
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly HttpClient httpClient = new HttpClient();
    public CnDSoundConfig Config { get; set; } = new CnDSoundConfig();
    public ENetworkDisconnectionReason Reason { get; set; }
    private Dictionary<int, bool> OnDisabled = new Dictionary<int, bool>();
    public void OnConfigParsed(CnDSoundConfig config)
    {
        Config = config;
        Stringlocalizer = Localizer;
        if (Config.SendLogToWebHook < 0 || Config.SendLogToWebHook > 3)
        {
            config.SendLogToWebHook = 0;
            Console.WriteLine("|||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("SendLogToWebHook: is invalid, setting to default value (0) Please Choose 0 or 1 or 2 or 3.");
            Console.WriteLine("SendLogToWebHook (0) = Disableis invalid");
            Console.WriteLine("SendLogToWebHook (1) = Text Only");
            Console.WriteLine("SendLogToWebHook (2) = Text With + Name + Hyperlink To Steam Profile");
            Console.WriteLine("SendLogToWebHook (3) = Text With + Name + Hyperlink To Steam Profile + Profile Picture");
            Console.WriteLine("|||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||");
        }

        if (Config.SideColorMessage.StartsWith("#"))
        {
            Config.SideColorMessage = Config.SideColorMessage.Substring(1);
        }
    }
    
    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnClientPutInServer>(OnClientPutInServer);
        RegisterListener<Listeners.OnMapStart>(OnMapStart);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect);
        AddCommandListener("say", OnPlayerSayPublic, HookMode.Post);
        AddCommandListener("say_team", OnPlayerSayTeam, HookMode.Post);
    }
    private HookResult OnPlayerSayPublic(CCSPlayerController? player, CommandInfo info)
	{
        if (string.IsNullOrEmpty(Config.InGameSoundDisableCommands) || player == null || !player.IsValid || player.IsBot || player.IsHLTV)return HookResult.Continue;
        var playerid = player.SteamID;
        bool playerValue = RetrieveBoolValueById((int)playerid);
        var message = info.GetArg(1);
        if (string.IsNullOrWhiteSpace(message)) return HookResult.Continue;
        string trimmedMessage1 = message.TrimStart();
        string trimmedMessage = trimmedMessage1.TrimEnd();
        
        string[] disableCommands = Config.InGameSoundDisableCommands.Split(',');
        
        if (disableCommands.Any(cmd => cmd.Equals(trimmedMessage, StringComparison.OrdinalIgnoreCase)))
        {
            DateTime personDate = DateTime.Now;
            

            playerValue = !playerValue;

            if (playerValue)
            {
                player.PrintToChat(Localizer["InGame_Command_Disabled"]);
            }else
            {
                player.PrintToChat(Localizer["InGame_Command_Enabled"]);
            }

            SaveToJsonFile((int)playerid, playerValue, personDate);
        }
        return HookResult.Continue;
    }
    private HookResult OnPlayerSayTeam(CCSPlayerController? player, CommandInfo info)
	{
        if (string.IsNullOrEmpty(Config.InGameSoundDisableCommands) || player == null || !player.IsValid || player.IsBot || player.IsHLTV)return HookResult.Continue;
        var playerid = player.SteamID;
        bool playerValue = RetrieveBoolValueById((int)playerid);
        var message = info.GetArg(1);
        if (string.IsNullOrWhiteSpace(message)) return HookResult.Continue;
        string trimmedMessage1 = message.TrimStart();
        string trimmedMessage = trimmedMessage1.TrimEnd();
        
        string[] disableCommands = Config.InGameSoundDisableCommands.Split(',');
        
        if (disableCommands.Any(cmd => cmd.Equals(trimmedMessage, StringComparison.OrdinalIgnoreCase)))
        {
            DateTime personDate = DateTime.Now;
            

            playerValue = !playerValue;

            if (playerValue)
            {
                player.PrintToChat(Localizer["InGame_Command_Disabled"]);
            }else
            {
                player.PrintToChat(Localizer["InGame_Command_Enabled"]);
            }

            SaveToJsonFile((int)playerid, playerValue, personDate);
        }
        return HookResult.Continue;
    }
    private void OnMapStart(string Map)
    {
        if(Config.AutoDeleteLogsMoreThanXdaysOld > 0)
        {
            string Fpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs");
            DeleteOldFiles(Fpath, "*" + Config.LogFileFormat, TimeSpan.FromDays(Config.AutoDeleteLogsMoreThanXdaysOld));
        }
    }
    

    private void OnClientPutInServer(int playerSlot)
    {
        
        string Fpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/");
        string Time = DateTime.Now.ToString(Config.LogInsideFileTimeFormat);
        string Date = DateTime.Now.ToString(Config.LogFileDateFormat);
        string fileName = DateTime.Now.ToString(Config.LogFileDateFormat) + Config.LogFileFormat;
        string Tpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/") + $"{fileName}";

        if(Config.SendLogToText && !Directory.Exists(Fpath))
        {
            Directory.CreateDirectory(Fpath);
        }

        if(Config.SendLogToText && !File.Exists(Tpath))
        {
            File.Create(Tpath);
        }
        
        var player = Utilities.GetPlayerFromSlot(playerSlot);

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV)return;

        var JoinPlayer = player.PlayerName;
        var steamId2 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId2 : "InvalidSteamID";
        var steamId3 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId3 : "InvalidSteamID";
        var steamId32 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId32.ToString() : "InvalidSteamID";
        var steamId64 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId64.ToString() : "InvalidSteamID";
        var GetIpAddress = NativeAPI.GetPlayerIpAddress(playerSlot);
        var ipAddress = GetIpAddress?.Split(':')[0] ?? "InValidIpAddress";
        var Country = GetCountry(ipAddress);
        var SCountry = GetCountryS(ipAddress);
        var City = GetCity(ipAddress);

        if (!string.IsNullOrEmpty(Localizer["InGame_Message_Connect"]))
        {
            Server.PrintToChatAll(Localizer["InGame_Message_Connect", Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, string.Empty]);
        }

        if(Config.SendLogToServerConsole)
        {
            if (!string.IsNullOrEmpty(Config.LogServerConsoleFormatConnect))
            {
                var replacer = ReplaceMessages(Config.LogServerConsoleFormatConnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, string.Empty);
                Server.PrintToConsole(replacer);
            }
        }

        if(Config.SendLogToText)
        {
            var replacerlog = ReplaceMessages(Config.LogTextFormatConnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, string.Empty);
            if (!string.IsNullOrEmpty(Config.LogTextFormatConnect) && File.Exists(Tpath))
            {
                try
                {
                    File.AppendAllLines(Tpath, new[]{replacerlog});
                }catch
                {
                    Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
                    Console.WriteLine("[Error Cant Write] Please Give CnD_Sound.dll Permissions To Write");
                    Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
                }
            }
        }

        var replacerDISCORDlog = ReplaceMessages(Config.LogDiscordChatFormatConnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, string.Empty);
        if(Config.SendLogToWebHook == 1)
        {
            if (!string.IsNullOrEmpty(Config.LogDiscordChatFormatConnect))
            {
                _ = SendToDiscordWebhookNormal(Config.WebHookURL, replacerDISCORDlog);
            }
        }else if(Config.SendLogToWebHook == 2)
        {
            if (!string.IsNullOrEmpty(Config.LogDiscordChatFormatConnect))
            {
                _ = SendToDiscordWebhookNameLink(Config.WebHookURL, replacerDISCORDlog, steamId64.ToString(), JoinPlayer);
            }
        }else if(Config.SendLogToWebHook == 3)
        {
            if (!string.IsNullOrEmpty(Config.LogDiscordChatFormatConnect))
            {
                _ = SendToDiscordWebhookNameLinkWithPicture(Config.WebHookURL, replacerDISCORDlog, steamId64.ToString(), JoinPlayer);
            }
        }
        
        if (!string.IsNullOrEmpty(Config.InGameSoundConnect))
        {
            foreach(var players in GetPlayerControllers().FindAll(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot))
            {
                if (players.IsValid)
                {
                    var playerid = players.SteamID;
                    bool playerValue = RetrieveBoolValueById((int)playerid);
                    if (!string.IsNullOrEmpty(Config.InGameSoundDisableCommands))
                    {
                        if (playerValue)
                        {
                            //skip sounds
                        }else
                        {
                            players.ExecuteClientCommand("play " + Config.InGameSoundConnect); 
                        }
                    }else
                    {
                        players.ExecuteClientCommand("play " + Config.InGameSoundConnect);
                    }
                    
                }
            }
        }
    }
    private HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        string Fpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/");
        string Time = DateTime.Now.ToString(Config.LogInsideFileTimeFormat);
        string Date = DateTime.Now.ToString(Config.LogFileDateFormat);
        string fileName = DateTime.Now.ToString(Config.LogFileDateFormat) + Config.LogFileFormat;
        string Tpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/") + $"{fileName}";

        if(Config.SendLogToText && !Directory.Exists(Fpath))
        {
            Directory.CreateDirectory(Fpath);
        }

        if(Config.SendLogToText && !File.Exists(Tpath))
        {
            File.Create(Tpath);
        }
        
        var player = @event.Userid;
        var reasonInt = @event.Reason;

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV)return HookResult.Continue;

        var playerSlot = player.Slot;
        var JoinPlayer = player.PlayerName;
        var steamId2 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId2 : "InvalidSteamID";
        var steamId3 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId3 : "InvalidSteamID";
        var steamId32 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId32.ToString() : "InvalidSteamID";
        var steamId64 = (player.AuthorizedSteamID != null) ? player.AuthorizedSteamID.SteamId64.ToString() : "InvalidSteamID";
        var GetIpAddress = NativeAPI.GetPlayerIpAddress(playerSlot);
        var ipAddress = GetIpAddress?.Split(':')[0] ?? "InValidIpAddress";
        var Country = GetCountry(ipAddress);
        var SCountry = GetCountryS(ipAddress);
        var City = GetCity(ipAddress);

        if (!string.IsNullOrEmpty(Localizer["InGame_Message_Disconnect"]))
        {
            if (Enum.IsDefined(typeof(ENetworkDisconnectionReason), reasonInt))
            {
                string disconnectReasonString = NetworkDisconnectionReasonHelper.GetDisconnectReasonString((ENetworkDisconnectionReason)reasonInt);
                Server.PrintToChatAll(Localizer["InGame_Message_Disconnect", Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, disconnectReasonString]);
            }
        }

        if(Config.SendLogToServerConsole)
        {
            if (!string.IsNullOrEmpty(Config.LogServerConsoleFormatDisconnect))
            {
                if (Enum.IsDefined(typeof(ENetworkDisconnectionReason), reasonInt))
                {
                    string disconnectReasonString = NetworkDisconnectionReasonHelper.GetDisconnectReasonString((ENetworkDisconnectionReason)reasonInt);
                    var replacer = ReplaceMessages(Config.LogServerConsoleFormatDisconnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, disconnectReasonString);
                    Server.PrintToConsole(replacer);
                }
            }
        }

        if(Config.SendLogToText)
        {
            if (Enum.IsDefined(typeof(ENetworkDisconnectionReason), reasonInt))
            {
                string disconnectReasonString = NetworkDisconnectionReasonHelper.GetDisconnectReasonString((ENetworkDisconnectionReason)reasonInt);
                var replacerlog = ReplaceMessages(Config.LogTextFormatDisconnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, disconnectReasonString);
                if (!string.IsNullOrEmpty(Config.LogTextFormatDisconnect) && File.Exists(Tpath))
                {
                    
                    try
                    {
                        File.AppendAllLines(Tpath, new[]{replacerlog});
                    }catch
                    {
                        Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
                        Console.WriteLine("[Error Cant Write] Please Give CnD_Sound.dll Permissions To Write");
                        Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
                    }
                }
            }
        }
        
        if (Enum.IsDefined(typeof(ENetworkDisconnectionReason), reasonInt))
        {
            string disconnectReasonString = NetworkDisconnectionReasonHelper.GetDisconnectReasonString((ENetworkDisconnectionReason)reasonInt);
            var replacerDISCORDlog = ReplaceMessages(Config.LogDiscordChatFormatDisconnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City, disconnectReasonString);
            if(Config.SendLogToWebHook == 1)
            {
                if (!string.IsNullOrEmpty(Config.LogDiscordChatFormatDisconnect))
                {
                    _ = SendToDiscordWebhookNormal(Config.WebHookURL, replacerDISCORDlog);
                }
            }else if(Config.SendLogToWebHook == 2)
            {
                if (!string.IsNullOrEmpty(Config.LogDiscordChatFormatDisconnect))
                {
                    _ = SendToDiscordWebhookNameLink(Config.WebHookURL, replacerDISCORDlog, steamId64.ToString(), JoinPlayer);
                }
            }else if(Config.SendLogToWebHook == 3)
            {
                if (!string.IsNullOrEmpty(Config.LogDiscordChatFormatDisconnect))
                {
                    _ = SendToDiscordWebhookNameLinkWithPicture(Config.WebHookURL, replacerDISCORDlog, steamId64.ToString(), JoinPlayer);
                }
            }
        }

        if (!string.IsNullOrEmpty(Config.InGameSoundDisconnect))
        {
            foreach(var players in GetPlayerControllers().FindAll(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot))
            {
                if (players.IsValid)
                {
                    var playerid = players.SteamID;
                    bool playerValue = RetrieveBoolValueById((int)playerid);
                    if (!string.IsNullOrEmpty(Config.InGameSoundDisableCommands))
                    {
                        if (playerValue)
                        {
                            //skip sounds
                        }else
                        {
                            players.ExecuteClientCommand("play " + Config.InGameSoundDisconnect); 
                        }
                    }else
                    {
                        players.ExecuteClientCommand("play " + Config.InGameSoundDisconnect);
                    }
                    
                }
            }
        }
        return HookResult.Continue;
    }
    private string ReplaceMessages(string Message, string date, string time, string PlayerName, string SteamId, string SteamId3, string SteamId32, string SteamId64, string ipAddress, string Country, string SCountry, string City, string reason)
    {
        var replacedMessage = Message
                                    .Replace("{TIME}", time)
                                    .Replace("{DATE}", date)
                                    .Replace("{PLAYERNAME}", PlayerName.ToString())
                                    .Replace("{STEAMID}", SteamId.ToString())
                                    .Replace("{STEAMID3}", SteamId3.ToString())
                                    .Replace("{STEAMID32}", SteamId32.ToString())
                                    .Replace("{STEAMID64}", SteamId64.ToString())
                                    .Replace("{IP}", ipAddress.ToString())
                                    .Replace("{LONGCOUNTRY}", Country)
                                    .Replace("{SHORTCOUNTRY}", SCountry)
                                    .Replace("{CITY}", City)
                                    .Replace("{REASON}", reason);
        return replacedMessage;
    }

    private static List<CCSPlayerController> GetPlayerControllers() 
    {
        var playerList = Utilities.FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller").ToList();

        return playerList;
    }

    private string GetCountryS(string ipAddress)
    {
        try
        {
            using (var reader = new DatabaseReader(Path.Combine(ModuleDirectory, "../../plugins/CnD_Sound/GeoLocation/GeoLite2-City.mmdb")))
            {
                var response = reader.City(ipAddress);
                return response.Country.IsoCode ?? "U/C";
            }
        }
        catch (AddressNotFoundException)
        {
            return "U/C";
        }
        catch
        {
            return "U/C";
        }
    }

    private string GetCountry(string ipAddress)
    {
        try
        {
            using (var reader = new DatabaseReader(Path.Combine(ModuleDirectory, "../../plugins/CnD_Sound/GeoLocation/GeoLite2-City.mmdb")))
            {
                var response = reader.City(ipAddress);
                return response.Country.Name ?? "Unknown Country";
            }
        }
        catch (AddressNotFoundException)
        {
            return "Unknown Country";
        }
        catch
        {
            return "Unknown Country";
        }
    }

    private string GetCity(string ipAddress)
    {
        try
        {
            using (var reader = new DatabaseReader(Path.Combine(ModuleDirectory, "../../plugins/CnD_Sound/GeoLocation/GeoLite2-City.mmdb")))
            {
                var response = reader.City(ipAddress);
                return response.City.Name ?? "Unknown City";
            }
        }
        catch (AddressNotFoundException)
        {
            return "Unknown City";
        }
        catch
        {
            return "Unknown City";
        }
    }

    public async Task SendToDiscordWebhookNormal(string webhookUrl, string message)
    {
        try
        {
            var payload = new { content = message };
            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(webhookUrl, content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}, Response: {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    public async Task SendToDiscordWebhookNameLink(string webhookUrl, string message, string steamUserId, string STEAMNAME)
    {
        try
        {
            string profileLink = GetSteamProfileLink(steamUserId);
            int colorss = int.Parse(Config.SideColorMessage, System.Globalization.NumberStyles.HexNumber);
            Color color = Color.FromArgb(colorss >> 16, (colorss >> 8) & 0xFF, colorss & 0xFF);
            using (var httpClient = new HttpClient())
            {
                var embed = new
                {
                    type = "rich",
                    title = STEAMNAME,
                    url = profileLink,
                    description = message,
                    color = color.ToArgb() & 0xFFFFFF
                };

                var payload = new
                {
                    embeds = new[] { embed }
                };

                var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(webhookUrl, content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}, Response: {await response.Content.ReadAsStringAsync()}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
    public async Task SendToDiscordWebhookNameLinkWithPicture(string webhookUrl, string message, string steamUserId, string STEAMNAME)
    {
        try
        {
            string profileLink = GetSteamProfileLink(steamUserId);
            string profilePictureUrl = await GetProfilePictureAsync(steamUserId);
            int colorss = int.Parse(Config.SideColorMessage, System.Globalization.NumberStyles.HexNumber);
            Color color = Color.FromArgb(colorss >> 16, (colorss >> 8) & 0xFF, colorss & 0xFF);
            using (var httpClient = new HttpClient())
            {
                var embed = new
                {
                    type = "rich",
                    description = message,
                    color = color.ToArgb() & 0xFFFFFF,
                    author = new
                    {
                        name = STEAMNAME,
                        url = profileLink,
                        icon_url = profilePictureUrl
                    }
                };

                var payload = new
                {
                    embeds = new[] { embed }
                };

                var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(webhookUrl, content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}, Response: {await response.Content.ReadAsStringAsync()}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
    private static async Task<string> GetProfilePictureAsync(string steamId64)
    {
        try
        {
            string apiUrl = $"https://steamcommunity.com/profiles/{steamId64}/?xml=1";
            
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string xmlResponse = await response.Content.ReadAsStringAsync();
                int startIndex = xmlResponse.IndexOf("<avatarFull><![CDATA[") + "<avatarFull><![CDATA[".Length;
                int endIndex = xmlResponse.IndexOf("]]></avatarFull>", startIndex);
                string profilePictureUrl = xmlResponse.Substring(startIndex, endIndex - startIndex);

                return profilePictureUrl;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return null!;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return null!;
        }
    }
    static string GetSteamProfileLink(string userId)
    {
        return $"https://steamcommunity.com/profiles/{userId}";
    }
    static void DeleteOldFiles(string folderPath, string searchPattern, TimeSpan maxAge)
    {
        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            if (directoryInfo.Exists)
            {
                FileInfo[] files = directoryInfo.GetFiles(searchPattern);
                DateTime currentTime = DateTime.Now;
                
                foreach (FileInfo file in files)
                {
                    TimeSpan age = currentTime - file.LastWriteTime;

                    if (age > maxAge)
                    {
                        file.Delete();
                    }
                }
            }
            else
            {
                Console.WriteLine($"Directory not found: {folderPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    private void SaveToJsonFile(int id, bool boolValue, DateTime date)
    {
        string Fpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/Cookies/");
        string Fpathc = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/Cookies/CnD_Sound_Cookies.json");
        try
        {
            if(!Directory.Exists(Fpath))
            {
                Directory.CreateDirectory(Fpath);
            }

            if (!File.Exists(Fpathc))
            {
                File.WriteAllText(Fpathc, "[]");
            }

            List<PersonData> allPersonsData;
            string jsonData = File.ReadAllText(Fpathc);
            allPersonsData = JsonConvert.DeserializeObject<List<PersonData>>(jsonData) ?? new List<PersonData>();

            PersonData existingPerson = allPersonsData.Find(p => p.Id == id)!;

            if (existingPerson != null)
            {
                existingPerson.BoolValue = boolValue;
                existingPerson.Date = date;
            }
            else
            {
                PersonData newPerson = new PersonData { Id = id, BoolValue = boolValue, Date = date };
                allPersonsData.Add(newPerson);
            }
            allPersonsData.RemoveAll(p => (DateTime.Now - p.Date).TotalDays > Config.RemovePlayerCookieOlderThanXDays);

            string updatedJsonData = JsonConvert.SerializeObject(allPersonsData, Formatting.Indented);
            try
            {
                File.WriteAllText(Fpathc, updatedJsonData);
            }catch
            {
            }
        }catch
        {
        }
    }
    
    private bool RetrieveBoolValueById(int targetId)
    {
        string Fpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/Cookies/");
        string Fpathc = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/Cookies/CnD_Sound_Cookies.json");
        try
        {
            if (File.Exists(Fpathc))
            {
                string jsonData = File.ReadAllText(Fpathc);
                List<PersonData> allPersonsData = JsonConvert.DeserializeObject<List<PersonData>>(jsonData) ?? new List<PersonData>();

                PersonData targetPerson = allPersonsData.Find(p => p.Id == targetId)!;

                if (targetPerson != null)
                {
                    if (DateTime.Now - targetPerson.Date <= TimeSpan.FromDays(Config.RemovePlayerCookieOlderThanXDays))
                    {
                        return targetPerson.BoolValue;
                    }
                    else
                    {
                        allPersonsData.Remove(targetPerson);
                        string updatedJsonData = JsonConvert.SerializeObject(allPersonsData, Formatting.Indented);
                        try
                        {
                        File.WriteAllText(Fpathc, updatedJsonData);
                        }catch
                        {
                        }
                    }
                }
            }
            return false;
        }catch
        {
            return false;
        }
    }


    private class PersonData
    {
        public int Id { get; set; }
        public bool BoolValue { get; set; }
        public DateTime Date { get; set; }
    }
    public override void Unload(bool hotReload)
    {
        OnDisabled.Clear();
    }
}