using Microcharts;
using Prism.AppModel;
using Prism.Navigation;
using SkiaSharp;
using System.Linq;
using System.Threading.Tasks;
using Todo.Services;

namespace Todo.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase, IPageLifecycleAware
    {
        #region Fields
        private readonly IDataService _dataService;
        #endregion

        #region Properties
        private DonutChart _chart;

        public DonutChart Chart
        {
            get { return _chart; }
            set { _chart = value; RaisePropertyChanged(nameof(Chart)); }
        }

        #endregion

        #region Constructors
        public ProfilePageViewModel(IDataService dataService, INavigationService navigationService) : base(navigationService)
        {
            _dataService = dataService;
        }
        #endregion

        private async Task GetStatistics()
        {
            var jobs = await _dataService.GetJobs();

            int active = jobs.Where(x => x.IsEnded == false).Count();
            int ended = jobs.Where(x => x.IsEnded).Count();

            var entries = new[]
            {
                SetChartEntry(nameof(active), active, "#77d065"),
                SetChartEntry(nameof(ended), ended, "#2c3e50"),
                SetChartEntry("deleted", 4, "#b455b6")
                // OPRACOWAĆ
            };

            var chart = new DonutChart 
            {
                Entries = entries, 
                LabelTextSize = 30 
            };

            Chart = chart;
        }

        private ChartEntry SetChartEntry(string label, int value, string hexaColor)
        {
            return new ChartEntry(value)
            {
                Label = label,
                ValueLabel = value.ToString(),
                ValueLabelColor = SKColor.Parse(hexaColor),
                Color = SKColor.Parse(hexaColor)
            };
        }

        public void OnAppearing()
        {
            Task.Run(() => GetStatistics()).Wait();
        }

        public void OnDisappearing()
        {
            //throw new System.NotImplementedException();
        }
    }
}
