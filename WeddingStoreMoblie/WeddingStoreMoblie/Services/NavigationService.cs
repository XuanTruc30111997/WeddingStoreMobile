using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.ViewModels;
using WeddingStoreMoblie.Views;
using Xamarin.Forms;

namespace WeddingStoreMoblie.Services
{
    public class NavigationService : BaseViewModel
    {

        public void NavigateOnMaster(int id,string maNV)
        {
            var _currentPage = GetCurrentPage();
            MasterDetailPage myPage = _currentPage as MasterDetailPage;
            switch (id)
            {
                case 1: // Detail by NhanVien
                    myPage.Detail = new NavigationPage(new NhanVienPage());
                    break;
                case 2: // Detail by HoaDon
                    myPage.Detail = new NavigationPage(new HoaDonPage());
                    break;
                case 3: // Detail by VatLieu
                    myPage.Detail = new NavigationPage(new KhoVatLieuPage());
                    break;
                case 4: // Detail by ThongTinTaiKhoan
                    myPage.Detail = new NavigationPage(new ThongTinTaiKhoanPage(maNV));
                    break;
            }
            myPage.IsPresented = false;
        }

        public void NavigateToMaster(string maNV, int type, int? request)
        {
            //Page _currentPage = GetCurrentPage();
            //await _currentPage.Navigation.PushAsync(new NavigationPage(new HomePage()));
            Device.BeginInvokeOnMainThread(() =>
            {
                //App.Current.MainPage = new NavigationPage(new HomePage(maNV));
                switch(type)
                {
                    case 1: // To DanhMuc
                        App.Current.MainPage = new NavigationPage(new DanhMucPage(maNV));
                        break;
                    case 2: // To Home
                        App.Current.MainPage = new NavigationPage(new HomePage(maNV,request.Value));
                        break;
                    case 3: // To Login
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                        break;
                }
            });
        }
    }
}
