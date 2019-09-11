using MyCart.Views.AboutUs;
using MyCart.Views.Ecommerce;
using MyCart.Views.Login;
using MyCart.Views.Onboarding;
using MyCart.Core.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using MyCart.ViewModels.Ecommerce;
using MyCart.ViewModels.Login;
using MyCart.ViewModels.Onboarding;
using MyCart.ViewModels.About;
using MyCart.ViewModels;

namespace MyCart.Services
{
    public class NavigationService : INavigationService
    {
        protected readonly Dictionary<Type, Type> MappingPageAndViewModel;

        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public NavigationService()
        {
            MappingPageAndViewModel = new Dictionary<Type, Type>();

            SetPageViewModelMappings();
        }

        public async void NavigateBack()
        {
            await CurrentApplication.MainPage.Navigation.PopAsync(true);
        }

        public async void NavigateTo(Type type, string parameterName, string parameterValue, bool replaceView = false)
        {
            if(type == typeof(CategoryPageViewModel) && string.IsNullOrEmpty(parameterValue))
            {
                CurrentApplication.MainPage = new AppShell();
                return;
            }
            if (!replaceView)
            {
                await CurrentApplication.MainPage.Navigation.PushAsync(GetPageWithBindingContext(type, parameterName, parameterValue), true);
            }
            else
            {
                CurrentApplication.MainPage = new NavigationPage(GetPageWithBindingContext(type, parameterName, parameterValue));
            }
        }

        private void SetPageViewModelMappings()
        {
            MappingPageAndViewModel.Add(typeof(LoginPageViewModel), typeof(SimpleLoginPage));
            MappingPageAndViewModel.Add(typeof(SignUpPageViewModel), typeof(SimpleSignUpPage));
            MappingPageAndViewModel.Add(typeof(ForgotPasswordViewModel), typeof(SimpleForgotPasswordPage));
            MappingPageAndViewModel.Add(typeof(ResetPasswordViewModel), typeof(SimpleResetPasswordPage));

            MappingPageAndViewModel.Add(typeof(OnBoardingGradientViewModel), typeof(OnBoardingGradientPage));
            MappingPageAndViewModel.Add(typeof(CategoryPageViewModel), typeof(CategoryTilePage));
            MappingPageAndViewModel.Add(typeof(CatalogPageViewModel), typeof(CatalogListPage));
            MappingPageAndViewModel.Add(typeof(DetailPageViewModel), typeof(DetailPage));
            MappingPageAndViewModel.Add(typeof(CartPageViewModel), typeof(CartPage));
            MappingPageAndViewModel.Add(typeof(CheckoutPageViewModel), typeof(CheckoutPage));

            MappingPageAndViewModel.Add(typeof(AboutUsViewModel), typeof(AboutUsSimplePage));
        }

        public Page GetPageWithBindingContext(Type type, string parameterName, string parameterValue)
        {
            Type pageType = GetPageForViewModel(type);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {type} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;

            if (string.IsNullOrEmpty(parameterName))
            {
                page.BindingContext = TypeLocator.Instance.Resolve(type) as ViewModelBase;
            }
            else
            {
                page.BindingContext = TypeLocator.Instance.Resolve(type, new Autofac.NamedParameter(parameterName, parameterValue)) as ViewModelBase;
            }

            return page;
        }

        private Type GetPageForViewModel(Type viewModelType)
        {
            if (!MappingPageAndViewModel.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return MappingPageAndViewModel[viewModelType];
        }
    }
}