using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using Xamarin.Forms;
using WeddingStoreMoblie.Services;

namespace WeddingStoreMoblie.ViewModels
{
    public class DanhMucViewModel:BaseViewModel
    {
        #region Properties
        private string _maNV;

        private TinhNang _SelectedTinhNang { get; set; }
        public TinhNang SelectedTinhNang
        {
            get => _SelectedTinhNang;
            set
            {
                _SelectedTinhNang = value;
                OnPropertyChanged();
            }
        }

        private NhanVienModel _MyNhanVien { get; set; }
        public NhanVienModel MyNhanVien
        {
            get => _MyNhanVien;
            set
            {
                _MyNhanVien = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Services
        MockNhanVienRepository nhanVien = new MockNhanVienRepository();
        NavigationService _myNavigationService = new NavigationService();
        #endregion

        #region Constructors
        public DanhMucViewModel(string maNV)
        {
            _maNV = maNV;
        }
        #endregion

        #region Commands
        public Command ThongTinCommand
        {
            get => new Command(ToThongTin);
        }
        public Command NhanVienCommand
        {
            get => new Command(ToNhanVien);
        }
        public Command HoaDonCommand
        {
            get => new Command(ToHoaDon);
        }
        public Command VatLieuCommand
        {
            get => new Command(ToVatLieu);
        }
        #endregion

        #region Methods
        public async Task GetData()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = true;
            });
            MyNhanVien = await nhanVien.GetById(_maNV);
            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = false;
            });
        }

        void ToThongTin()
        {
            _myNavigationService.NavigateToMaster(_maNV, 2, 4);
        }
        void ToNhanVien()
        {
            _myNavigationService.NavigateToMaster(_maNV, 2, 1);
        }
        void ToHoaDon()
        {
            _myNavigationService.NavigateToMaster(_maNV, 2, 2);
        }
        void ToVatLieu()
        {
            _myNavigationService.NavigateToMaster(_maNV, 2, 3);
        }
        #endregion
    }
}
