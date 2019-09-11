using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyCart.Models.Ecommerce;
using MyCart.Core.Helper;
using MyCart.Core.Services;
using System.Linq;
using MyCart.Core.Models.Ecommerce;
using MyCart.Core.Interface;

namespace MyCart.ViewModels.Ecommerce
{
    /// <summary>
    /// ViewModel for Category page.
    /// </summary>
    public class CategoryPageViewModel : ViewModelBase
    {
        #region Fields

        private Command itemSelectedCommand;

        public Command categorySelectedCommand;

        public Command expandingCommand;

        public Command notificationCommand;

        INavigationService navigationService;
        IDialogService dialogService;
        bool isMainCategory;

        #endregion

        #region Public properties

        private List<ICategory> categories;

        public List<ICategory> Categories
        {
            get { return categories; }
            set
            {
                if (SetProperty(ref categories, value))
                {
                    if(categories.Count == 0)
                    {
                        ValidateCategory();
                    }
                }
            }
        }

        private async void ValidateCategory()
        {
            await dialogService.Show("Nothing to show", "In this demo, only fashion products are loaded.", "Close");
            navigationService.NavigateBack();
        }

        #endregion

        public CategoryPageViewModel(INavigationService navigation, IDialogService dialogService, string selectedCategory)
        {
            navigationService = navigation;
            this.dialogService = dialogService;

            if (string.IsNullOrEmpty(selectedCategory))
            {
                //Main category
                Categories = DataStore.GetCategories().ToList<ICategory>();
                isMainCategory = true;
            }
            else
            {
                //Sub category
                isMainCategory = false;
                var category = DataStore.GetCategories().Where(
                    item => item.Name.ToLower() == selectedCategory.ToLower()).FirstOrDefault();

                if (category is MainCategory)
                {
                    Categories = (category as MainCategory).SubCategories.ToList<ICategory>();
                }
            }
        }

        #region Command

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand
        {
            get { return itemSelectedCommand ?? (itemSelectedCommand = new Command(this.ItemSelected)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the Category is selected.
        /// </summary>
        public Command CategorySelectedCommand
        {
            get { return categorySelectedCommand ?? (categorySelectedCommand = new Command(CategorySelected)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when expander is expanded.
        /// </summary>
        public Command ExpandingCommand
        {
            get { return expandingCommand ?? (expandingCommand = new Command(this.ExpanderClicked)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the Notification button is clicked.
        /// </summary>
        public Command NotificationCommand
        {
            get { return notificationCommand ?? (notificationCommand = new Command(this.NotificationClicked)); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private async void ItemSelected(object attachedObject)
        {
            Category category = attachedObject as Category;
            if (category == null && attachedObject is string)
            {
                navigationService.NavigateTo(typeof(CatalogPageViewModel), "selectedCategory", attachedObject as string);
                return;
            }
            else if (category != null)
            {
                if (isMainCategory)
                {
                    navigationService.NavigateTo(typeof(CategoryPageViewModel), "selectedCategory", category.Name);
                }
                else
                {
                    navigationService.NavigateTo(typeof(CatalogPageViewModel), "selectedCategory", category.Name);
                }
                return;
            }
            await dialogService.Show("Error", $"Selected any one category or selected category {attachedObject} not found", "Close");
        }

        /// <summary>
        /// Invoked when the Category is selected.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void CategorySelected(object attachedObject)
        {
            ItemSelected(attachedObject);
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
            //    await Task.Delay(100);
            //    listView.LayoutManager.ScrollToRowIndex(scrollIndex, Syncfusion.ListView.XForms.ScrollToPosition.End,
            //        true);
            //});
        }

        /// <summary>
        /// Invoked when the notification button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void NotificationClicked(object obj)
        {
            // Do something
        }

        #endregion
    }
}