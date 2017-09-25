using System.Globalization;
using System.Threading.Tasks;
using Core.Stats;

namespace ServerPerformanceMonitor
{
    public sealed partial class MainPage
    {
        private readonly StatsService _statsService;

        public MainPage()
        {
            InitializeComponent();

            _statsService = new StatsService();
            GetServerStatus();
        }

        private async void GetServerStatus()
        {
            while (true)
            {
                var stats = await _statsService.GetStatus();

                CpuUsageBox.Text = $"{stats.CpuUsage.ToString(CultureInfo.InvariantCulture)}%";
                MemoryUsageBox.Text = $"{stats.MemoryUsage.ToString(CultureInfo.InvariantCulture)}%";

                await Task.Delay(1000);
            }
        }
    }
}
