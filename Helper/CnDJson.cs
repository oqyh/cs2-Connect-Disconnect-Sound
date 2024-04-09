using Newtonsoft.Json;
using CnD_Sound.Config;
using System.Text;
using System.Drawing;

namespace CnD_Sound;

public class CnD_SoundJson
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly HttpClient httpClient = new HttpClient();
    private class PersonData
    {
        public int Id { get; set; }
        public bool BoolValue { get; set; }
        public DateTime Date { get; set; }
    }
    public static async Task SendToDiscordWebhookNormal(string webhookUrl, string message)
    {
        try
        {
            var payload = new { content = message };
            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(webhookUrl, content).ConfigureAwait(false);

            
        }
        catch
        {
        }
    }

    public static async Task SendToDiscordWebhookNameLink(string webhookUrl, string message, string steamUserId, string STEAMNAME)
    {
        try
        {
            string profileLink = GetSteamProfileLink(steamUserId);
            int colorss = int.Parse(Configs.GetConfigData().Log_DiscordSideColor, System.Globalization.NumberStyles.HexNumber);
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

            }
        }
        catch 
        {
        }
    }
    public static async Task SendToDiscordWebhookNameLinkWithPicture(string webhookUrl, string message, string steamUserId, string STEAMNAME)
    {
        try
        {
            string profileLink = GetSteamProfileLink(steamUserId);
            string profilePictureUrl = await GetProfilePictureAsync(steamUserId, Configs.GetConfigData().Log_DiscordUsersWithNoAvatarImage);
            int colorss = int.Parse(Configs.GetConfigData().Log_DiscordSideColor, System.Globalization.NumberStyles.HexNumber);
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
            }
        }
        catch
        {

        }
    }
    public static async Task<string> GetProfilePictureAsync(string steamId64, string defaultImage)
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

                if (endIndex >= 0)
                {
                    string profilePictureUrl = xmlResponse.Substring(startIndex, endIndex - startIndex);
                    return profilePictureUrl;
                }
                else
                {
                    return defaultImage;
                }
            }
            else
            {
                return null!;
            }
        }
        catch
        {
            return null!;
        }
    }
    public static string GetSteamProfileLink(string userId)
    {
        return $"https://steamcommunity.com/profiles/{userId}";
    }
    
    public static  void SaveToJsonFile(int id, bool boolValue, DateTime date)
    {
        string cookiesFilePath = Configs.Shared.CookiesFolderPath!;
        string Fpath = Path.Combine(cookiesFilePath,"../../plugins/CnD_Sound/Cookies/");
        string Fpathc = Path.Combine(cookiesFilePath,"../../plugins/CnD_Sound/Cookies/CnD_Sound_Cookies.json");
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
            allPersonsData.RemoveAll(p => (DateTime.Now - p.Date).TotalDays > Configs.GetConfigData().RemovePlayerCookieOlderThanXDays);

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
    
    public static  bool RetrieveBoolValueById(int targetId)
    {
        string cookiesFilePath = Configs.Shared.CookiesFolderPath!;
        string Fpath = Path.Combine(cookiesFilePath,"../../plugins/CnD_Sound/Cookies/");
        string Fpathc = Path.Combine(cookiesFilePath,"../../plugins/CnD_Sound/Cookies/CnD_Sound_Cookies.json");
        try
        {
            if (File.Exists(Fpathc))
            {
                string jsonData = File.ReadAllText(Fpathc);
                List<PersonData> allPersonsData = JsonConvert.DeserializeObject<List<PersonData>>(jsonData) ?? new List<PersonData>();

                PersonData targetPerson = allPersonsData.Find(p => p.Id == targetId)!;

                if (targetPerson != null)
                {
                    if (DateTime.Now - targetPerson.Date <= TimeSpan.FromDays(Configs.GetConfigData().RemovePlayerCookieOlderThanXDays))
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
}