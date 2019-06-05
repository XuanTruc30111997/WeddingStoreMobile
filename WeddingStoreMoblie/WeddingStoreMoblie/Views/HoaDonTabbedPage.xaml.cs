using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using WeddingStoreMoblie.Models.AppModels;
using BottomBar.XamarinForms;
using System.Threading;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HoaDonTabbedPage : TabbedPage
    {
        private HoaDonKhachHang _hoaDonKH;
        ViewModels.HoaDonTabbedViewModel vm;
        public HoaDonTabbedPage(HoaDonKhachHang hoaDonKH)
        {
            InitializeComponent();

            _hoaDonKH = hoaDonKH;
            //vm = new ViewModels.HoaDonTabbedViewModel(_hoaDonKH.MaHD, _hoaDonKH.MaKH);
            //BindingContext = vm;
            Title = "Hóa Đơn " + _hoaDonKH.TenKH;
            ReadyForPage().GetAwaiter();
            //Ready().GetAwaiter();
        }

        async Task ReadyForPage()
        {
            Page ReadyThongTinPage()
            {
                var thongTinPage = new ThongTinPage(_hoaDonKH.MaHD, _hoaDonKH.MaKH);
                thongTinPage.Title = "Thông tin";

                return thongTinPage;
            }

            Page ReadyChiTietPage()
            {
                var chiTietPage = new ChiTietHoaDonPage(_hoaDonKH.MaHD);
                chiTietPage.Title = "Chi tiết";

                return chiTietPage;
            }

            Page ReadyPhanCongPage()
            {
                var phanCongPage = new PhanCongPage(_hoaDonKH.MaHD);
                phanCongPage.Title = "Phân công";

                return phanCongPage;
            }

            List<Task<Page>> myTask = new List<Task<Page>>();
            myTask.Add(Task.Run(() => ReadyThongTinPage()));
            myTask.Add(Task.Run(() => ReadyChiTietPage()));
            myTask.Add(Task.Run(() => ReadyPhanCongPage()));

            var results = await Task.WhenAll(myTask);
            foreach (var myResult in results)
                Children.Add(myResult);
        }

        async Task Ready()
        {
            Page[] myPage = await vm.GetDataAsync();
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var my in myPage)
                    Children.Add(my);
            });
        }
    }
}