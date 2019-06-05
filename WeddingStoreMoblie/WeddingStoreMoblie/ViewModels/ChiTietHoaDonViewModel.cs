using System;
using System.Collections.Generic;
using System.Text;

using WeddingStoreMoblie.Models.AppModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using System.Threading;

namespace WeddingStoreMoblie.ViewModels
{
    public class ChiTietHoaDonViewModel : BaseViewModel
    {
        #region Properties
        private string _maHD;
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

        private List<ThongTinPhatSinh> _lstThongTinPhatSinh { get; set; }
        public List<ThongTinPhatSinh> LstThongTinPhatSinh
        {
            get { return _lstThongTinPhatSinh; }
            set
            {
                if (_lstThongTinPhatSinh != value)
                {
                    _lstThongTinPhatSinh = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Services
        private MockThongTinChiTietHoaDonRepository _thongTinChiTietHD = new MockThongTinChiTietHoaDonRepository();
        #endregion

        #region Constructors
        public ChiTietHoaDonViewModel(string maHD)
        {
            _maHD = maHD;
            if (isChanged)
            {
                TestGetDataAfterpopup(_maHD).GetAwaiter();
            }
            else
            {
                GetDataAsync(_maHD).GetAwaiter();
            }
        }
        #endregion

        #region Commands
        public Command ThemPhatSinhCommand
        {
            get
            {
                return new Command(ThemPhatSinh);
            }
        }
        #endregion

        #region Methods
        async Task GetDataAsync(string maHD)
        {
            var t1 = Task.Run(async () =>
            {
                LstThongTinChiTietHoaDon = await _thongTinChiTietHD.GetThongTinChiTietHoaDon(maHD);
            });
            var t2 = Task.Run(async () =>
            {
                LstThongTinPhatSinh = await _thongTinChiTietHD.GetThongTinPhatSinhByIdHD(maHD);
            });

            await Task.WhenAll(t1, t2);

            //new Thread(async () =>
            //{
            //    _lstThongTinChiTietHoaDon = await _thongTinChiTietHD.GetThongTinChiTietHoaDon(maHD);
            //}).Start();

            //new Thread(async () =>
            //{
            //    _lstThongTinPhatSinh = await _thongTinChiTietHD.GetThongTinPhatSinhByIdHD(maHD);
            //});
        }

        public async Task TestGetDataAfterpopup(string maHD)
        {
            _thongTinChiTietHD = new MockThongTinChiTietHoaDonRepository();
            _lstThongTinChiTietHoaDon = await _thongTinChiTietHD.GetThongTinChiTietHoaDon(maHD);
            LstThongTinPhatSinh = _thongTinChiTietHD.TestPhatSinh(maHD);
        }


        private void ThemPhatSinh()
        {
            var themPhatSinhPopupView = new Views.ThemPhatSinhPopupView(_maHD);
            themPhatSinhPopupView.CallbackEvent += (object sender, bool e) => GetDataAsync(_maHD).GetAwaiter();
            PopupNavigation.Instance.PushAsync(themPhatSinhPopupView);
        }

        //private Task ToPopupPage => PopupNavigation.Instance.PushAsync(new Views.ThemPhatSinhPopupView(_maHD));
        #endregion
    }
}
