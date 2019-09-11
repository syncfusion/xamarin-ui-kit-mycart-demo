using MyCart.Models.Ecommerce;
using MyCart.Services;
using Microsoft.Data.Sqlite;
using MyCart.Core.Models.Ecommerce;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MyCart.Core.Services
{
    public class LocalDataStore : IDataStore
    {
        Dictionary<string, Cart> items = new Dictionary<string, Cart>();

        List<Product> products;
        List<MainCategory> categories;

        //TODO: Remove these two properties after passing from shell argument.
        public string SelectedCategory { get; set; }

        public Product SelectedProduct { get; set; }

        //TODO tihs needs to be removed.
        public ObservableCollection<Product> CartList { get; set; }

        private static IDataStore instance;

        private string email = "xam@xam.com";

        public LocalDataStore()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=userdata.db"))
            {
                //SQLitePCL.raw.SetProvider(new SQLitePCL.());

                //db.Open();

                //string tableCommand = "CREATE TABLE IF NOT EXISTS Users (Name NVARCHAR(2048) PRIMARY KEY)";

                //string insert = "INSERT INTO Users (Name) VALUES('Bud Powell')";

                //SqliteCommand command = new SqliteCommand(tableCommand, db);
                //var result = command.ExecuteReader();

                ////new SqliteCommand(insert, db).ExecuteReader();

                //string getString = "SELECT Name from Users";

                //result = new SqliteCommand(getString, db).ExecuteReader();

                //if (result.HasRows)
                //{
                //    var name = result.GetString(0);
                //}
            }

            items.Add("xam@xam.com", new Cart { UserName = "Xamarin", Password = "xamarin", CartList = { "1", "3" } });
            items.Add("user@syncufsion.com", new Cart { UserName = "User", Password = "user123", CartList = { "2", "3" } });
        }

        public static IDataStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LocalDataStore();
                }
                return instance;
            }
        }

        public async Task<bool> AddItemAsync(string pid)
        {
            items.TryGetValue(email, out Cart result);

            if (result != null)
            {
                result.CartList.Add(pid);
                return true;
            }
            return false;
        }

        public async Task<bool> ClearProducts()
        {
            items.TryGetValue(email, out Cart result);
            if (items != null)
            {
                result.CartList.Clear();
                return true;
            }
            return false;
        }

        public List<MainCategory> GetCategories()
        {
            if (categories == null)
            {
                categories = PopulateData<List<MainCategory>>("category.json");
            }
            return categories;
        }

        public async Task<IEnumerable<string>> GetItemsAsync()
        {
            items.TryGetValue(email, out Cart result);

            return result.CartList;
        }

        public List<Product> GetProducts()
        {
            if (products == null)
            {
                products = PopulateData<List<Product>>("products.json");
            }
            return products;
        }

        public async Task<bool> RemoveProduct(string pid)
        {
            items.TryGetValue(email, out Cart result);
            if (items != null && result.CartList.Contains(pid))
            {
                result.CartList.Remove(pid);
                return true;
            }
            return false;
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            items.TryGetValue(email, out Cart result);

            if (result != null && result.Password == password)
            {
                return true;
            }
            return false;
        }

        private T PopulateData<T>(string fileName)
        {
            var file = "MyCart.Core.Data." + fileName;

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

    public class Cart
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public List<string> CartList { get; set; }

        public Cart()
        {
            CartList = new List<string>();
        }
    }
}