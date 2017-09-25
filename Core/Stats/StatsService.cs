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

            var greenColour = Color.FromArgb(255, 39, 174, 96);
            var amberColour = Color.FromArgb(255, 230, 126, 34);
            var redColour = Color.FromArgb(255, 231, 76, 60);

            var cpuUsage = double.Parse(parsedResponse.cpu);
            var memoryUsage = double.Parse(parsedResponse.memory);

            var cpuColour = greenColour;
            var memoryColour = greenColour;

            if (cpuUsage > 75)
                cpuColour = redColour;

            if (cpuUsage > 50)
                cpuColour = amberColour;

            if (memoryUsage > 75)
                memoryColour = redColour;

            if (memoryUsage > 50)
                memoryColour = amberColour;
            
            return new ServerStatus
            {
                CpuUsage = cpuUsage,
                MemoryUsage = memoryUsage,
                CpuColour = cpuColour,
                MemoryColour = memoryColour
            };
        }
    }
}