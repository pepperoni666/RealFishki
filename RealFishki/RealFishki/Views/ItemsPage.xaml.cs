﻿using System;
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
	public partial class ItemsPage : ContentPage
	{
        ItemsViewModel viewModel;

        public ItemsPage(ItemsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            viewModel.LoadItemsCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(new ItemDetailViewModel(item))));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override bool OnBackButtonPressed()
        {
            viewModel.Delete();
            return false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            viewModel.LoadItemsCommand.Execute(null);
        }

        async void DeleteCat_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteCategory", viewModel.Category);
            viewModel.Delete();
            await Navigation.PopModalAsync();
        }
    }
}