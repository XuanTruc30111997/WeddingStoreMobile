using System;
using System.Collections.Generic;
using System.Text;

using WeddingStoreMoblie.Models.AppModels;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;
using WeddingStoreMoblie.Views;
using Rg.Plugins.Popup.Services;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Linq;
using WeddingStoreMoblie.MockDatas.MockDataApp;

namespace WeddingStoreMoblie.ViewModels
{
    public class PhanCongViewModel : BaseViewModel
    {
        #region Properties

        private string _maHD;
        private List<PhanCongNhanVien> _lstPhanCongNhanVien { get; set; }
        public List<PhanCongNhanVien> LstPhanCongNhanVien
        {
            get => _lstPhanCongNhanVien;
            set
            {
                _lstPhanCongNhanVien = value;
                OnPropertyChanged();
            }
        }

        private ThongTinNhanVienPhanCong _selectedNhanVienPhanCong { get; set; }
        public ThongTinNhanVienPhanCong SelectedNhanVienPhanCong
        {
            get => _selectedNhanVienPhanCong;
            set
            {
                _selectedNhanVienPhanCong = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Services
        MockHoaDonRepository _hoaDon = new MockHoaDonRepository();
        MockPhanCongNhanVienRepository _phanCongNhanVien = new MockPhanCongNhanVienRepository();
        #endregion

        #region Constructors
        public PhanCongViewModel(string maHD)
        {
            _maHD = maHD;
            GetData().GetAwaiter();
            NhanVienClick = new Command<ThongTinNhanVienPhanCong>(ClickOnNhanVien);
        }
        #endregion

        #region Commands
        public ICommand NhanVienClick { get; private set; }
        public Command ThemPhanCongCommand
        {
            get
            {
                return new Command(ThemPhanCong);
            }
        }
        public Command RefreshCommand
        {
            get => new Command(async () =>
             {
                 await GetData();
             });
        }
        #endregion

        #region Methods
        public void ClickOnNhanVien(ThongTinNhanVienPhanCong thongTin)
        {
            // Tại sao SelectedNhanVienPhanCong không set giá trị

            //var currentPage = GetCurrentPage();
            //currentPage.Navigation.PushAsync(new ThongTinNhanVienPage(maNV));
            SelectedNhanVienPhanCong = thongTin;

            var modifyPhanCongPopupView = new ModifyPhanCongPopupView(_selectedNhanVienPhanCong);
            modifyPhanCongPopupView.CallbackEvent += (object sender, bool e) => GetData().GetAwaiter();
            PopupNavigation.Instance.PushAsync(modifyPhanCongPopupView);
        }
        public async Task GetData()
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });
            HoaDonModel hoaDon = await _hoaDon.GetById(_maHD);
            LstPhanCongNhanVien = await _phanCongNhanVien.GetPhanCongNhanVienByIdHDNgay(hoaDon.MaHD, hoaDon.NgayTrangTri, hoaDon.NgayThaoDo);
            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }
        private void ThemPhanCong()
        {
            var themPhanCongPopupView = new ThemPhanCongPopupView(_maHD);
            themPhanCongPopupView.CallbackEvent += (object sender, bool e) => GetData().GetAwaiter();
            PopupNavigation.Instance.PushAsync(themPhanCongPopupView);
        }
        #endregion
    }
}
