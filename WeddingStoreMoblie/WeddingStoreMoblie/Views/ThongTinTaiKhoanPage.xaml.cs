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
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.GetDataAsync();
        }
    }
}