using System.Text.Json;
using System.Text.Json.Serialization;

namespace CnD_Sound.Config
{
    public static class Configs
    {
        public static class Shared {
            public static string? CookiesFolderPath { get; set; }
        }
        
        private static readonly string ConfigDirectoryName = "config";
        private static readonly string ConfigFileName = "config.json";
        private static string? _configFilePath;
        private static ConfigData? _configData;

        private static readonly JsonSerializerOptions SerializationOptions = new()
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        public static bool IsLoaded()
        {
            return _configData is not null;
        }

        public static ConfigData GetConfigData()
        {
            if (_configData is null)
            {
                throw new Exception("Config not yet loaded.");
            }
            
            return _configData;
        }

        public static ConfigData Load(string modulePath)
        {
            var configFileDirectory = Path.Combine(modulePath, ConfigDirectoryName);
            if(!Directory.Exists(configFileDirectory))
            {
                Directory.CreateDirectory(configFileDirectory);
            }

            _configFilePath = Path.Combine(configFileDirectory, ConfigFileName);
            if (File.Exists(_configFilePath))
            {
                _configData = JsonSerializer.Deserialize<ConfigData>(File.ReadAllText(_configFilePath), SerializationOptions);
            }
            else
            {
                _configData = new ConfigData();
            }

            if (_configData is null)
            {
                throw new Exception("Failed to load configs.");
            }

            SaveConfigData(_configData);

            return _configData;
        }

        private static void SaveConfigData(ConfigData configData)
        {
            if (_configFilePath is null)
            {
                throw new Exception("Config not yet loaded.");
            }

            File.WriteAllText(_configFilePath, JsonSerializer.Serialize(configData, SerializationOptions));
        }

        public class ConfigData
        {
            public bool DisableLoopConnections { get; set; }
            public bool RemoveDefaultDisconnect { get; set; }
            public string InGameSoundConnect { get; set; }
            public string InGameSoundDisconnect { get; set; }
            public string InGameAllowDisableCommandsOnlyForGroups { get; set; }
            public string InGameSoundDisableCommands { get; set; }
            public int RemovePlayerCookieOlderThanXDays { get; set; }
            public string empty { get; set; }
            public bool SendLogToText { get; set; }
            public string Log_TextConnectMessageFormat { get; set; }
            public string Log_TextDisconnectMessageFormat { get; set; }
            public int Log_AutoDeleteLogsMoreThanXdaysOld { get; set; }
            private int _Log_SendLogToDiscordOnMode;
            public int Log_SendLogToDiscordOnMode
            {
                get => _Log_SendLogToDiscordOnMode;
                set
                {
                    _Log_SendLogToDiscordOnMode = value;
                    if (_Log_SendLogToDiscordOnMode < 0 || _Log_SendLogToDiscordOnMode > 3)
                    {
                        Log_SendLogToDiscordOnMode = 0;
                        Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||||||||||||||");
                        Console.WriteLine("[Vote-GoldKingZ] Log_SendLogToDiscordOnMode: is invalid, setting to default value (0) Please Choose 0 or 1 or 2 or 3.");
                        Console.WriteLine("[Vote-GoldKingZ] Log_SendLogToDiscordOnMode (0) = Disable");
                        Console.WriteLine("[Vote-GoldKingZ] Log_SendLogToDiscordOnMode (1) = Text Only");
                        Console.WriteLine("[Vote-GoldKingZ] Log_SendLogToDiscordOnMode (2) = Text With + Name + Hyperlink To Steam Profile");
                        Console.WriteLine("[Vote-GoldKingZ] Log_SendLogToDiscordOnMode (3) = Text With + Name + Hyperlink To Steam Profile + Profile Picture");
                        Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||||||||||||||");
                    }
                }
            }
            private string? _Log_DiscordSideColor;
            public string Log_DiscordSideColor
            {
                get => _Log_DiscordSideColor!;
                set
                {
                    _Log_DiscordSideColor = value;
                    if (_Log_DiscordSideColor.StartsWith("#"))
                    {
                        Log_DiscordSideColor = _Log_DiscordSideColor.Substring(1);
                    }
                }
            }
            public string Log_DiscordWebHookURL { get; set; }
            public string Log_DiscordConnectMessageFormat { get; set; }
            public string Log_DiscordDisconnectMessageFormat { get; set; }
            public string Log_DiscordUsersWithNoAvatarImage { get; set; }
            public string empty2 { get; set; }
            public string Information_For_You_Dont_Delete_it { get; set; }
            
            public ConfigData()
            {
                DisableLoopConnections = true;
                RemoveDefaultDisconnect = true;
                InGameSoundConnect = "sounds/buttons/blip1.vsnd_c";
                InGameSoundDisconnect = "sounds/player/taunt_clap_01.vsnd_c";
                InGameAllowDisableCommandsOnlyForGroups = "";
                InGameSoundDisableCommands = "!stopsound,!stopsounds";
                RemovePlayerCookieOlderThanXDays = 7;
                empty = "-----------------------------------------------------------------------------------";
                SendLogToText = false;
                Log_TextConnectMessageFormat = "[{DATE} - {TIME}] {PLAYERNAME} Connected [{SHORTCOUNTRY} - {CITY}] [{STEAMID} - {IP}]";
                Log_TextDisconnectMessageFormat = "[{DATE} - {TIME}] {PLAYERNAME} Disconnected [{SHORTCOUNTRY} - {CITY}] [{STEAMID64}] [{STEAMID} - {IP}] [{REASON}]";
                Log_AutoDeleteLogsMoreThanXdaysOld = 7;
                Log_SendLogToDiscordOnMode = 0;
                Log_DiscordSideColor = "00FFFF";
                Log_DiscordWebHookURL = "https://discord.com/api/webhooks/XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
                Log_DiscordConnectMessageFormat = "{PLAYERNAME} Connected [{LONGCOUNTRY} - {CITY}]";
                Log_DiscordDisconnectMessageFormat = "{PLAYERNAME} Disconnected [{LONGCOUNTRY} - {CITY}] [{REASON}]";
                Log_DiscordUsersWithNoAvatarImage = "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/b5/b5bd56c1aa4644a474a2e4972be27ef9e82e517e_full.jpg";
                empty2 = "-----------------------------------------------------------------------------------";
                Information_For_You_Dont_Delete_it = " Vist  [https://github.com/oqyh/cs2-Connect-Disconnect-Sound/tree/main?tab=readme-ov-file#-configuration-] To Understand All Above";
            }
        }
    }
}
