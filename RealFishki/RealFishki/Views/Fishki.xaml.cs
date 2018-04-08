using RealFishki.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealFishki.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Fishki : ContentPage
	{
        FishkiViewModel viewModel;
        bool visible;
        int i;

		public Fishki ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new FishkiViewModel();

            fiszka.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            fiszka.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            fiszka.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });


            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnFrameTapped;
            frame.GestureRecognizers.Add(tapGestureRecognizer);

            viewModel.LoadFishkiItemsCommand.Execute(null);
            setNewFishka();
        }

        private void setNewFishka()
        {
            visible = false;
            if (viewModel.FishkiItems.Count != 0)
            {
                Random rnd = new Random();
                i = rnd.Next(0, viewModel.FishkiItems.Count);
            }
            setFishka();
        }

        private void setFishka()
        {
            visible = false;
            String title, subject;
            if (viewModel.FishkiItems.Count == 0)
            {
                title = "Task";
                subject = "Subject";
            }
            else
            {
                title = viewModel.FishkiItems.ElementAt(i).Text;
                subject = viewModel.DataStore.GetCategoryAsync(viewModel.FishkiItems.ElementAt(i).CatId).Subject;
            }

            var text = new Label()
            {
                Text = title,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 25,
                Margin = new Thickness(5, 5, 5, 0)
            };

            var sub = new Label()
            {
                Text = subject,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 25,
                Margin = new Thickness(5, 5, 5, 0)
            };

            fiszka.Children.Clear();
            fiszka.Children.Add(sub, 0, 0);
            fiszka.Children.Add(text, 0, 1);
        }

        private async void OnFrameTapped(object sender, EventArgs e)
        {
            frame.IsEnabled = false;
            got_it.IsEnabled = false;
            refresh.IsEnabled = false;
            await fiszka.RotateYTo(90, 400, Easing.CubicIn);
            setNewFishka();
            await fiszka.RotateYTo(-90, 0);
            frame.IsEnabled = true;
            await fiszka.RotateYTo(0, 400, Easing.CubicOut);
            got_it.IsEnabled = true;
            refresh.IsEnabled = true;
        }

        public async void Show(object sender, EventArgs e)
        {
            if (visible)
            {
                setFishka();
            }
            else
            {

                visible = true;
                String descr;
                if (viewModel.FishkiItems.Count == 0)
                {
                    descr = "Sample";
                }
                else
                {
                    descr = viewModel.FishkiItems.ElementAt(i).Description;
                }

                var desc = new Label()
                {
                    Text = descr,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 16,
                    Margin = new Thickness(5, 0, 5, 0)
                };

                fiszka.Children.Add(desc, 0, 2);
            }
        }

        public async void Got_it(object sender, EventArgs e)
        {
            if (viewModel.FishkiItems.Count != 0)
            {
                viewModel.Gotten.Add(viewModel.FishkiItems.ElementAt(i));
                viewModel.FishkiItems.RemoveAt(i);
                OnFrameTapped(sender, e);
            }
        }

        public async void Refresh(object sender, EventArgs e)
        {
            viewModel.LoadFishkiItemsCommand.Execute(null);
            viewModel.Gotten.Clear();
            OnFrameTapped(sender, e);
        }
    }
}