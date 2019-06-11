using Syncfusion.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Services;
using WeddingStoreMoblie.Models.SystemModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeddingStoreMoblie.Converters;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KhoVatLieuPage : ContentPage
    {
        ViewModels.VatLieuViewModel vm;
        public KhoVatLieuPage()
        {
            InitializeComponent();
            vm = new ViewModels.VatLieuViewModel();
            BindingContext = vm;
        }

        private object CreateGroupHeaderTemplate()
        {
            var grid = new Grid { BackgroundColor = Color.FromHex("#6f809b") };

            var column0 = new ColumnDefinition { Width = 30 };
            var column1 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            var column2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            grid.ColumnDefinitions.Add(column0);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);

            var image = new Image();
            Binding binding = new Binding("IsExpand");
            binding.Converter = new BoolToImageConverter();
            image.SetBinding(Image.SourceProperty, binding);

            image.HeightRequest = 30;
            image.WidthRequest = 30;
            image.VerticalOptions = LayoutOptions.Center;
            image.HorizontalOptions = LayoutOptions.Center;

            var label = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 0, 0, 0),
                TextColor = Color.White
            };

            label.SetBinding(Label.TextProperty, new Binding("Key"));

            var lblCount = new Label
            {
                FontAttributes = FontAttributes.Italic,
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Color.White
            };
            lblCount.SetBinding(Label.TextProperty, new Binding("Count", stringFormat: "{0:0} items"));

            grid.Children.Add(image, 0, 0);
            grid.Children.Add(label, 1, 0);
            grid.Children.Add(lblCount, 2, 0);

            return grid;
        }

        private void Handler_keyWordChanged(object sender, TextChangedEventArgs e)
        {
            var vlPageVM = BindingContext as ViewModels.VatLieuViewModel;
            vlPageVM.SearchCommand.Execute(null);
        }

        private void ahihi(object sender, EventArgs e)
        {
            DisplayAlert("AHIHI", "fdsfdsfds", "OK");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await MyAnimation();
            await vm.GetData();
        }

        async Task MyAnimation()
        {
            await this.ScaleTo(4, 1000);
            await this.ScaleTo(0.1, 500, Easing.BounceIn);
            await this.ScaleTo(1, 1000, Easing.BounceOut);
        }
    }
}