using GalaSoft.MvvmLight.Command;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Todo.Models;
using Todo.Services;

namespace Todo.ViewModels
{
    public class JobsPageViewModel : BindableBase
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
            set { _jobs = value; }
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
        public RelayCommand<Job> MarkAsEndCommand => _markAsEndCommand ?? (_markAsEndCommand = new RelayCommand<Job>(MarkAsEnd));
        #endregion

        #region Constructor
        public JobsPageViewModel(IDataService dataService, IPageDialogService pageDialogService)
        {
            _dataService = dataService;
            _pageDialogService = pageDialogService;
            LoadJobs();
        }
        #endregion

        #region Methods
        async void LoadJobs()
        {
            var jobs = await _dataService.GetJobs();

            var orderedJobs = jobs
                .Where(job => job.IsEnded == false)
                .OrderByDescending(job => DateTime.Parse(job.Added))
                .ToList();

            if (Jobs != null)
            {
                Jobs.Clear();
                orderedJobs.ForEach(x => Jobs.Add(x));
            }
            else Jobs = new ObservableCollection<Job>(orderedJobs);
        }

        private async void MarkAsEnd(Job job)
        {
            var result = await _pageDialogService.DisplayAlertAsync("Information", $"Complete the task \"{job.Description}\"?", "Yes", "Not yet");

            if (result)
            {
                job.IsEnded = true;
                if (await _dataService.UpdateJob(job))
                    LoadJobs();
                else
                    await _pageDialogService.DisplayAlertAsync("Information", "Complete", "Yes");
            }
        }
        #endregion
    }
}