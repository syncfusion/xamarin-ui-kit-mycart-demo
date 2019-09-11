using MyCart.Models.Ecommerce;
using MyCart.Core.Models.Ecommerce;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyCart.Services
{
    public interface IDataStore
    {
        Task<bool> AddItemAsync(string pid);
        Task<bool> RemoveProduct(string pid);
        Task<bool> ClearProducts();
        Task<IEnumerable<string>> GetItemsAsync();
        Task<bool> ValidateUser(string email, string password);
        List<Product> GetProducts();
        List<MainCategory> GetCategories();
    }
}