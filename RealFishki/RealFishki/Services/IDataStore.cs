using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealFishki.Services
{
    public interface IDataStore<T, C>
    {
        Task<bool> AddItemAsync(T item, C category);
        Task<bool> AddCategoryAsync(C category);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<bool> UpdateCategoryAsync(C category);
        Task<bool> DeleteCategoryAsync(C category);
        Task<T> GetItemAsync(int id);
        C GetCategoryAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<C>> GetCategoriesAsync(bool forceRefresh = false);
        Task<bool> UpdateLocal();
    }
}
