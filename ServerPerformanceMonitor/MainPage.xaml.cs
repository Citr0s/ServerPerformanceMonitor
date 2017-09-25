using System.Globalization;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
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
                
                CpuProgressBar.Foreground = new SolidColorBrush(stats.CpuColour);
                MemoryProgressBar.Foreground = new SolidColorBrush(stats.MemoryColour);

                CpuProgressBar.Value = stats.CpuUsage;
                MemoryProgressBar.Value = stats.MemoryUsage;

                await Task.Delay(1000);
            }
        }
    }
}
