using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using System.Text;
using System.Drawing;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Entities;


namespace CnD_Sound;

public class CnDSoundConfig : BasePluginConfig
{
    [JsonPropertyName("InGameMessageFormatConnect")] public string InGameMessageFormatConnect { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {lime}Connected [{SHORTCOUNTRY} - {CITY}]";
    [JsonPropertyName("InGameMessageFormatDisconnect")] public string InGameMessageFormatDisconnect { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {red}Disconnected [{SHORTCOUNTRY} - {CITY}]";



    [JsonPropertyName("InGameSoundConnect")] public string InGameSoundConnect { get; set; } = "sounds/buttons/blip1.vsnd_c";
    [JsonPropertyName("InGameSoundDisconnect")] public string InGameSoundDisconnect { get; set; } = "sounds/player/taunt_clap_01.vsnd_c";



    [JsonPropertyName("SendLogToText")] public bool SendLogToText { get; set; } = false;
    [JsonPropertyName("LogFileFormat")] public string LogFileFormat { get; set; } = ".txt";
    [JsonPropertyName("LogFileDateFormat")] public string LogFileDateFormat { get; set; } = "MM-dd-yyyy";
    [JsonPropertyName("LogInsideFileTimeFormat")] public string LogInsideFileTimeFormat { get; set; } = "HH:mm:ss";
    [JsonPropertyName("LogTextFormatConnect")] public string LogTextFormatConnect { get; set; } = "[{DATE} - {TIME}] {PLAYERNAME} Connected [{SHORTCOUNTRY} - {CITY}] [{STEAMID} - {IP}]";
    [JsonPropertyName("LogTextFormatDisconnect")] public string LogTextFormatDisconnect { get; set; } = "[{DATE} - {TIME}] {PLAYERNAME} Disconnected [{SHORTCOUNTRY} - {CITY}] [{STEAMID64}] [{STEAMID} - {IP}]";
    [JsonPropertyName("AutoDeleteLogsMoreThanXdaysOld")] public int AutoDeleteLogsMoreThanXdaysOld { get; set; } = 0;



    [JsonPropertyName("SendLogToWebHook")] public int SendLogToWebHook { get; set; } = 0;
    [JsonPropertyName("SideColorMessage")] public string SideColorMessage { get; set; } = "00FFFF";
    [JsonPropertyName("WebHookURL")] public string WebHookURL { get; set; } = "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
    [JsonPropertyName("LogDiscordChatFormatConnect")] public string LogDiscordChatFormatConnect { get; set; } = "{PLAYERNAME} Connected [{LONGCOUNTRY} - {CITY}]";
    [JsonPropertyName("LogDiscordChatFormatDisconnect")] public string LogDiscordChatFormatDisconnect { get; set; } = "{PLAYERNAME} Disconnected [{LONGCOUNTRY} - {CITY}]";



	[JsonPropertyName("SendLogToServerConsole")] public bool SendLogToServerConsole { get; set; } = false;
    [JsonPropertyName("LogServerConsoleFormatConnect")] public string LogServerConsoleFormatConnect { get; set; } = "Gold KingZ | {PLAYERNAME} Connected [{SHORTCOUNTRY} - {CITY}]";
    [JsonPropertyName("LogServerConsoleFormatDisconnect")] public string LogServerConsoleFormatDisconnect { get; set; } = "Gold KingZ | {PLAYERNAME} Disconnected [{SHORTCOUNTRY} - {CITY}]";
}

public class CnDSound : BasePlugin, IPluginConfig<CnDSoundConfig>
{
    public override string ModuleName => "Connect Disconnect Sound";
    public override string ModuleVersion => "1.0.5";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "Connect , Disconnect , Country , City , Message , Sound , Logs , Discord";
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly HttpClient httpClient = new HttpClient();
    public CnDSoundConfig Config { get; set; } = new CnDSoundConfig();
    public void OnConfigParsed(CnDSoundConfig config)
    {
        Config = config;

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
            //Console.WriteLine("SideColorMessage: # Detect At Start No Need For That");
        }
    }
    
    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnClientPutInServer>(OnClientPutInServer);
        RegisterListener<Listeners.OnClientDisconnect>(OnClientDisconnect);

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

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.AuthorizedSteamID == null|| player.IpAddress == null)return;

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

        if (!string.IsNullOrEmpty(Config.InGameMessageFormatConnect))
        {
            var replacer = ReplaceMessages(emp + Config.InGameMessageFormatConnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
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

        var replacerDISCORDlog = ReplaceMessages(Config.LogDiscordChatFormatConnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
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
                if (!player.IsValid) continue;
                players.ExecuteClientCommand("play " + Config.InGameSoundConnect);
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
        
        var player = Utilities.GetPlayerFromSlot(playerSlot);

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.AuthorizedSteamID == null|| player.IpAddress == null)return;

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

        if (!string.IsNullOrEmpty(Config.InGameMessageFormatDisconnect))
        {
            var replacer = ReplaceMessages(emp + Config.InGameMessageFormatDisconnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
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
        
        var replacerDISCORDlog = ReplaceMessages(Config.LogDiscordChatFormatDisconnect, Date, Time, JoinPlayer, steamId2, steamId3, steamId32.ToString(), steamId64.ToString(), ipAddress.ToString(), Country, SCountry, City);
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

        if (!string.IsNullOrEmpty(Config.InGameSoundDisconnect))
        {
            foreach(var players in GetPlayerControllers().FindAll(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot))
            {
                if (!player.IsValid) continue;
                players.ExecuteClientCommand("play " + Config.InGameSoundDisconnect);
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
                        //Console.WriteLine($"Deleted file: {file.FullName}");
                    }
                }

                //Console.WriteLine("Deletion process completed.");
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
}