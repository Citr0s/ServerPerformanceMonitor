using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Newtonsoft.Json;

namespace Core.Stats
{
    public class StatsService
    {
        private readonly HttpClient _httpClient;

        public StatsService()
        {
            var rootFilter = new HttpBaseProtocolFilter();
            rootFilter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;
            rootFilter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;

            _httpClient = new HttpClient(rootFilter);
            _httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        }

        public async Task<ServerStatus> GetStatus()
        {
            var uri = new Uri("https://ci.miloszdura.com/api/usage");
            var result = await _httpClient.GetStringAsync(uri);

            var parsedResponse = JsonConvert.DeserializeObject<ServerStatusResponse>(result);
            
            return new ServerStatus
            {
                Cpu = new Statistic
                {
                    Usage = parsedResponse.cpu,
                    Colour = GetStatusColour(parsedResponse.cpu)
                },
                Memory = new Statistic
                {
                    Usage = parsedResponse.memory,
                    Colour = GetStatusColour(parsedResponse.memory)
                }
            };
        }

        private static Color GetStatusColour(double usage)
        {
            var greenColour = Color.FromArgb(255, 39, 174, 96);
            var amberColour = Color.FromArgb(255, 230, 126, 34);
            var redColour = Color.FromArgb(255, 231, 76, 60);

            var colour = greenColour;

            if (usage > 75)
                colour = redColour;

            if (usage > 50)
                colour = amberColour;

            return colour;
        }
    }
}