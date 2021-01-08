using GalaSoft.MvvmLight.Command;
using Microcharts;
using Prism.AppModel;
using Prism.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;
using Todo.Interfaces;
using Todo.Services;

namespace Todo.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase, IPageLifecycleAware
    {
        #region Fields
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private readonly IMicrochartsService _microchartsService;
        #endregion

        #region Properties
        private DonutChart _chart;

        public DonutChart Chart
        {
            get { return _chart; }
            set 
            {
                _chart = value; 
                RaisePropertyChanged(nameof(Chart));
            }
        }
        #endregion

        #region Commands
        private RelayCommand _goToEndedJobsCommand;

        public RelayCommand GoToEndedJobsCommand => _goToEndedJobsCommand ??= new RelayCommand(GoToEndedJobsPage);
        #endregion

        #region Constructors
        public ProfilePageViewModel(IDataService dataService, INavigationService navigationService, IMicrochartsService microchartsService) : base(navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _microchartsService = microchartsService;
        }
        #endregion

        #region Methods
        private async Task GetStatistics()
        {
            var jobs = await _dataService.GetJobs();

            int active = jobs.Where(x => x.IsEnded == false && x.IsDeleted == false).Count();
            int ended = jobs.Where(x => x.IsEnded).Count();
            int deleted = jobs.Where(x => x.IsDeleted).Count();

            var entries = new[]
            {
                _microchartsService.SetChartEntry(nameof(active), active, "#77d065"),
                _microchartsService.SetChartEntry(nameof(ended), ended, "#2c3e50"),
                _microchartsService.SetChartEntry(nameof(deleted), deleted, "#b455b6"),
            };

            var chart = new DonutChart 
            {
                Entries = entries, 
                LabelTextSize = 30 
            };

            Chart = chart;
        }

        private void GoToEndedJobsPage()
        {
            _navigationService.NavigateAsync("EndedJobsPage");
        }

        public void OnAppearing()
        {
            Task.Run(() => GetStatistics()).Wait();
        }

        public void OnDisappearing()
        {
            //throw new System.NotImplementedException();
        }
        #endregion
    }
}
