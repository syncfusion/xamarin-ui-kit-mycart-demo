using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyCart.Models.Onboarding;
using MyCart.ViewModels.Login;
using MyCart.Core.Helper;
using MyCart.Core.Services;

namespace MyCart.ViewModels.Onboarding
{
    /// <summary>
    /// ViewModel for on-boarding gradient page.
    /// </summary>
    public class OnBoardingGradientViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Boarding> boardings;

        private string nextButtonText = "NEXT";

        private bool isSkipButtonVisible = true;

        private int selectedIndex;

        INavigationService navigationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="OnBoardingGradientViewModel" /> class.
        /// </summary>
        public OnBoardingGradientViewModel(INavigationService navigation)
        {
            navigationService = navigation;

            this.Boardings = new ObservableCollection<Boarding>
            {
                new Boarding
                {
                    ImagePath = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/ChooseGradient.svg",
                    Header = "CHOOSE",
                    Content = "Pick the item that is right for you"
                },
                new Boarding
                {
                    ImagePath = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/ConfirmGradient.svg",
                    Header = "ORDER CONFIRMED",
                    Content = "Your order is confirmed and will be on its way soon"
                },
                new Boarding
                {
                    ImagePath = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/DeliverGradient.svg",
                    Header = "DELIVERY",
                    Content = "Your item will arrive soon. Email us if you have any issues"
                }
            };

            this.SkipCommand = new Command(this.Skip);
            this.NextCommand = new Command(this.Next);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the Skip button is clicked.
        /// </summary>
        public ICommand SkipCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Done button is clicked.
        /// </summary>
        public ICommand NextCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the boardings collection.
        /// </summary>
        public ObservableCollection<Boarding> Boardings
        {
            get { return this.boardings; }
            set
            {
                this.boardings = value;
                this.OnPropertyChanged();
            }
        }

        public string NextButtonText
        {
            get
            {
                return this.nextButtonText;
            }
            set
            {
                SetProperty(ref nextButtonText, value);
            }
        }

        public bool IsSkipButtonVisible
        {
            get
            {
                return this.isSkipButtonVisible;
            }
            set
            {
                SetProperty(ref isSkipButtonVisible, value);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }
            set
            {
                SetProperty(ref selectedIndex, value);
                this.ValidateSelection();
            }
        }

        #endregion

        #region Methods

        private bool ValidateAndUpdateSelectedIndex()
        {
            if (this.selectedIndex >= this.Boardings.Count - 1) return true;

            this.SelectedIndex++;
            return false;
        }

        private void ValidateSelection()
        {
            if (this.selectedIndex < this.Boardings.Count - 1)
            {
                this.IsSkipButtonVisible = true;
                this.NextButtonText = "NEXT";
            }
            else
            {
                this.NextButtonText = "DONE";
                this.IsSkipButtonVisible = false;
            }
        }

        /// <summary>
        /// Invoked when the Skip button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void Skip(object obj)
        {
            MoveToNextPage();
        }

        /// <summary>
        /// Invoked when the Done button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void Next(object obj)
        {
            if (ValidateAndUpdateSelectedIndex())
            {
                //TODO: Need to popup the page.
                //Application.Current.MainPage.Navigation.PopAsync();                
                MoveToNextPage();
            }
        }

        private void MoveToNextPage()
        {
            navigationService.NavigateTo(typeof(LoginPageViewModel), string.Empty, string.Empty);
        }

        #endregion
    }
}