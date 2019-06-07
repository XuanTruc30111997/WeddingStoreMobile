using Syncfusion.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Converters;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhatSinhPage : ContentPage
    {
        Image modifyImage;
        Image deleteImage;
        ViewModels.PhatSinhViewModel myVM;
        private string _maHD;

        public PhatSinhPage(string maHD)
        {
            InitializeComponent();
            _maHD = maHD;
            myVM = new ViewModels.PhatSinhViewModel(_maHD);
            BindingContext = myVM;

            them.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = myVM.ThemPhatSinhCommand
            });
        }

        private object CreateGroupHeaderTemplate()
        {
            var grid = new Grid { BackgroundColor = Color.FromHex("#77a4dd") };

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
            };

            var lblCount = new Label
            {
                FontAttributes = FontAttributes.Italic,
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.End
            };
            lblCount.SetBinding(Label.TextProperty, new Binding("Count", stringFormat: "{0:0} items"));

            label.SetBinding(Label.TextProperty, new Binding("Key"));

            grid.Children.Add(image, 0, 0);
            grid.Children.Add(label, 1, 0);
            grid.Children.Add(lblCount, 2, 0);

            return grid;
        }

        private void modifyImage_BindingContextChanged(object sender, EventArgs e)
        {
            if (modifyImage == null)
            {
                modifyImage = sender as Image;
                (modifyImage as View).GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = myVM.ModifyCommand
                });
            }
        }

        private void deleteImage_BindingContextChanged(object sender, EventArgs e)
        {
            if (deleteImage == null)
            {
                deleteImage = sender as Image;
                (deleteImage as View).GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = myVM.DeleteCommand
                });
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if(Constant.isNewPS)
            {
                myVM.isBusy = true;
                await myVM.GetThongTinHoaDon();
                myVM.isBusy = false;
            }

            if (myVM.isFirst)
            {
                myVM.isBusy = true;
                await myVM.GetData();
                myVM.isBusy = false;
            }
        }
    }
}