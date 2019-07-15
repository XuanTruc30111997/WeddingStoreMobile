using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.SystemModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThongTinNhanVienPage : ContentPage
    {
        ViewModels.ThongTinNhanVienViewModel vm;
        public ThongTinNhanVienPage(string maNV)
        {
            InitializeComponent();
            Title = "Thông tin nhân viên";
            vm = new ViewModels.ThongTinNhanVienViewModel(maNV);
            BindingContext = vm;

            //stackHinh.IsVisible = false;
            //stackThongTin.IsVisible = false;
            //gridChamCong.IsVisible = false;
            //lstChamCong.IsVisible = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var t1 = Task.Run(async () =>
            {
                //await MyAnimationFirst();
            });
            var t2 = Task.Run(async () =>
            {
                await vm.GetData();
            });
            await Task.WhenAll(t1, t2);
            //await MyAnimationSecond();
        }

        //async Task MyAnimationFirst()
        //{
        //    await this.ScaleTo(4, 1000);
        //    await this.ScaleTo(0.1, 500, Easing.BounceIn);
        //    await this.ScaleTo(1, 1000, Easing.BounceOut);
        //}

        //async Task MyAnimationSecond()
        //{
        //    stackHinh.IsVisible = true;
        //    await stackHinh.TranslateTo(0, 500, 1000, Easing.SpringOut);
        //    await stackHinh.TranslateTo(0, 0);

        //    stackThongTin.IsVisible = true;
        //    await stackThongTin.FadeTo(0.2, 1000, Easing.SinInOut);
        //    await stackThongTin.FadeTo(1, 1000, Easing.SinInOut);

        //    gridChamCong.IsVisible = true;
        //    await gridChamCong.FadeTo(0.6, 1000, Easing.SinInOut);
        //    await gridChamCong.RotateTo(360, 1000);
        //    gridChamCong.Rotation = 0;
        //    await gridChamCong.FadeTo(1, 1000, Easing.SinInOut);

        //    lstChamCong.IsVisible = true;
        //    await lstChamCong.FadeTo(0.4, 500, Easing.CubicIn);
        //    await lstChamCong.FadeTo(1, 500, Easing.CubicOut);
        //}
    }
}