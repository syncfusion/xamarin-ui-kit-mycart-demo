using System;
using MyCart.ViewModels.Ecommerce;
using MyCart.ViewModels.Login;
using MyCart.ViewModels.Onboarding;
using MyCart.Core.Services;

namespace MyCart.Core
{
    public class Startup
    {
        IPreferenceService preferenceService;

        public Startup(IPreferenceService preferenceService)
        {
            this.preferenceService = preferenceService;
        }

        public Type GetMainPage()
        {
            var isNew = preferenceService.Get("isnew");
            //if (isNew == "false")
            if (true)
            {
                preferenceService.Set("isnew", "false");
                return typeof(OnBoardingGradientViewModel);
            }
            else
            {
                var email = preferenceService.Get("email");

                if (string.IsNullOrEmpty(email))
                {
                    return typeof(LoginPageViewModel);
                }
                else
                {
                    return typeof(CategoryPageViewModel);
                }
            }
        }
    }
}