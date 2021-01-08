using Prism;
using Prism.Ioc;
using Todo.Interfaces;
using Todo.Services;
using Todo.ViewModels;
using Todo.Views;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace Todo
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<JobsPage, JobsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<AddJobPage, AddJobPageViewModel>();
            containerRegistry.RegisterForNavigation<EndedJobsPage, EndedJobsPageViewModel>();

            containerRegistry.RegisterSingleton<IMicrochartsService, MicrochartsService>();
        }
    }
}
