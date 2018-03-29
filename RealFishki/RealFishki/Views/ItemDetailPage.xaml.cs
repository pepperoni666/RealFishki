using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RealFishki.Models;
using RealFishki.ViewModels;

namespace RealFishki.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage : ContentPage
	{
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void Delete(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteItem", viewModel.Item);
            await Navigation.PopModalAsync();
        }
    }
}