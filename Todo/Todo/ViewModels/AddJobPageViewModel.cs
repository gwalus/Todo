using GalaSoft.MvvmLight.Command;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using Todo.Models;
using Todo.Services;

namespace Todo.ViewModels
{
    public class AddJobPageViewModel : BindableBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        private RelayCommand<string> _addJobCommand;
        public RelayCommand<string> AddJobCommand
        {
            get
            {
                return _addJobCommand ?? (_addJobCommand = new RelayCommand<string>(
                    parameter =>
                    {
                        var newJob = new Job()
                        {
                            Description = parameter,
                            Added = DateTime.Now.ToString(),
                            IsEnded = false
                        };

                        AddJob(newJob);
                    },
                    parameter =>
                    {
                        if (string.IsNullOrEmpty(parameter)) return false;
                        if (parameter.Length < 5) return false;
                        return true;
                    }
                    ));
            }
        }

        async void AddJob(Job newJob)
        {
            if (await _dataService.AddJob(newJob))
            {
                await _pageDialogService.DisplayAlertAsync("Success", "Added new job successfully", "Ok");
                await _navigationService.NavigateAsync("/NavigationPage/MainPage");
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Failed", "Something went wront", "Ok");
                Description = string.Empty;
            }
        }

        public AddJobPageViewModel(IDataService dataService, INavigationService navigationService, IPageDialogService pageDialogService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
        }
    }
}
