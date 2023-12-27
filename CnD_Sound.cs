using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using System.Text;

namespace CnD_Sound;

public class CnDSoundConfig : BasePluginConfig
{
    [JsonPropertyName("MessageConsoleFormatConnect")] public string MessageConsoleFormatConnect { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {lime}Connected [{SHORTCOUNTRY} - {CITY}]";
    [JsonPropertyName("MessageConsoleFormatDisconnect")] public string MessageConsoleFormatDisconnect { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {red}Disconnected [{SHORTCOUNTRY} - {CITY}]";



    [JsonPropertyName("ConnectSound")] public bool ConnectSound { get; set; } = true;
    [JsonPropertyName("ConnectSoundPath")] public string ConnectSoundPath { get; set; } = "sounds/buttons/blip1.vsnd_c";
    [JsonPropertyName("DisconnectSound")] public bool DisconnectSound { get; set; } = true;
    [JsonPropertyName("DisconnectSoundPath")] public string DisconnectSoundPath { get; set; } = "sounds/player/taunt_clap_01.vsnd_c";



    [JsonPropertyName("SendLogToText")] public bool SendLogToText { get; set; } = false;
    [JsonPropertyName("LogFileFormat")] public string LogFileFormat { get; set; } = ".txt";
    [JsonPropertyName("LogFileDateFormat")] public string LogFileDateFormat { get; set; } = "MM-dd-yyyy";
    [JsonPropertyName("LogInsideFileTimeFormat")] public string LogInsideFileTimeFormat { get; set; } = "HH:mm:ss";
    [JsonPropertyName("LogTextFormatConnect")] public string LogTextFormatConnect { get; set; } = "[{TIME}] [Playername:{PLAYERNAME}] CONNECTED TO THE SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]";
    [JsonPropertyName("LogTextFormatDisconnect")] public string LogTextFormatDisconnect { get; set; } = "[{TIME}] [Playername:{PLAYERNAME}] DISCONNECTED FROM SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]";



    [JsonPropertyName("SendLogToWebHook")] public bool SendLogToWebHook { get; set; } = false;
    [JsonPropertyName("WebHookURL")] public string WebHookURL { get; set; } = "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
    [JsonPropertyName("LogDiscordChatFormatConnect")] public string LogDiscordChatFormatConnect { get; set; } = "[{DATE} - {TIME}] CONNECTED TO THE SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]";
    [JsonPropertyName("LogDiscordChatFormatDisconnect")] public string LogDiscordChatFormatDisconnect { get; set; } = "[{DATE} - {TIME}] DISCONNECTED FROM SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]";
	
	
	[JsonPropertyName("SendLogToServerConsole")] public bool SendLogToServerConsole { get; set; } = false;
    [JsonPropertyName("LogServerConsoleFormatConnect")] public string LogServerConsoleFormatConnect { get; set; } = "[{DATE} - {TIME}] [Playername:{PLAYERNAME}] CONNECTED TO THE SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]";
    [JsonPropertyName("LogServerConsoleFormatDisconnect")] public string LogServerConsoleFormatDisconnect { get; set; } = "[{DATE} - {TIME}] [Playername:{PLAYERNAME}] DISCONNECTED FROM SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]";
}

public class CnDSound : BasePlugin, IPluginConfig<CnDSoundConfig>
{
    public override string ModuleName => "Connect Disconnect Sound";
    public override string ModuleVersion => "1.0.4";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "Connect , Disconnect , Country , City , Message , Sound , Logs , Discord";
    public CnDSoundConfig Config { get; set; } = new CnDSoundConfig();
    public void OnConfigParsed(CnDSoundConfig config)
    {
        Config = config;
    }
    
    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnClientPutInServer>(OnClientPutInServer);
        RegisterListener<Listeners.OnClientDisconnect>(OnClientDisconnect);
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
        
        CCSPlayerController player = Utilities.GetPlayerFromSlot(playerSlot);
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.AuthorizedSteamID == null)return;

        var JoinPlayer = player.PlayerName;
        var steamId2 = player.AuthorizedSteamID.SteamId2;
        var steamId3 = player.AuthorizedSteamID.SteamId3;
        var steamId32 = player.AuthorizedSteamID.SteamId32;
        var steamId64 = player.AuthorizedSteamID.SteamId64;
        var GetIpAddress = NativeAPI.GetPlayerIpAddress(playerSlot);
        var ipAddress = GetIpAddress.Split(':')[0];
        var Country = GetCountry(ipAddress);
        var SCountry = GetCountryS(ipAddress);
        var City = GetCity(ipAddress);
        string emp = " ";

        if (!string.IsNullOrEmpty(Config.MessageConsoleFormatConnect))
        {
            var replacer = ReplaceMessages(emp + Config.MessageConsoleFormatConnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            Server.PrintToChatAll(replacer);
        }

        if(Config.SendLogToServerConsole)
        {
            if (!string.IsNullOrEmpty(Config.LogServerConsoleFormatConnect))
            {
                var replacer = ReplaceMessages(Config.LogServerConsoleFormatConnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
                Server.PrintToConsole(replacer);
            }
        }

        if(Config.SendLogToText)
        {
            var replacerlog = ReplaceMessages(Config.LogTextFormatConnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
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

        if(Config.SendLogToWebHook)
        {
            var replacerlog = ReplaceMessages(Config.LogDiscordChatFormatConnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            if (!string.IsNullOrEmpty(Config.LogDiscordChatFormatConnect))
            {
                Task.Run(() => SendToDiscordWebhook(Config.WebHookURL, replacerlog, steamId64.ToString(), JoinPlayer));
            }
        }

        if(Config.ConnectSound)
        {
            foreach(var players in GetPlayerControllers().FindAll(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot))
            {
                if (!player.IsValid) continue;

                players.ExecuteClientCommand("play " + Config.ConnectSoundPath);
            }
        }
    }
    private void OnClientDisconnect(int playerSlot)
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
        
        CCSPlayerController player = Utilities.GetPlayerFromSlot(playerSlot);
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.AuthorizedSteamID == null)return;

        var JoinPlayer = player.PlayerName;
        var steamId2 = player.AuthorizedSteamID.SteamId2;
        var steamId3 = player.AuthorizedSteamID.SteamId3;
        var steamId32 = player.AuthorizedSteamID.SteamId32;
        var steamId64 = player.AuthorizedSteamID.SteamId64;
        var GetIpAddress = NativeAPI.GetPlayerIpAddress(playerSlot);
        var ipAddress = GetIpAddress.Split(':')[0];
        var Country = GetCountry(ipAddress);
        var SCountry = GetCountryS(ipAddress);
        var City = GetCity(ipAddress);
        string emp = " ";

        if (!string.IsNullOrEmpty(Config.MessageConsoleFormatDisconnect))
        {
            var replacer = ReplaceMessages(emp + Config.MessageConsoleFormatDisconnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            Server.PrintToChatAll(replacer);
        }

        if(Config.SendLogToServerConsole)
        {
            if (!string.IsNullOrEmpty(Config.LogServerConsoleFormatDisconnect))
            {
                var replacer = ReplaceMessages(Config.LogServerConsoleFormatDisconnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
                Server.PrintToConsole(replacer);
            }
        }

        if(Config.SendLogToText)
        {
            var replacerlog = ReplaceMessages(Config.LogTextFormatDisconnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
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

        if(Config.SendLogToWebHook)
        {
            var replacerlog = ReplaceMessages(Config.LogDiscordChatFormatDisconnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            if (!string.IsNullOrEmpty(Config.LogDiscordChatFormatDisconnect))
            {
                Task.Run(() => SendToDiscordWebhook(Config.WebHookURL, replacerlog, steamId64.ToString(), JoinPlayer));
            }
        }

        if(Config.DisconnectSound)
        {
            foreach(var players in GetPlayerControllers().FindAll(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot))
            {
                if (!player.IsValid) continue;

                players.ExecuteClientCommand("play " + Config.DisconnectSoundPath);
            }
        }
    }
    private string ReplaceMessages(string Message, string date, string time, string PlayerName, string SteamId, string SteamId3, string SteamId32, string SteamId64, string ipAddress, string Country, string SCountry, string City)
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
                                    .Replace("{CITY}", City);
        replacedMessage = ReplaceColors(replacedMessage);
        return replacedMessage;
    }

    private string ReplaceColors(string input)
    {
        string[] colorPatterns =
        {
            "{default}", "{white}", "{darkred}", "{green}", "{lightyellow}",
            "{lightblue}", "{olive}", "{lime}", "{red}", "{lightpurple}",
            "{purple}", "{grey}", "{yellow}", "{gold}", "{silver}",
            "{blue}", "{darkblue}", "{bluegrey}", "{magenta}", "{lightred}",
            "{orange}"
        };
        string[] colorReplacements =
        {
            "\x01", "\x01", "\x02", "\x04", "\x09", "\x0B", "\x05",
            "\x06", "\x07", "\x03", "\x0E", "\x08", "\x09", "\x10",
            "\x0A", "\x0B", "\x0C", "\x0A", "\x0E", "\x0F", "\x10"
        };

        for (var i = 0; i < colorPatterns.Length; i++)
            input = input.Replace(colorPatterns[i], colorReplacements[i]);

        return input;
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

    static async Task SendToDiscordWebhook(string webhookUrl, string message, string steamUserId, string STEAMNAME)
{
    string profileLink = GetSteamProfileLink(steamUserId);

    using (HttpClient client = new HttpClient())
    {
        var embed = new
        {
            description = message,
            author = new
            {
                name = STEAMNAME,
                url = profileLink
            }
        };

        var payload = new
        {
            embeds = new[] { embed }
        };

        var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(webhookUrl, content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Message sent successfully!");
        }
        else
        {
            Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}, Response: {await response.Content.ReadAsStringAsync()}");
        }
    }
}

    static string GetSteamProfileLink(string userId)
    {
        return $"https://steamcommunity.com/profiles/{userId}";
    }
}