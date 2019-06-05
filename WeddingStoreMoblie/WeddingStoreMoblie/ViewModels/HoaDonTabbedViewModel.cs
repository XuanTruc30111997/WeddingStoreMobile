using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Views;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class HoaDonTabbedViewModel : BaseViewModel
    {
        #region Properties
        private string _maHD { get; set; }
        private string _maKH { get; set; }
        #endregion
        public HoaDonTabbedViewModel(string maHD,string maKH)
        {
            _maHD = maHD;
            _maKH = maKH;
        }

        Page ReadyThongTin()
        {
            var thongTinPage = new ThongTinPage(_maKH, _maKH);
            thongTinPage.Title = "Thông tin";

            return thongTinPage;
        }

        Page ReadyChiTiet()
        {
            var chiTietPage = new ChiTietHoaDonPage(_maHD);
            chiTietPage.Title = "Chi tiết";

            return chiTietPage;
        }

        Page ReadyPhanCong()
        {
            var phanCongPage = new PhanCongPage(_maHD);
            phanCongPage.Title = "Phân công";

            return phanCongPage;
        }

        public async Task<Page[]> GetDataAsync()
        {
            List<Task<Page>> myTask = new List<Task<Page>>();
            myTask.Add(Task.Run(() => ReadyThongTin()));
            myTask.Add(Task.Run(() => ReadyChiTiet()));
            myTask.Add(Task.Run(() => ReadyPhanCong()));

            var results = await Task.WhenAll(myTask);
            //foreach (var myResult in results)
            //    Children.Add(myResult);
            return results;
        }
    }
}
