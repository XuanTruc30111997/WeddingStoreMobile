using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Services;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class MasterPageViewModel:BaseViewModel
    {
        private List<TinhNang> _lstTinhNang;
        public List<TinhNang> LstTinhNang
        {
            get { return _lstTinhNang; }
            set
            {
                _lstTinhNang = value;
                OnPropertyChanged();
            }
        }

        private MockTinhNangRepository _tinhNang;
        private NavigationService _myNavigationService;
        public MasterPageViewModel()
        {
            _myNavigationService = new NavigationService();
            _tinhNang = new MockTinhNangRepository();
            LstTinhNang = _tinhNang.GetTinhNang();

            ChucNangClick = new Command<int>(CLickOnChucNang);
        }

        public ICommand ChucNangClick { get; set; }

        public void CLickOnChucNang(int id)
        {
            _myNavigationService.NavigateOnMaster(id);
        }
    }
}
