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
    public partial class LoginPage : ContentPage
    {
        ViewModels.LoginViewModel vm;
        public LoginPage()
        {
            InitializeComponent();
            vm = new ViewModels.LoginViewModel();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ahihi.IsVisible = true;
            myGrid.IsVisible = false;
            //await MyAnimation();
        }

        async Task MyAnimation()
        {
            //await Task.WhenAll(ahihi.ScaleTo(1, 2000),
            //    ahihi.ScaleTo(0.9, 1500, Easing.Linear),
            //    ahihi.ScaleTo(150, 1200, Easing.Linear));

            await ahihi.ScaleTo(0.5, 1500, Easing.Linear);
            await ahihi.ScaleTo(150, 2000, Easing.Linear);

            ahihi.IsVisible = false;
            myGrid.IsVisible = true;

            await imgTop.ScaleTo(5, 1000);
            await imgTop.ScaleTo(0.1, 500, Easing.BounceIn);
            await imgTop.ScaleTo(1, 1000, Easing.BounceOut);

            await lblLogin.TranslateTo(100, 0, 500, Easing.SpringOut);
            await lblLogin.TranslateTo(-100, 0, 500, Easing.SpringOut);
            await lblLogin.TranslateTo(0, 0);

            await Task.WhenAll(
                frameNhap.FadeTo(1, 100),
                frameNhap.RotateXTo(720, 1000, Easing.SpringOut));

            await btnLogin.ScaleTo(0);
            await btnLogin.ScaleTo(1, 1000, Easing.BounceOut);
        }

        private void ahihiTap(object sender, EventArgs e)
        {
            if (!vm.IsClicked)
            {
                vm.IsClicked = true;
                MyAnimation().GetAwaiter();
            }
        }
    }
}