using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Views;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class ThongTinMauViewModel : BaseViewModel
    {
        #region Properties
        private string _maHD;
        private bool _showMenu { get; set; }
        public bool ShowMenu
        {
            get => _showMenu;
            set
            {
                _showMenu = value;
                OnPropertyChanged();
            }
        }
        private List<ThongTinChiTietHoaDon> _lstThongTinChiTietHoaDon { get; set; }
        public List<ThongTinChiTietHoaDon> LstThongTinChiTietHoaDon
        {
            get { return _lstThongTinChiTietHoaDon; }
            set
            {
                if (_lstThongTinChiTietHoaDon != value)
                {
                    _lstThongTinChiTietHoaDon = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        private MockThongTinChiTietHoaDonRepository _thongTinChiTietHD;

        #region Constructor
        public ThongTinMauViewModel(string maHD)
        {
            isFirst = true;
            _maHD = maHD;
            //GetData(maHD).GetAwaiter();
        }
        #endregion

        #region Commands
        public Command ThemChiTietCommand
        {
            get
            {
                return new Command(ThemChiTiet);
            }
        }
        public Command MenuCommand
        {
            get
            {
                return new Command(Ahihi);
            }
        }
        public Command ModifyCommand
        {
            get
            {
                return new Command(Modify);
            }
        }
        #endregion

        #region Methods
        public async Task GetData()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = true;
            });

            _showMenu = false;
            _thongTinChiTietHD = new MockThongTinChiTietHoaDonRepository();
            LstThongTinChiTietHoaDon = await _thongTinChiTietHD.GetThongTinChiTietHoaDon(_maHD);
            isFirst = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = false;
            });
        }
        private void ThemChiTiet()
        {
            Constant.isNew = true;
            var themChiTietHoaDonPopupView = new ThemChiTietHDPopupView(_maHD);
            themChiTietHoaDonPopupView.CallbackEvent += (object sender, bool e) => GetData().GetAwaiter();
            PopupNavigation.Instance.PushAsync(themChiTietHoaDonPopupView);
        }
        private void Ahihi(object obj)
        {
            ShowMenu = !ShowMenu;
        }
        private void Modify(object obj)
        {
            Constant.isNew = true;
            var modifyThongTinMau = new ModifyThongTinMauPopupView(_maHD);
            modifyThongTinMau.CallbackEvent += (object sender, bool e) => GetData().GetAwaiter();
            var ahihi = PopupNavigation.Instance.PushAsync(modifyThongTinMau);
        }
        #endregion


    }
}
