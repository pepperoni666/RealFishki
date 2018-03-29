using RealFishki.Models;
using RealFishki.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

using Xamarin.Forms;
using System.Collections.Generic;

namespace RealFishki.ViewModels
{
    public class FishkiViewModel : BaseViewModel
    {

        public ObservableCollection<Item> FishkiItems { get; set; }
        public List<Item> Gotten { get; set; }
        public Command LoadFishkiItemsCommand { get; set; }

        public FishkiViewModel()
        {
            Title = "Fishki";

            FishkiItems = new ObservableCollection<Item>();
            Gotten = new List<Item>();
            LoadFishkiItemsCommand = new Command(async () => await UpdateFishkiItems());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Item;
                FishkiItems.Add(_item);
            });

            MessagingCenter.Subscribe<ItemDetailPage, Item>(this, "DeleteItem", async (obj, item) =>
            {
                var _item = item as Item;
                FishkiItems.Remove(_item);
            });
        }
        async Task UpdateFishkiItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                FishkiItems.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    FishkiItems.Add(item);
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