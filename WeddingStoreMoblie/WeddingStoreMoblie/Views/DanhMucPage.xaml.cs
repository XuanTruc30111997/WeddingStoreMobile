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
    public partial class DanhMucPage : ContentPage
    {
        ViewModels.DanhMucViewModel vm;
        public DanhMucPage(string maNV)
        {
            InitializeComponent();
            vm = new ViewModels.DanhMucViewModel(maNV);
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.GetData();
        }

        //private async void ThongTin_Tapped(object sender, EventArgs e)
        //{
        //    //await frameThongTin.ScaleTo(1, 2000);
        //    //await frameThongTin.ScaleTo(0.9, 1500, Easing.Linear);
        //    //await frameThongTin.ScaleTo(150, 2000, Easing.Linear);
        //    await Ahaha(frameThongTin);
        //    vm.ThongTinCommand.Execute(null);
        //}

        //private async void NhanVien_Tapped(object sender, EventArgs e)
        //{
        //    await Ahaha(frameNhanVien);
        //    vm.NhanVienCommand.Execute(null);
        //}

        //private async void HoaDon_Tapped(object sender, EventArgs e)
        //{
        //    await Ahaha(frameHoaDon);
        //    vm.HoaDonCommand.Execute(null);
        //}

        //private async void VatLieu_Tapped(object sender, EventArgs e)
        //{
        //    await Ahaha(frameVatLieu);
        //    vm.VatLieuCommand.Execute(null);
        //}

        //async Task Ahaha(Frame myFrame)
        //{
        //    await myFrame.ScaleTo(0.6, 1500, Easing.Linear);
        //    await myFrame.ScaleTo(150, 2000, Easing.Linear);
        //}
    }
}