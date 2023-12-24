using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using System.Text;

namespace CnD_Sound;

public class CnDSoundConfig : BasePluginConfig
{
    [JsonPropertyName("ConnectPlayers")] public string ConnectPlayers { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {lime}Connected {SHORTCOUNTRY} {CITY}";
    [JsonPropertyName("DisconnectPlayers")] public string DisconnectPlayers { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {red}Disconnected {SHORTCOUNTRY} {CITY}";


    [JsonPropertyName("ConnectSound")] public bool ConnectSound { get; set; } = false;
    [JsonPropertyName("ConnectSoundPath")] public string ConnectSoundPath { get; set; } = "sounds/buttons/bell1.vsnd_c";
    [JsonPropertyName("DisconnectSound")] public bool DisconnectSound { get; set; } = false;
    [JsonPropertyName("DisconnectSoundPath")] public string DisconnectSoundPath { get; set; } = "sounds/buttons/blip1.vsnd_c";


    [JsonPropertyName("CnDModeLogs")] public bool CnDModeLogs { get; set; } = false;
    [JsonPropertyName("LogFileFormat")] public string LogFileFormat { get; set; } = ".txt";
    [JsonPropertyName("LogFileDateFormat")] public string LogFileDateFormat { get; set; } = "MM-dd-yyyy";
    [JsonPropertyName("LogInsideFileTimeFormat")] public string LogInsideFileTimeFormat { get; set; } = "HH:mm:ss";
    [JsonPropertyName("ConnectPlayersLog")] public string ConnectPlayersLog { get; set; } = "[Playername:{PLAYERNAME}] CONNECTED TO THE SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]";
    [JsonPropertyName("DisconnectPlayersLog")] public string DisconnectPlayersLog { get; set; } = "[Playername:{PLAYERNAME}] DISCONNECTED FROM SERVER [SteamdID64:{STEAMID64}] [IpAddress:{IP}] [Long Country:{LONGCOUNTRY}] [City:{CITY}]";

    [JsonPropertyName("SendLogToWebHook")] public bool SendLogToWebHook { get; set; } = false;
    [JsonPropertyName("WebHookURL")] public string WebHookURL { get; set; } = "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
}

public class CnDSound : BasePlugin, IPluginConfig<CnDSoundConfig>
{
    public override string ModuleName => "Connect Disconnect Sound";
    public override string ModuleVersion => "1.0.2";
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

        if(Config.CnDModeLogs && !Directory.Exists(Fpath))
        {
            Directory.CreateDirectory(Fpath);
        }

        if(Config.CnDModeLogs && !File.Exists(Tpath))
        {
            File.Create(Tpath);
        }
        

        CCSPlayerController player = Utilities.GetPlayerFromSlot(playerSlot);
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.AuthorizedSteamID == null)return;

        var JoinPlayer = player.PlayerName;
        var steamId2 = player.AuthorizedSteamID.SteamId2;
        var steamId64 = player.AuthorizedSteamID.SteamId64;
        var GetIpAddress = NativeAPI.GetPlayerIpAddress(playerSlot);
        var ipAddress = GetIpAddress.Split(':')[0];
        var Country = GetCountry(ipAddress);
        var SCountry = GetCountryS(ipAddress);
        var City = GetCity(ipAddress);
        //playerz = Utilities.GetPlayerFromUserid(userid);
        //steamid = playerz.SteamID.ToString();
        string emp = " ";

        if (!string.IsNullOrEmpty(Config.ConnectPlayers))
        {
            var replacer = ReplaceMessages(emp + Config.ConnectPlayers, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            Server.PrintToChatAll(replacer);
        }

        if(Config.CnDModeLogs)
        {
            var replacerlog = ReplaceMessages(Config.ConnectPlayersLog, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            if (!string.IsNullOrEmpty(Config.ConnectPlayersLog) && File.Exists(Tpath))
            {
                File.AppendAllLines(Tpath, new[]{ Time + emp + replacerlog});
            }
        }
        if(Config.SendLogToWebHook)
        {
            var replacerlog = ReplaceMessages(Config.ConnectPlayersLog, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            if (!string.IsNullOrEmpty(Config.ConnectPlayersLog))
            {
                Task.WaitAll(SendToDiscordWebhook(Config.WebHookURL,  Date + emp + Time + emp + replacerlog));
            }
        }

        

        if(!Config.ConnectSound)return;

        foreach(var players in GetPlayerControllers().FindAll(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot))
        {
            if (!player.IsValid) continue;

            players.ExecuteClientCommand("play " + Config.ConnectSoundPath);
        }
    }
    private void OnClientDisconnect(int playerSlot)
    {
        string Fpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/");
        string Time = DateTime.Now.ToString(Config.LogInsideFileTimeFormat);
        string Date = DateTime.Now.ToString(Config.LogFileDateFormat);
        string fileName = DateTime.Now.ToString(Config.LogFileDateFormat) + Config.LogFileFormat;
        string Tpath = Path.Combine(ModuleDirectory,"../../plugins/CnD_Sound/logs/") + $"{fileName}";

        if(Config.CnDModeLogs && !Directory.Exists(Fpath))
        {
            Directory.CreateDirectory(Fpath);
        }

        if(Config.CnDModeLogs && !File.Exists(Tpath))
        {
            File.Create(Tpath);
        }
        

        CCSPlayerController player = Utilities.GetPlayerFromSlot(playerSlot);
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.AuthorizedSteamID == null)return;

        var JoinPlayer = player.PlayerName;
        var steamId2 = player.AuthorizedSteamID.SteamId2;
        var steamId64 = player.AuthorizedSteamID.SteamId64;
        var GetIpAddress = NativeAPI.GetPlayerIpAddress(playerSlot);
        var ipAddress = GetIpAddress.Split(':')[0];
        var Country = GetCountry(ipAddress);
        var SCountry = GetCountryS(ipAddress);
        var City = GetCity(ipAddress);
        //playerz = Utilities.GetPlayerFromUserid(userid);
        //steamid = playerz.SteamID.ToString();
        string emp = " ";

        if (!string.IsNullOrEmpty(Config.DisconnectPlayers))
        {
            var replacer = ReplaceMessages(emp + Config.DisconnectPlayers, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            Server.PrintToChatAll(replacer);
        }

        if(Config.CnDModeLogs)
        {
            var replacerlog = ReplaceMessages(Config.DisconnectPlayersLog, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            if (!string.IsNullOrEmpty(Config.DisconnectPlayersLog) && File.Exists(Tpath))
            {
                File.AppendAllLines(Tpath, new[]{ Time + emp + replacerlog});
            }
        }
        if(Config.SendLogToWebHook)
        {
            var replacerlog = ReplaceMessages(Config.DisconnectPlayersLog, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
            if (!string.IsNullOrEmpty(Config.ConnectPlayersLog))
            {
                Task.WaitAll(SendToDiscordWebhook(Config.WebHookURL,  Date + emp + Time + emp + replacerlog));
            }
        }

        

        if(!Config.DisconnectSound)return;

        foreach(var players in GetPlayerControllers().FindAll(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot))
        {
            if (!player.IsValid) continue;

            players.ExecuteClientCommand("play " + Config.DisconnectSoundPath);
        }
    }
    private string ReplaceMessages(string Message, string PlayerName, string SteamId, string SteamId64, string ipAddress, string Country, string SCountry, string City)
    {
        var replacedMessage = Message
                                    .Replace("{PLAYERNAME}", PlayerName.ToString())
                                    .Replace("{STEAMID}", SteamId.ToString())
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
    static bool IsPlayerValid(CCSPlayerController? player)
    {
        return (player != null && player.IsValid && !player.IsBot && !player.IsHLTV && player.AuthorizedSteamID != null);
    }
    static async Task SendToDiscordWebhook(string webhookUrl, string message)
    {
        using (HttpClient client = new HttpClient())
        {
            var payload = new { content = message };
            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(webhookUrl, content);

            if (response.IsSuccessStatusCode)
            {
                
            }
            else
            {
                Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}, Response: {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
    
}