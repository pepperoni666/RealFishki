using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using RealFishki.Models;
using RealFishki.Views;

namespace RealFishki.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Category Category { get; set; }

        public ItemsViewModel(Category category)
        {
            Category = category;
            Title = Category.Subject;
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Item;
                Items.Add(_item);
                await DataStore.AddItemAsync(_item, Category);
            });

            MessagingCenter.Subscribe<ItemDetailPage, Item>(this, "DeleteItem", async (obj, item) =>
            {
                var _item = item as Item;
                Items.Remove(_item);
                await DataStore.DeleteItemAsync(_item);
            });
        }

        public void Delete()
        {
            MessagingCenter.Unsubscribe<NewItemPage, Item>(this, "AddItem");
            MessagingCenter.Unsubscribe<NewItemPage, Item>(this, "DeleteItem");
            Category = null;
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                await DataStore.UpdateLocal();
                var cat = DataStore.GetCategoryAsync(Category.Id);
                var items = cat.CatItems;
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}