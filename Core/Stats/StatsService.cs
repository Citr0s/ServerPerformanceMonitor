using System;
using System.Threading.Tasks;
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
                CpuUsage = double.Parse(parsedResponse.cpu),
                MemoryUsage = double.Parse(parsedResponse.memory)
            };
        }
    }

    public class ServerStatusResponse
    {
        public string cpu { get; set; }
        public string memory { get; set; }
    }

    public class ServerStatus
    {
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
    }
}