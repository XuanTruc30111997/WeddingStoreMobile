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
    public partial class NhanVienPage : ContentPage
    {
        ViewModels.NhanVienViewModel vm;
        public NhanVienPage()
        {
            InitializeComponent();
            vm = new ViewModels.NhanVienViewModel();
            BindingContext = vm;
        }

        //private void NhanVien_Click(object sender, ItemTappedEventArgs e)
        //{
        //    vm.NhanVienCommand.Execute(null);
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //await MyAnimation();
            await vm.GetData();
        }

        //async Task MyAnimation()
        //{
        //    await this.ScaleTo(4, 1000);
        //    await this.ScaleTo(0.1, 500, Easing.BounceIn);
        //    await this.ScaleTo(1, 1000, Easing.BounceOut);
        //}
    }
}