using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyCart.Models;
using System.Linq;
using MyCart.Models.Ecommerce;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;
using MyCart.Core.Models.Ecommerce;

namespace MyCart.Services
{
    public class AzureDataStore : IDataStore
    {
        HttpClient client;
        List<Product> products;
        List<MainCategory> categories;

        string email = "";

        public AzureDataStore()
        {
            //TODO:Essential email = Preferences.Get("email", "");
            GetProducts();
            client = new HttpClient
            {
                BaseAddress = new Uri($"https://{"replace your webiste"}.azurewebsites.net/")
            };
        }

        //TODO: Remove these two properties after passing from shell argument.
        public string SelectedCategory { get; set; }

        public Product SelectedProduct { get; set; }

        //TODO tihs needs to be removed.
        public ObservableCollection<Product> CartList { get; set; }

        private static IDataStore instance;

        public static IDataStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureDataStore();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<string>> GetItemsAsync()
        {
            //TODO:Essentialif (IsConnected)
            {
                var json = await client.GetStringAsync($"api/item?email={email}");
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<string>>(json));
            }

            return null;
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            var result = await client.GetStringAsync($"api/validate?email={email}&password={password}");
            if (result == "true")
            {
                //TODO:Essential Preferences.Set("email", "");
                this.email = email;
                return true;
            }
            return false;
        }


        public async Task<bool> RemoveProduct(string pid)
        {
            //TODO:Essentialif (string.IsNullOrEmpty(email) || !IsConnected)
            if (string.IsNullOrEmpty(email))
                return false;

            var result = await client.GetStringAsync($"api/remove?email={email}&product={pid}");

            return result == "true";
        }

        public async Task<bool> ClearProducts()
        {
            //TODO:if (string.IsNullOrEmpty(email) || !IsConnected)
            if (string.IsNullOrEmpty(email))
                return false;

            var result = await client.GetStringAsync($"api/clear?email={email}");

            return result == "true";
        }

        public async Task<bool> AddItemAsync(string pid)
        {
            //TODO:if (string.IsNullOrEmpty(email) || !IsConnected)
            if (string.IsNullOrEmpty(email))
                return false;

            var result = await client.GetStringAsync($"api/add?email={email}&product={pid}");

            return result == "true";
        }

        public List<Product> GetProducts()
        {
            if (products == null)
            {
                products = PopulateData<List<Product>>("products.json");
            }
            return products;
        }

        public List<MainCategory> GetCategories()
        {
            if (categories == null)
            {
                categories = PopulateData<List<MainCategory>>("category.json");
            }
            return categories;
        }

        private T PopulateData<T>(string fileName)
        {
            var file = "MyCart.Data." + fileName;

            var assembly = this.GetType().Assembly;

            T obj;

            using (var stream = assembly.GetManifestResourceStream(file))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                obj = (T)serializer.ReadObject(stream);
            }

            return obj;
        }
    }
}