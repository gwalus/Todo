using Microcharts;
using SkiaSharp;
using Todo.Interfaces;

namespace Todo.Services
{
    public class MicrochartsService : IMicrochartsService
    {
        public ChartEntry SetChartEntry(string label, int value, string hexaColor)
        {
            var chartEntry = new ChartEntry(value)
            {
                Label = label,
                ValueLabel = value.ToString(),
                ValueLabelColor = SKColor.Parse(hexaColor),
                Color = SKColor.Parse(hexaColor)
            };

            return chartEntry;
        }
    }
}
