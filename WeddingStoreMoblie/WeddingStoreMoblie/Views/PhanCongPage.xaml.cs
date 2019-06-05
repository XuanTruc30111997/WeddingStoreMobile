using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.AppModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhanCongPage : ContentPage
    {
        ViewModels.PhanCongViewModel myVM;
        private string _maHD;
        public PhanCongPage(string maHD)
        {
            InitializeComponent();
            _maHD = maHD;
            myVM = new ViewModels.PhanCongViewModel(_maHD);
            BindingContext = myVM;

            them.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = myVM.ThemPhanCongCommand
            });
        }

        private void Handler_NVTapped(object sender, ItemTappedEventArgs e)
        {
            var nv = e.Item as ThongTinNhanVienPhanCong;
            //var thongTinNhanVienVM = new ViewModels.PhanCongViewModel(nv.MaHD);
            //thongTinNhanVienVM.ClickOnNhanVien(nv);
            myVM.ClickOnNhanVien(nv);
        }
    }
}