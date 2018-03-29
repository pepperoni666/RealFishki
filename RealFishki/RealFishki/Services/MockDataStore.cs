using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RealFishki.Models;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(RealFishki.Services.MockDataStore))]
namespace RealFishki.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        public readonly SQLiteAsyncConnection Connection;

        ILocalPathToFile localFilePathProvider = new LocalPathToFile();

        List<Item> items;

        public MockDataStore()
        {
            Connection = new SQLiteAsyncConnection(localFilePathProvider.GetLocalPathToFile("database.db1"));
            Connection.CreateTableAsync<Item>().Wait();
            items = new List<Item>();
            UpdateLocalItems();
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            await Connection.InsertAsync(item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            await Connection.DeleteAsync(_item);
            items.Remove(_item);
            await Connection.InsertAsync(item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            await Connection.DeleteAsync(_item);
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<bool> UpdateLocalItems()
        {
            items = await Connection.Table<Item>().ToListAsync();
            return await Task.FromResult(true);
        }
    }
}