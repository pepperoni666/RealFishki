using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RealFishki.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;

[assembly: Xamarin.Forms.Dependency(typeof(RealFishki.Services.MockDataStore))]
namespace RealFishki.Services
{
    public class MockDataStore : IDataStore<Item, Category>
    {
        public readonly SQLiteConnection Connection;

        ILocalPathToFile localFilePathProvider = new LocalPathToFile();

        List<Item> items;
        List<Category> categories;

        public MockDataStore()
        {
            Connection = new SQLiteConnection(localFilePathProvider.GetLocalPathToFile("database.db1"));
            Connection.CreateTable<Item>();
            Connection.CreateTable<Category>();
            items = new List<Item>();
            categories = new List<Category>();
            UpdateLocal();
        }

        public async Task<bool> AddItemAsync(Item item, Category category)
        {
            Connection.Insert(item);
            category.CatItems.Add(item);
            Connection.UpdateWithChildren(category);
            items.Add(item);


            return await Task.FromResult(true);
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            Connection.Insert(category);
            categories.Add(category);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            Connection.Update(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            Category _cat = GetCategoryAsync(item.CatId);
            _cat.CatItems.Remove(item);
            items.Remove(item);
            Connection.UpdateWithChildren(_cat);
            Connection.Delete(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            Connection.UpdateWithChildren(category);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            Connection.Delete(category);
            categories.Remove(category);
            foreach (Item it in category.CatItems)
            {
                items.Remove(it);
                Connection.Delete(it);
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAll()
        {
            Connection.DropTable<Item>();
            Connection.DropTable<Category>();
            items.RemoveRange(0, items.Count);
            categories.RemoveRange(0, items.Count);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public Category GetCategoryAsync(int id)
        {
            return categories.FirstOrDefault(s => s.Id == id);
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(categories);
        }

        public async Task<bool> UpdateLocal()
        {
            categories = Connection.GetAllWithChildren<Category>();
            items = Connection.Table<Item>().ToList();
            return await Task.FromResult(true);
        }
    }
}