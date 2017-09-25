using System.Globalization;
using System.Threading.Tasks;
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

                CpuUsageBox.Text = $"{stats.Cpu.Usage.ToString(CultureInfo.InvariantCulture)}%";
                MemoryUsageBox.Text = $"{stats.Memory.Usage.ToString(CultureInfo.InvariantCulture)}%";
                
                CpuProgressBar.Foreground = new SolidColorBrush(stats.Cpu.Colour);
                MemoryProgressBar.Foreground = new SolidColorBrush(stats.Memory.Colour);

                CpuProgressBar.Value = stats.Cpu.Usage;
                MemoryProgressBar.Value = stats.Memory.Usage;

                await Task.Delay(1000);
            }
        }
    }
}
