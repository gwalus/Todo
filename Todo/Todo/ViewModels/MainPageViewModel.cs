using Prism.Commands;
using Prism.Navigation;

namespace Todo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private DelegateCommand _goToAddJobPageCommand;
        private readonly INavigationService _navigationService;

        public DelegateCommand GoToAddJobPageCommand => _goToAddJobPageCommand ?? (_goToAddJobPageCommand = new DelegateCommand(GoToAddJobPage));

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }

        async void GoToAddJobPage()
        {
            await _navigationService.NavigateAsync("AddJobPage");    
        }
    }
}
