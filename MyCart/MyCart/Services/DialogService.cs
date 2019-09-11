using MyCart.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCart.Services
{
    public class DialogService : IDialogService
    {
        public async Task Show(string title, string msg, string closeText)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, msg, closeText);
        }
    }
}