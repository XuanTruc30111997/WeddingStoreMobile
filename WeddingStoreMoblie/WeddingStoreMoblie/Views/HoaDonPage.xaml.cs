using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeddingStoreMoblie.ViewModels;
using WeddingStoreMoblie.Models.AppModels;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HoaDonPage : ContentPage
    {
        HoaDonViewModel vm;
        public HoaDonPage()
        {
            InitializeComponent();
            vm = new HoaDonViewModel();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //await MyAnimation();
            await vm.GetDataAsync();
        }
        //async Task MyAnimation()
        //{
        //    await this.ScaleTo(4, 1000);
        //    await this.ScaleTo(0.1, 500, Easing.BounceIn);
        //    await this.ScaleTo(1, 1000, Easing.BounceOut);
        //}
    }
}