using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using RealFishki.Models;
using RealFishki.Views;

namespace RealFishki.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        public Command LoadCategoriesCommand { get; set; }

        public CategoriesViewModel()
        {
            Title = "Categories";
            Categories = new ObservableCollection<Category>();
            LoadCategoriesCommand = new Command(async () => await ExecuteLoadCategoriesCommand());

            MessagingCenter.Subscribe<NewCategoryPage, Category>(this, "AddCategory", async (obj, category) =>
            {
                var _category = category as Category;
                Categories.Add(_category);
                await DataStore.AddCategoryAsync(_category);
            });

            MessagingCenter.Subscribe<ItemsPage, Category>(this, "DeleteCategory", async (obj, category) =>
            {
                var _category = category as Category;
                Categories.Remove(_category);
                await DataStore.DeleteCategoryAsync(_category);
            });
        }

        async Task ExecuteLoadCategoriesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Categories.Clear();
                await DataStore.UpdateLocal();
                var categories = await DataStore.GetCategoriesAsync(true);
                foreach (var category in categories)
                {
                    Categories.Add(category);
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