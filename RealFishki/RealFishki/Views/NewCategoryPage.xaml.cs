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
                Subject = "Subject",
                Color = "#6A1B9A"
            };

            BindingContext = this;
        }

        async void getColor(object sender, EventArgs e)
        {
            var button = sender as Button;
            Category.Color = GetHexString(button.BackgroundColor);
            color.TextColor = button.BackgroundColor;
        }

        private string GetHexString(Xamarin.Forms.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";

            return hex;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddCategory", Category);
            await Navigation.PopModalAsync();
        }
    }
}