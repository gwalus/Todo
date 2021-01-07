using Prism.AppModel;
using Prism.Navigation;
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
        private string _activeJobs;

        public string ActiveJobs
        {
            get { return _activeJobs; }
            set
            { 
                _activeJobs = value;
                RaisePropertyChanged(nameof(ActiveJobs));
            }
        }

        private string _endedJobs;

        public string EndedJobs
        {
            get { return _endedJobs; }
            set
            {
                _endedJobs = value;
                RaisePropertyChanged(nameof(EndedJobs));
            }
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

            ActiveJobs = jobs.Where(x => x.IsEnded == false).Count().ToString();
            EndedJobs = jobs.Where(x => x.IsEnded).Count().ToString();
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
