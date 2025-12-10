using System.Net.Http.Json;
using Cs2SkinsApp.Models;

namespace Cs2SkinsApp.Services
{
    public class Cs2SkinService
    {
        private readonly HttpClient _httpClient;

        private const string SkinsUrl =
            "https://raw.githubusercontent.com/ByMykel/CSGO-API/main/public/api/en/skins.json";

        public Cs2SkinService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(List<Skin> Skins, string? Error)> GetSkinsAsync()
        {
            try
            {
                var skins = await _httpClient.GetFromJsonAsync<List<Skin>>(SkinsUrl);

                if (skins == null || skins.Count == 0)
                {
                    return (new List<Skin>(), "No skins were returned from the API.");
                }

                return (skins, null);
            }
            catch (Exception ex)
            {
                return (new List<Skin>(), $"Error loading skins: {ex.Message}");
            }
        }
    }
}
