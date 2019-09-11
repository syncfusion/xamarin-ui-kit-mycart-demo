using MyCart.ViewModels.About;
using MyCart.ViewModels.Ecommerce;
using MyCart.ViewModels.Login;
using MyCart.Core;
using MyCart.Core.Services;
using MyCart.Services;
using MyCart.Views.Ecommerce;
using MyCart.Views.ErrorAndEmpty;
using MyCart.Views.Login;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyCart
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public ICommand LogoutCommand => new Command(GoToLogout);
        NavigationService navigationService = (NavigationService)TypeLocator.Instance.Resolve(typeof(INavigationService));

        public AppShell()
        {
            InitializeComponent();

            FlyoutItem item = new FlyoutItem() { Title = "Home" };
            ShellContent content = new ShellContent();

            //Home Page
            content.Title = "Home";
            content.Content = navigationService.GetPageWithBindingContext(typeof(CategoryPageViewModel), "selectedCategory", string.Empty);
            item.Items.Add(content);
            this.Items.Add(item);

            //Aboutus Page
            item = new FlyoutItem() { Title = "About" };
            content = new ShellContent();
            content.Title = "About";
            content.Content = navigationService.GetPageWithBindingContext(typeof(AboutUsViewModel), string.Empty, string.Empty);
            item.Items.Add(content);
            this.Items.Add(item);

            //Logout Menu
            MenuItem logout = new MenuItem() { Text = "Logout", Command = new Command(GoToLogout) };
            this.Items.Add(logout);

            RegisterRoutes();
        }

        internal static Page Init()
        {
            ListenNetworkChanges();
            IntializeBuildContainer();

            var navigationService = TypeLocator.Instance.Resolve(typeof(INavigationService)) as INavigationService;
            var startup = TypeLocator.Instance.Resolve(typeof(Startup)) as Startup;

            var mainPage = startup.GetMainPage();

            return ((NavigationService)navigationService).GetPageWithBindingContext(mainPage, string.Empty, string.Empty);
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("login", typeof(SimpleLoginPage));
            Routing.RegisterRoute("forgotpassword", typeof(SimpleForgotPasswordPage));
            Routing.RegisterRoute("resetpassword", typeof(SimpleResetPasswordPage));
            Routing.RegisterRoute("signup", typeof(SimpleSignUpPage));

            Routing.RegisterRoute("category", typeof(CategoryTilePage));
            Routing.RegisterRoute("catalog", typeof(CatalogListPage));
            Routing.RegisterRoute("detail", typeof(DetailPage));
            Routing.RegisterRoute("cart", typeof(CartPage));
            Routing.RegisterRoute("checkout", typeof(CheckoutPage));

            Routing.RegisterRoute("empty", typeof(EmptyCartPage));
        }

        void GoToLogout()
        {
            Preferences.Set("email", "");
            navigationService.NavigateTo(typeof(LoginPageViewModel), string.Empty, string.Empty, true);
        }

        private static void IntializeBuildContainer()
        {
            TypeLocator.Instance.Build();
        }

        private static void ListenNetworkChanges()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private static void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckInternet();
        }

        static bool onErrorPage;
        private static void CheckInternet()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                onErrorPage = true;
                Application.Current.MainPage.Navigation.PushAsync(new NoInternetConnectionPage());
            }
            else if (onErrorPage)
            {
                Application.Current.MainPage.Navigation.PopAsync();
                onErrorPage = false;
            }
        }
    }
}