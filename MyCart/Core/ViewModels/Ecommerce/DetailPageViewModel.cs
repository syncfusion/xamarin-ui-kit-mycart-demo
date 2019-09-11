using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyCart.Models.Ecommerce;
using MyCart.Core.Helper;
using System.Linq;
using MyCart.Core.Services;

namespace MyCart.ViewModels.Ecommerce
{
    /// <summary>
    /// ViewModel for detail page.
    /// </summary>
    public class DetailPageViewModel : ViewModelBase
    {
        #region Fields

        private readonly double productRating;

        private Product productDetail;

        private ObservableCollection<Category> categories;

        private bool isFavourite;

        private bool isReviewVisible;

        private string cartItemCount = "2";

        IDialogService dialogService;

        INavigationService navigationService;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance for the <see cref="DetailPageViewModel" /> class.
        /// </summary>
        public DetailPageViewModel(string selectedProduct, IDialogService dialog, INavigationService navigation)
        {
            dialogService = dialog;
            navigationService = navigation;

            var selectedPoductDetail = ProductDetail = DataStore.GetProducts().Where(item => item.Id == int.Parse(selectedProduct)).FirstOrDefault();

            if (selectedPoductDetail.Reviews == null || selectedPoductDetail.Reviews.Count == 0)
                this.IsReviewVisible = true;
            else
            {
                foreach (var review in selectedPoductDetail.Reviews)
                {
                    this.productRating += review.Rating;
                }
            }

            if (this.productRating > 0)
                selectedPoductDetail.OverallRating = this.productRating / selectedPoductDetail.Reviews.Count;

            this.AddFavouriteCommand = new Command(this.AddFavouriteClicked);
            this.NotificationCommand = new Command(this.NotificationClicked);
            this.AddToCartCommand = new Command(this.AddToCartClicked);
            this.LoadMoreCommand = new Command(this.LoadMoreClicked);
            this.ShareCommand = new Command(this.ShareClicked);
            this.VariantCommand = new Command(VariantClicked);
            this.ItemSelectedCommand = new Command(this.ItemSelected);
            this.CardItemCommand = new Command(this.CartClicked);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the property that has been bound with SfRotator and labels, which display the product images and details.
        /// </summary>
        public Product ProductDetail
        {
            get
            {
                return this.productDetail;
            }

            set
            {
                SetProperty(ref productDetail, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the Favourite.
        /// </summary>
        public bool IsFavourite
        {
            get
            {
                return this.isFavourite;
            }
            set
            {
                SetProperty(ref isFavourite, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the empty message.
        /// </summary>
        public bool IsReviewVisible
        {
            get
            {
                if (productDetail.Reviews.Count == 0)
                    this.isReviewVisible = true;
                return this.isReviewVisible;
            }
            set
            {
                SetProperty(ref isReviewVisible, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the cart items count.
        /// </summary>
        public string CartItemCount
        {
            get
            {
                return this.cartItemCount;
            }
            set
            {
                SetProperty(ref cartItemCount, value);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that will be executed when the Favourite button is clicked.
        /// </summary>
        public Command AddFavouriteCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Notification button is clicked.
        /// </summary>
        public Command NotificationCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the AddToCart button is clicked.
        /// </summary>
        public Command AddToCartCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Show All button is clicked.
        /// </summary>
        public Command LoadMoreCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Share button is clicked.
        /// </summary>
        public Command ShareCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the size button is clicked.
        /// </summary>
        public Command VariantCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when cart button is clicked.
        /// </summary>
        public Command CardItemCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Favourite button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void AddFavouriteClicked(object obj)
        {
            if (obj is DetailPageViewModel model)
            {
                //DataStore.SelectedProduct.IsFavourite = !DataStore.SelectedProduct.IsFavourite;
            }
        }

        /// <summary>
        /// Invoked when the Notification button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void NotificationClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Cart button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void AddToCartClicked(object obj)
        {
            IsBusy = true;
            var result = await DataStore.AddItemAsync(ProductDetail.Id.ToString());

            if(result == true)
            {
                await dialogService.Show("Success", "Your item has been added to cart", "Go to Cart");
                navigationService.NavigateTo(typeof(CartPageViewModel), string.Empty, string.Empty);
                //TODO: Need to show the display alert
                //TODO: Done navigation to cart page

                //await Shell.Current.DisplayAlert("Success", $"The item {DataStore.SelectedProduct.Name} has been added to cart", "Go to Cart");
                //await Shell.Current.GoToAsync("cart", true);
            }
        }

        /// <summary>
        /// Invoked when Load more button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void LoadMoreClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Share button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void ShareClicked(object obj)
        {
            // Do something.
        }

        /// <summary>
        /// Invoked when the variant button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void VariantClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private void ItemSelected(object attachedObject)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when cart icon button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void CartClicked(object obj)
        {
            navigationService.NavigateTo(typeof(CartPageViewModel), string.Empty, string.Empty);
        }

        #endregion
    }
}
