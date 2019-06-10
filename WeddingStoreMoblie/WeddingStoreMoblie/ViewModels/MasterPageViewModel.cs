using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.Services;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class MasterPageViewModel : BaseViewModel
    {
        private string _maNV;
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

        private TinhNang _SelectedTinhNang;
        public TinhNang SelectedTinhNang
        {
            get { return _SelectedTinhNang; }
            set
            {
                _SelectedTinhNang = value;
                OnPropertyChanged();
                if (_SelectedTinhNang != null)
                    CLickOnChucNang();
            }
        }

        private NhanVienModel _MyNhanVien;

        public NhanVienModel MyNhanVien
        {
            get { return _MyNhanVien; }
            set { _MyNhanVien = value; OnPropertyChanged(); }
        }


        private MockTinhNangRepository _tinhNang = new MockTinhNangRepository();
        private NavigationService _myNavigationService = new NavigationService();
        MockNhanVienRepository nhanVienMock = new MockNhanVienRepository();
        public MasterPageViewModel(string maNV)
        {
            _maNV = maNV;
        }

        public Command ToDanhMucCommand
        {
            get => new Command(ToDanhMuc);
        }

        public Command ThonTinTaiKhoanCommand
        {
            get => new Command(ToThongTinTaiKhoan);
        }
        public async Task GetDataAsync()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = true;
            });
            MyNhanVien = await nhanVienMock.GetById(_maNV);
            LstTinhNang = _tinhNang.GetTinhNang();
            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = false;
            });
        }

        private void CLickOnChucNang()
        {
            _myNavigationService.NavigateOnMaster(_SelectedTinhNang.id, null);
        }
        void ToDanhMuc()
        {
            _myNavigationService.NavigateToMaster(_maNV, 1, null);
        }

        void ToThongTinTaiKhoan()
        {
            _myNavigationService.NavigateOnMaster(4, _maNV);
        }
    }
}
