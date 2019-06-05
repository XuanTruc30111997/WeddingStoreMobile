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

        public void NavigateOnMaster(int id)
        {
            var _currentPage = GetCurrentPage();
            MasterDetailPage myPage = _currentPage as MasterDetailPage;
            switch (id)
            {
                case 1:
                    myPage.Detail = new NavigationPage(new NhanVienPage());
                    break;
                case 2:
                    myPage.Detail = new NavigationPage(new HoaDonPage());
                    break;
                case 3:
                    myPage.Detail = new NavigationPage(new KhoVatLieuPage());
                    break;
            }
            myPage.IsPresented = false;
        }

        public async Task NavigateToMaster()
        {
            //Page _currentPage = GetCurrentPage();
            //await _currentPage.Navigation.PushAsync(new NavigationPage(new HomePage()));
            Device.BeginInvokeOnMainThread(() =>
            {
                App.Current.MainPage = new NavigationPage(new HomePage());
            });
        }
    }
}
