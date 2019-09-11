using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using MyCart.Models.Ecommerce;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using MyCart.Core.Helper;
using MyCart.Core.Services;
using System.Threading.Tasks;

namespace MyCart.ViewModels.Ecommerce
{
    /// <summary>
    /// ViewModel for catalog page.
    /// </summary>
    // TODO: Map query property
    //[QueryProperty("selectedcategory", "SelectedCategory")]
    public class CatalogPageViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Category> filterOptions;

        private ObservableCollection<string> sortOptions;

        private Command addFavouriteCommand;

        private Command itemSelectedCommand;

        private Command sortCommand;

        private Command filterCommand;

        private Command addToCartCommand;

        private Command expandingCommand;

        public Command cardItemCommand;

        private string cartItemCount;

        INavigationService navigationService;

        IDialogService dialogService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="CatalogPageViewModel" /> class.
        /// </summary>
        public CatalogPageViewModel(INavigationService navigation, IDialogService dialogService, string selectedCategory)
        {
            navigationService = navigation;
            this.dialogService = dialogService;
            
            SelectedCategory = selectedCategory;

            //this.FilterOptions = new ObservableCollection<Category>
            //{
            //    new Category
            //    {
            //        CategoryName = "Gender",
            //        SubCategories = new List<string>
            //        {
            //            "Men",
            //            "Women"
            //        }
            //    },
            //    new Category
            //    {
            //        CategoryName = "Brand",
            //        SubCategories = new List<string>
            //        {
            //            "Brand A",
            //            "Brand B"
            //        }
            //    },
            //    new Category
            //    {
            //        CategoryName = "Categories",
            //        SubCategories = new List<string>
            //        {
            //            "Category A",
            //            "Category B"
            //        }
            //    },
            //    new Category
            //    {
            //        CategoryName = "Color",
            //        SubCategories = new List<string>
            //        {
            //            "Maroon",
            //            "Pink"
            //        }
            //    },
            //    new Category
            //    {
            //        CategoryName = "Price",
            //        SubCategories = new List<string>
            //        {
            //            "Above 3000",
            //            "1000 to 3000",
            //            "Below 1000"
            //        }
            //    },
            //    new Category
            //    {
            //        CategoryName = "Size",
            //        SubCategories = new List<string>
            //        {
            //            "S", "M", "L", "XL", "XXL"
            //        }
            //    },
            //    new Category
            //    {
            //        CategoryName = "Patterns",
            //        SubCategories = new List<string>
            //        {
            //            "Pattern 1", "Pattern 2"
            //        }
            //    },
            //    new Category
            //    {
            //        CategoryName = "Offers",
            //        SubCategories = new List<string>
            //        {
            //            "Buy 1 Get 1", "Buy 1 Get 2"
            //        }
            //    },
            //    new Category
            //    {
            //        CategoryName = "Coupons",
            //        SubCategories = new List<string>
            //        {
            //            "Coupon 1", "Coupon 2"
            //        }
            //    },
            //};

            //this.SortOptions = new ObservableCollection<string>
            //{
            //    "New Arrivals",
            //    "Price - high to low",
            //    "Price - Low to High",
            //    "Popularity",
            //    "Discount"
            //};
        }

        #endregion

        #region Public properties

        public List<Product> Products { get; set; }

        private string selectedItem = "Watches";

        public string SelectedCategory
        {
            get { return selectedItem; }
            set
            {
                ValidateCategory(value);
            }
        }

        private async Task ValidateCategory(string value)
        {
            if (SetProperty(ref selectedItem, value))
            {
                Products = AllProducts.Where(item => item.Category.ToLower() == selectedItem.ToLower()).ToList();
                if (Products.Count == 0)
                {
                    await dialogService.Show("Nothing to show", "In this demo, only watch products are loaded.", "Close");
                    navigationService.NavigateBack();

                    return;
                }
                var items = await GetCartList();
                CartItemCount = items.Count.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the filter options.
        /// </summary>
        public ObservableCollection<Category> FilterOptions
        {
            get
            {
                return this.filterOptions;
            }

            set
            {
                SetProperty(ref filterOptions, value);
            }
        }

        /// <summary>
        /// Gets or sets the property has been bound with a list view, which displays the sort details.
        /// </summary>
        public ObservableCollection<string> SortOptions
        {
            get
            {
                return this.sortOptions;
            }

            set
            {
                SetProperty(ref sortOptions, value);
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

        #region Command

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand
        {
            get { return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the sort button is clicked.
        /// </summary>
        public Command SortCommand
        {
            get { return this.sortCommand ?? (this.sortCommand = new Command(this.SortClicked)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the filter button is clicked.
        /// </summary>
        public Command FilterCommand
        {
            get { return this.filterCommand ?? (this.filterCommand = new Command(this.FilterClicked)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the Favourite button is clicked.
        /// </summary>
        public Command AddFavouriteCommand
        {
            get
            {
                return this.addFavouriteCommand ?? (this.addFavouriteCommand = new Command(this.AddFavouriteClicked));
            }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the AddToCart button is clicked.
        /// </summary>
        public Command AddToCartCommand
        {
            get { return this.addToCartCommand ?? (this.addToCartCommand = new Command(this.AddToCartClicked)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when expander is expanded.
        /// </summary>
        public Command ExpandingCommand
        {
            get { return this.expandingCommand ?? (this.expandingCommand = new Command(this.ExpanderClicked)); }
        }

        /// <summary>
        /// Gets or sets the command will be executed when the cart icon button has been clicked.
        /// </summary>
        public Command CardItemCommand
        {
            get { return this.cardItemCommand ?? (this.cardItemCommand = new Command(this.CartClicked)); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private async void ItemSelected(object attachedObject)
        {
            Product product = attachedObject as Product;

            if(product == null && attachedObject is string)
            {
                navigationService.NavigateTo(typeof(DetailPageViewModel), "selectedProduct", attachedObject as string);
                return;
            }

            navigationService.NavigateTo(typeof(DetailPageViewModel), "selectedProduct", product.Id.ToString());
        }

        /// <summary>
        /// Invoked when the items are sorted.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private void SortClicked(object attachedObject)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the filter button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void FilterClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the favourite button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void AddFavouriteClicked(object obj)
        {
            if (obj is Product product)
                product.IsFavourite = !product.IsFavourite;
        }

        /// <summary>
        /// Invoked when the cart button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void AddToCartClicked(object obj)
        {
            navigationService.NavigateTo(typeof(CartPageViewModel), string.Empty, string.Empty);
        }

        /// <summary>
        /// Invoked when the expander is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void ExpanderClicked(object obj)
        {
            //var objects = obj as List<object>;
            //var category = objects[0] as Category;
            //var listView = objects[1] as SfListView;

            //if (listView == null)
            //{
            //    return;
            //}

            //var itemIndex = listView.DataSource.DisplayItems.IndexOf(category);
            //var scrollIndex = itemIndex + category.SubCategories.Count;
            ////Expand and bring the item in the view.
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    await System.Threading.Tasks.Task.Delay(100);
            //    listView.LayoutManager.ScrollToRowIndex(scrollIndex, Syncfusion.ListView.XForms.ScrollToPosition.End, true);
            //});
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