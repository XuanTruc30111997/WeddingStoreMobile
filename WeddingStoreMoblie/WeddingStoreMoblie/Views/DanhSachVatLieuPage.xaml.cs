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
    public partial class DanhSachVatLieuPage : ContentPage
    {
        ViewModels.DanhSachVatLieuViewModel vm;
        public DanhSachVatLieuPage(string maHD)
        {
            InitializeComponent();
            vm = new ViewModels.DanhSachVatLieuViewModel(maHD);
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Constant.isNewDanhSachVatLieu)
                await vm.GetDataAsync();
        }
    }
}