using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RealFishki.Models;

namespace RealFishki.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCategoryPage : ContentPage
    {
        public Category Category { get; set; }

        public NewCategoryPage()
        {
            InitializeComponent();

            Category = new Category
            {
                CatItems = new List<Item>(),
                Subject = "Subject"
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddCategory", Category);
            await Navigation.PopModalAsync();
        }
    }
}