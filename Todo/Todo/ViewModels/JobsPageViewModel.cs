using GalaSoft.MvvmLight.Command;
using Prism.AppModel;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Todo.Models;
using Todo.Services;

namespace Todo.ViewModels
{
    public class JobsPageViewModel : ViewModelBase, IPageLifecycleAware
    {
        #region Fields
        private ObservableCollection<Job> _jobs;
        private readonly IDataService _dataService;
        private readonly IPageDialogService _pageDialogService;
        #endregion

        #region Properties
        public ObservableCollection<Job> Jobs
        {
            get { return _jobs; }
            set 
            {
                _jobs = value; 
                RaisePropertyChanged(nameof(Jobs)); 
            }
        }

        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion

        #region Commands
        private RelayCommand<Job> _markAsEndCommand;
        public RelayCommand<Job> MarkAsEndCommand => _markAsEndCommand ??= new RelayCommand<Job>(UpdateJob);
        #endregion

        #region Constructor
        public JobsPageViewModel(IDataService dataService, IPageDialogService pageDialogService, INavigationService navigationService) : base(navigationService)
        {
            _dataService = dataService;
            _pageDialogService = pageDialogService;
        }
        #endregion

        #region Methods
        private async Task LoadJobs()
        {
            var jobs = await _dataService.GetJobs();

            if(jobs != null)
            {
                var orderedJobs = jobs
                .Where(job => job.IsEnded == false && job.IsDeleted == false)
                .OrderByDescending(job => DateTime.Parse(job.Added))
                .ToList();

                Jobs = new ObservableCollection<Job>(orderedJobs);
            }                
        }

        private async void UpdateJob(Job job)
        {
            var resultComplete = await _pageDialogService.DisplayAlertAsync("Information", $"Complete the task \"{job.Description}\"?", "Complete", "Not yet");

            if (resultComplete)
            {
                job.IsEnded = true;
                job.Ended = DateTime.Now.ToString();
                if (await _dataService.UpdateJob(job))
                    await LoadJobs();
            }
            else
            {
                var resultDelete = await _pageDialogService.DisplayAlertAsync("Information", $"Delete the task \"{job.Description}\"?", "Delete", "No");
                if (resultDelete)
                {
                    job.IsDeleted = true;
                    if (await _dataService.UpdateJob(job))
                        await LoadJobs();
                }
            }

        }

        public void OnAppearing()
        {
            Task.Run(() => LoadJobs()).Wait();
        }

        public void OnDisappearing()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}