using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;

namespace CnD_Sound;

public class CnDSoundConfig : BasePluginConfig
{
    [JsonPropertyName("ConnectPlayers")] public string ConnectPlayers { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {lime}Connected To The Server {STEAMID}";
    [JsonPropertyName("DisconnectPlayers")] public string DisconnectPlayers { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {red}Disconnected To The Server {STEAMID}";


    [JsonPropertyName("ConnectSound")] public bool ConnectSound { get; set; } = false;
    [JsonPropertyName("ConnectSoundPath")] public string ConnectSoundPath { get; set; } = "sounds/buttons/bell1.vsnd_c";
    [JsonPropertyName("DisconnectSound")] public bool DisconnectSound { get; set; } = false;
    [JsonPropertyName("DisconnectSoundPath")] public string DisconnectSoundPath { get; set; } = "sounds/buttons/blip1.vsnd_c";


    [JsonPropertyName("CnDModeLogs")] public bool CnDModeLogs { get; set; } = false;
    [JsonPropertyName("LogFileFormat")] public string LogFileFormat { get; set; } = ".txt";
    [JsonPropertyName("LogFileDateFormat")] public string LogFileDateFormat { get; set; } = "MM-dd-yyyy";
    [JsonPropertyName("LogInsideFileTimeFormat")] public string LogInsideFileTimeFormat { get; set; } = "HH:mm:ss";
    [JsonPropertyName("ConnectPlayersLog")] public string ConnectPlayersLog { get; set; } = "{PLAYERNAME} Connected SteamdID:{STEAMID} ipAddress:{IP}";
    [JsonPropertyName("DisconnectPlayersLog")] public string DisconnectPlayersLog { get; set; } = "{PLAYERNAME} Disconnected SteamdID:{STEAMID} ipAddress:{IP}";
}

public class CnDSound : BasePlugin, IPluginConfig<CnDSoundConfig>
{
    public override string ModuleName => "Connect Disconnect Sound";
    public override string ModuleVersion => "1.0.1";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "Connect , Disconnect , Message , Sound , Logs";
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
        //playerz = Utilities.GetPlayerFromUserid(userid);
        //steamid = playerz.SteamID.ToString();
        string emp = " ";

        
        var replacer = ReplaceMessages(emp + Config.ConnectPlayers, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString());
        Server.PrintToChatAll(replacer);

        if(Config.CnDModeLogs)
        {
            var replacerlog = ReplaceMessages(Config.ConnectPlayersLog, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString());
            File.AppendAllLines(Tpath, new[]{ Time + emp + replacerlog});
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
        //playerz = Utilities.GetPlayerFromUserid(userid);
        //steamid = playerz.SteamID.ToString();
        string emp = " ";

        
        var replacer = ReplaceMessages(emp + Config.DisconnectPlayers, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString());
        Server.PrintToChatAll(replacer);

        if(Config.CnDModeLogs)
        {
            var replacerlog = ReplaceMessages(Config.DisconnectPlayersLog, JoinPlayer, steamId2, steamId64.ToString(), ipAddress.ToString());
            File.AppendAllLines(Tpath, new[]{ Time + emp + replacerlog});
        }

        

        if(!Config.DisconnectSound)return;

        foreach(var players in GetPlayerControllers().FindAll(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot))
        {
            if (!player.IsValid) continue;

            players.ExecuteClientCommand("play " + Config.DisconnectSoundPath);
        }
    }

    
    private string ReplaceMessages(string Message, string PlayerName, string SteamId, string SteamId64, string ipAddress)
    {
        var replacedMessage = Message
                                    .Replace("{PLAYERNAME}", PlayerName.ToString())
                                    .Replace("{STEAMID}", SteamId.ToString())
                                    .Replace("{STEAMID64}", SteamId64.ToString())
                                    .Replace("{IP}", ipAddress.ToString());
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
    static bool IsPlayerValid(CCSPlayerController? player)
    {
        return (player != null && player.IsValid && !player.IsBot && !player.IsHLTV && player.AuthorizedSteamID != null);
    }
}