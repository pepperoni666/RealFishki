using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RealFishki.Models;
using RealFishki.Views;
using RealFishki.ViewModels;

namespace RealFishki.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriesPage : ContentPage
	{
        CategoriesViewModel viewModel;

        public CategoriesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CategoriesViewModel();
            viewModel.LoadCategoriesCommand.Execute(null);
        }

        async void OnCategorySelected(object sender, SelectedItemChangedEventArgs args)
        {
            var category = args.SelectedItem as Category;
            if (category == null)
                return;

            await Navigation.PushModalAsync(new NavigationPage(new ItemsPage(new ItemsViewModel(category))));

            // Manually deselect item.
            CategoriesListView.SelectedItem = null;
        }

        async void AddCategory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewCategoryPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            viewModel.LoadCategoriesCommand.Execute(null);
        }
    }
}