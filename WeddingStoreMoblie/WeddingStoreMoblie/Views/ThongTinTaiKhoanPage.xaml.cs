using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThongTinTaiKhoanPage : ContentPage
    {
        ViewModels.ThongTinTaiKhoanViewModel vm;
        public ThongTinTaiKhoanPage(string maNV)
        {
            InitializeComponent();
            vm = new ViewModels.ThongTinTaiKhoanViewModel(maNV);
            BindingContext = vm;
            SetVisible();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.GetDataAsync();
            await MyAnimation();
        }

        async Task MyAnimation()
        {
            imgAvatar.IsVisible = true;
            await imgAvatar.TranslateTo(0, 200, 500, Easing.SpringOut);
            await imgAvatar.TranslateTo(0, -200, 500, Easing.SpringOut);
            await imgAvatar.TranslateTo(0, 0);

            abUserName.IsVisible = true;
            await abUserName.RotateTo(360, 1000);
            abUserName.Rotation = 0;

            abPassword.IsVisible = true;
            await abPassword.RotateTo(-360, 1000);
            abPassword.Rotation = 0;

            abAction.IsVisible = true;
            await abAction.FadeTo(0.5, 1000, Easing.SinInOut);
            await abAction.FadeTo(1, 1000, Easing.SinInOut);
        }

        void SetVisible()
        {
            imgAvatar.IsVisible = false;
            abUserName.IsVisible = false;
            abPassword.IsVisible = false;
            abAction.IsVisible = false;
        }
    }
}