using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.AppModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using WeddingStoreMoblie.ViewModels;
using WeddingStoreMoblie.Views;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChiTietHoaDonPage : TabbedPage
    {
        private string _maHD;
        public ChiTietHoaDonPage(string maHD)
        {
            InitializeComponent();
            _maHD = maHD;

            Ready().GetAwaiter();
        }

        async Task Ready()
        {
            Page ReadyThongTinMauPage()
            {
                var thongTinMauPage = new ThongTinMauPage(_maHD);
                thongTinMauPage.Title = "Mẫu";
                //thongTinMauPage.Icon = "mau.png";
                thongTinMauPage.IconImageSource = Constant.ImagePatch + "mau.png";
                return thongTinMauPage;
            }

            Page ReadyPhatSinhPage()
            {
                var phatSinhPage = new PhatSinhPage(_maHD);
                phatSinhPage.Title = "Phát Sinh";
                //phatSinhPage.Icon = "phatsinh.png";
                phatSinhPage.IconImageSource = Constant.ImagePatch + "PhatSinh.png";
                return phatSinhPage;
            }

            Page ReadyDanhSachVatLieuPage()
            {
                var danhSachVLPage = new DanhSachVatLieuPage(_maHD);
                danhSachVLPage.Title = "Vật Liệu";
                danhSachVLPage.IconImageSource = Constant.ImagePatch + "DanhSachVatLieu.png";
                return danhSachVLPage;
            }

            List<Task<Page>> myTasks = new List<Task<Page>>();
            myTasks.Add(Task.Run(() => ReadyThongTinMauPage()));
            myTasks.Add(Task.Run(() => ReadyPhatSinhPage()));
            myTasks.Add(Task.Run(() => ReadyDanhSachVatLieuPage()));

            var results = await Task.WhenAll(myTasks);
            foreach (var myResult in results)
                Children.Add(myResult);
        }
    }
}