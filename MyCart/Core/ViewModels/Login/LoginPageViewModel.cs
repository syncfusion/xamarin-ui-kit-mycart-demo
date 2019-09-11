using MyCart.ViewModels.Ecommerce;
using MyCart.Core.Helper;
using MyCart.Core.Services;
using MyCart.Core.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCart.ViewModels.Login
{

    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    public class LoginPageViewModel : LoginViewModel
    {
        #region Fields

        private string password;

        IDialogService dialogService;

        INavigationService navigationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        public LoginPageViewModel(IDialogService dialog, INavigationService navigation)
        {
            dialogService = dialog;
            navigationService = navigation;

            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
            this.ForgotPasswordCommand = new Command(this.ForgotPasswordClicked);
            this.SocialMediaLoginCommand = new Command(this.SocialLoggedIn);
        }

        #endregion

        #region property

        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                SetProperty(ref password, value);
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Command ForgotPasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the social media login button is clicked.
        /// </summary>
        public Command SocialMediaLoginCommand { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
            navigationService.NavigateTo(typeof(CategoryPageViewModel), "selectedCategory", string.Empty, true);

            return;
            if (string.IsNullOrEmpty(Email))
            {
                IsInvalidEmail = true;
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                return;
            }

            var result = await DataStore.ValidateUser(Email, Password);
            if (result)
            {
                //Preferences.Set("email", Email);
                navigationService.NavigateTo(typeof(CategoryPageViewModel), string.Empty, string.Empty, true);
                //Application.Current.MainPage = new AppShell();
            }
        }        

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            navigationService.NavigateTo(typeof(SignUpPageViewModel), string.Empty, string.Empty, false);
            //await Application.Current.MainPage.Navigation.PushAsync(new SimpleSignUpPage());
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            //TODO: Find an alternative way to change this color.
            //var label = obj as Label;
            //label.BackgroundColor = Color.FromHex("#70FFFFFF");
            //await Task.Delay(100);
            //label.BackgroundColor = Color.Transparent;

            //await Application.Current.MainPage.Navigation.PushAsync(new SimpleForgotPasswordPage());
            navigationService.NavigateTo(typeof(ForgotPasswordViewModel), string.Empty, string.Empty, false);
        }

        /// <summary>
        /// Invoked when social media login button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SocialLoggedIn(object obj)
        {
            // Do something
        }

        #endregion
    }
}