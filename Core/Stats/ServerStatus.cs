using Windows.UI;

namespace Core.Stats
{
    public class ServerStatus
    {
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public Color CpuColour { get; set; }
        public Color MemoryColour { get; set; }
    }
}