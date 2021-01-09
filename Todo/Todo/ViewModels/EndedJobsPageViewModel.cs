using GalaSoft.MvvmLight.Command;
using Prism.AppModel;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Dtos;
using Todo.Helpers;
using Todo.Models;
using Todo.Services;

namespace Todo.ViewModels
{
    public class EndedJobsPageViewModel : ViewModelBase, IPageLifecycleAware
    {
        #region Fields
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        private List<JobEndedDto> _endedJobs;

        private RelayCommand<int> _goToNextPageCommand;
        private RelayCommand<int> _goToPrevPageCommand;

        private int _jobsCount;
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private string _searchJob;
        private int _searchedJobsCount;
        private bool _isInSearchMode = false;
        #endregion

        #region Properties
        public List<JobEndedDto> EndedJobs
        {
            get { return _endedJobs; }
            set 
            {
                _endedJobs = value;
                RaisePropertyChanged(nameof(EndedJobs));
            }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set 
            {
                _pageNumber = value;
                RaisePropertyChanged(nameof(PageNumber));
            }
        }

        public string SearchJob
        {
            get { return _searchJob; }
            set 
            {
                _searchJob = value;
                RaisePropertyChanged(nameof(SearchJob));

                if (string.IsNullOrEmpty(SearchJob))
                {
                    _isInSearchMode = false;
                    Task.Run(() => GetEndedJobs(PageNumber)).Wait();
                }
                else
                {
                    _isInSearchMode = true;
                    Task.Run(() => GetEndedJobsSearch(PageNumber, _searchJob)).Wait();
                }
            }
        }
        #endregion

        #region Commands
        public RelayCommand<int> GoToNextPageCommand => _goToNextPageCommand ??= new RelayCommand<int>(GoToNextPage, GoToNextPageCanExecute);
        public RelayCommand<int> GoToPrevPageCommand => _goToPrevPageCommand ??= new RelayCommand<int>(GoToPrevPage, GoToPrevPageCanExecute);
        #endregion

        #region Constructors
        public EndedJobsPageViewModel(IDataService dataService, INavigationService navigationService) : base(navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;

            Task.Run(() => GetCountJobs()).Wait();
        }
        #endregion

        #region Methods
        private async void GetCountJobs()
        {
            var jobs = await _dataService.GetEndedJobs();
            _jobsCount = jobs.Count;
        }

        private async Task GetEndedJobs(int currentPage)
        {
            var endedJobs = await _dataService.GetEndedJobs();

            var pagedList = new PagedList<Job>().CreatePagedList(endedJobs, currentPage);

            CreateEndedJobList(pagedList);
        }

        private async Task GetEndedJobsSearch(int currentPage, string description)
        {
            var endedJobs = await _dataService.GetEndedJobs();

            var pagedList = new PagedList<Job>();

            _searchedJobsCount = pagedList.GetJobsCountByDescription(endedJobs, description);
            var searchedPagedList = pagedList.CreatePagedListByDescription(endedJobs, currentPage, description);

            CreateEndedJobList(searchedPagedList);
        }

        private void CreateEndedJobList(List<Job> pagedList)
        {
            EndedJobs = new List<JobEndedDto>();

            foreach (var endedJob in pagedList)
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
            if(_isInSearchMode)
            {
                ++PageNumber;
                Task.Run(() => GetEndedJobsSearch(PageNumber, SearchJob)).Wait();
            }
            else
            {
                ++PageNumber;
                Task.Run(() => GetEndedJobs(PageNumber)).Wait();
            }
        }

        private bool GoToNextPageCanExecute(int pageNumber)
        {            
            if(_isInSearchMode)
            {
                if (_searchedJobsCount > pageNumber * _pageSize) return true;
                return false;
            }

            else if (_jobsCount > pageNumber * _pageSize) return true;
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
