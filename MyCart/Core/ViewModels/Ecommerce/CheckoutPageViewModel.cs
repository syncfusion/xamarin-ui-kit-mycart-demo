using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyCart.Models.Ecommerce;
using System;
using System.Threading.Tasks;
using MyCart.Core.Helper;
using MyCart.Core.Services;

namespace MyCart.ViewModels.Ecommerce
{
    /// <summary>
    /// ViewModel for Checkout page.
    /// </summary>
    public class CheckoutPageViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Customer> deliveryAddress;

        private ObservableCollection<Payment> paymentModes;

        private ObservableCollection<Product> cartDetails;

        private double totalPrice;

        private double discountPrice;

        private double discountPercent;

        private IDialogService dialogService;
        private INavigationService navigationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="CheckoutPageViewModel" /> class.
        /// </summary>
        public CheckoutPageViewModel(IDialogService dialog, INavigationService navigation)
        {
            dialogService = dialog;
            navigationService = navigation;

            this.DeliveryAddress = new ObservableCollection<Customer>
            {
                new Customer
                {
                    CustomerId = 1, CustomerName = "John Doe", AddressType = "Home", Address = "410 Terry Ave N, USA",
                    MobileNumber = "+1-202-555-0101"
                },
                new Customer
                {
                    CustomerId = 1, CustomerName = "John Doe", AddressType = "Office",
                    Address = "388 Fort Worth, Texas, United States", MobileNumber = "+1-356-636-8572"
                },
            };

            this.PaymentModes = new ObservableCollection<Payment>
            {
                new Payment
                {
                    PaymentMode = "Goldman Sachs Bank Credit Card", CardNumber = "48** **** **** 9876",
                    CardTypeIcon = "Card.png"
                },
                new Payment {PaymentMode = "Wells Fargo Bank Credit Card"},
                new Payment {PaymentMode = "Debit / Credit Card"},
                new Payment {PaymentMode = "NetBanking"},
                new Payment {PaymentMode = "Cash on Delivery"},
                new Payment {PaymentMode = "Wallet"},
            };

            FetchCartList();

            this.EditCommand = new Command(this.EditClicked);
            this.AddAddressCommand = new Command(this.AddAddressClicked);
            this.PlaceOrderCommand = new Command(this.PlaceOrderClicked);
            this.PaymentOptionCommand = new Command(PaymentOptionClicked);
            this.ApplyCouponCommand = new Command(this.ApplyCouponClicked);
        }


        #endregion

        private async void FetchCartList()
        {
            CartDetails = await GetCartList();

            double percent = 0;
            foreach (var item in this.CartDetails)
            {
                this.TotalPrice += (item.ActualPrice * item.TotalQuantity);
                this.DiscountPrice += (item.DiscountPrice * item.TotalQuantity);
                percent += item.DiscountPercent;
            }

            this.DiscountPercent = percent > 0 ? percent / this.CartDetails.Count : 0;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets the property that has been bound with SfListView, which displays the delivery address.
        /// </summary>
        public ObservableCollection<Customer> DeliveryAddress
        {
            get { return this.deliveryAddress; }

            set
            {
                SetProperty(ref deliveryAddress, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with SfListView, which displays the payment modes.
        /// </summary>
        public ObservableCollection<Payment> PaymentModes
        {
            get { return this.paymentModes; }

            set
            {
                SetProperty(ref paymentModes, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the cart details.
        /// </summary>
        public ObservableCollection<Product> CartDetails
        {
            get { return this.cartDetails; }

            set
            {
                SetProperty(ref cartDetails, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays total price.
        /// </summary>
        public double TotalPrice
        {
            get { return this.totalPrice; }

            set
            {
                SetProperty(ref totalPrice, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays total discount price.
        /// </summary>
        public double DiscountPrice
        {
            get { return this.discountPrice; }

            set
            {
                SetProperty(ref discountPrice, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays discount.
        /// </summary>
        public double DiscountPercent
        {
            get { return this.discountPercent; }

            set
            {
                SetProperty(ref discountPercent, value);
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that will be executed when the Edit button is clicked.
        /// </summary>
        public Command EditCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Add new address button is clicked.
        /// </summary>
        public Command AddAddressCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Edit button is clicked.
        /// </summary>
        public Command PlaceOrderCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the payment option button is clicked.
        /// </summary>
        public Command PaymentOptionCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the apply coupon button is clicked.
        /// </summary>
        public Command ApplyCouponCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Edit button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void EditClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Add address button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void AddAddressClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Place order button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void PlaceOrderClicked(object obj)
        {
            //TODO: Need to show the display alert popup
            await dialogService.Show("Success", "Your order has been placed! :)", "Shop More");

            await DataStore.ClearProducts();
            navigationService.NavigateTo(typeof(CategoryPageViewModel), string.Empty, string.Empty, true);
        }

        /// <summary>
        /// Invoked when the Payment option is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void PaymentOptionClicked(object obj)
        {
            //TODO: Need to implement for changing the height of the grid row.
            //if (obj is RowDefinition rowDefinition && rowDefinition.Height.Value == 0)
            //{
            //    rowDefinition.Height = GridLength.Auto;
            //}
        }

        /// <summary>
        /// Invoked when the Apply coupon button is selected.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void ApplyCouponClicked(object obj)
        {
            // Do something
        }

        #endregion
    }
}