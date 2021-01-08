using Microcharts;

namespace Todo.Interfaces
{
    public interface IMicrochartsService
    {
        ChartEntry SetChartEntry(string label, int value, string hexaColor);
    }
}
