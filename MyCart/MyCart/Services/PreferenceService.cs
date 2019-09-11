using System;
using MyCart.Core.Services;
using Xamarin.Essentials;

namespace MyCart.Services
{
    public class PreferenceService : IPreferenceService
    {
        public string Get(string key)
        {
            return Preferences.Get(key, "");
        }

        public void Set(string key, string value)
        {
            Preferences.Set(key, value);
        }
    }
}