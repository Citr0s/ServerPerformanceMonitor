using Core.Stats;

namespace ServerPerformanceMonitor
{
    public sealed partial class MainPage
    {
        private StatsService _statsService;

        public MainPage()
        {
            InitializeComponent();

            _statsService = new StatsService();
        }
    }
}
