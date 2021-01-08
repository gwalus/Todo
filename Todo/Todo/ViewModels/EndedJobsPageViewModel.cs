using GalaSoft.MvvmLight.Command;
using Prism.AppModel;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Services;

namespace Todo.ViewModels
{
    public class EndedJobsPageViewModel : ViewModelBase, IPageLifecycleAware
    {
        #region Fields
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        private int _jobsCount;
        private int _pageSize = 10;
        #endregion

        #region Properties
        private List<JobEndedDto> _endedJobs;

        public List<JobEndedDto> EndedJobs
        {
            get { return _endedJobs; }
            set 
            {
                _endedJobs = value;
                RaisePropertyChanged(nameof(EndedJobs));
            }
        }

        private int _pageNumber = 1;

        public int PageNumber
        {
            get { return _pageNumber; }
            set 
            {
                _pageNumber = value;
                RaisePropertyChanged(nameof(PageNumber));
            }
        }

        #endregion

        #region Commands
        private RelayCommand<int> _goToNextPageCommand;

        public RelayCommand<int> GoToNextPageCommand => _goToNextPageCommand ??= new RelayCommand<int>(GoToNextPage, GoToNextPageCanExecute);

        private RelayCommand<int> _goToPrevPageCommand;

        public RelayCommand<int> GoToPrevPageCommand => _goToPrevPageCommand ??= new RelayCommand<int>(GoToPrevPage, GoToPrevPageCanExecute);
        #endregion

        #region Constructors
        public EndedJobsPageViewModel(IDataService dataService, INavigationService navigationService) : base(navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;

            Task.Run(() => GetCountJobs()).Wait();
        }

        private async void GetCountJobs()
        {
            var jobs = await _dataService.GetEndedJobs();
            _jobsCount = jobs.Count;
        }
        #endregion

        #region Methods
        private async Task GetEndedJobs(int pageNumber, int jobsCount = 10)
        {
            var endedJobs = await _dataService.GetEndedJobs();

            var pagedEndedJobs = endedJobs
                .Skip((pageNumber - 1) * jobsCount)
                .Take(jobsCount)
                .ToList();

            EndedJobs = new List<JobEndedDto>();

            foreach (var endedJob in pagedEndedJobs)
            {
                var jobsEndedDto = new JobEndedDto()
                {
                    Description = endedJob.Description,
                    Added = endedJob.Added,
                    Ended = endedJob.Ended,
                    CompletionTime = (DateTime.Parse(endedJob.Ended) - DateTime.Parse(endedJob.Added)).ToString()
                };

                EndedJobs.Add(jobsEndedDto);
            }
        }

        private void GoToNextPage(int parameter)
        {
            ++PageNumber;
            Task.Run(() => GetEndedJobs(PageNumber)).Wait();
        }

        private bool GoToNextPageCanExecute(int pageNumber)
        {            
            if (_jobsCount > pageNumber * _pageSize) return true;
                return false;
        }

        private void GoToPrevPage(int parameter)
        {
            --PageNumber;
            Task.Run(() => GetEndedJobs(PageNumber)).Wait();
        }

        private bool GoToPrevPageCanExecute(int pageNumber)
        {
            if (pageNumber > 1) return true;
                return false;
        }

        public void OnAppearing()
        {            
            Task.Run(() => GetEndedJobs(PageNumber)).Wait();
        }

        public void OnDisappearing()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
