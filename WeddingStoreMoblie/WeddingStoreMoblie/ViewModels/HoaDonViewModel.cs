using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Models.AppModels;
using System.Windows.Input;
using Xamarin.Forms;
using WeddingStoreMoblie.Views;
using WeddingStoreMoblie.Models.SystemModels;
using System.Threading.Tasks;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Threading;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using System.Diagnostics;

namespace WeddingStoreMoblie.ViewModels
{
    public class HoaDonViewModel : BaseViewModel
    {
        #region Properties
        private List<HoaDonModel> _lstHoaDon { get; set; }
        public List<HoaDonModel> LstHoaDon
        {
            get { return _lstHoaDon; }
            set
            {
                _lstHoaDon = value;
                OnPropertyChanged();
            }
        }

        private List<KhachHangModel> _lstKhachHang { get; set; }
        public List<KhachHangModel> LstKhachHang
        {
            get { return _lstKhachHang; }
            set
            {
                _lstKhachHang = value;
                OnPropertyChanged();
            }
        }

        private List<HoaDonKhachHang> _myList { get; set; }
        public List<HoaDonKhachHang> MyList
        {
            get { return _myList; }
            set
            {
                _myList = value;
                OnPropertyChanged();
            }
        }

        private HoaDonKhachHang _SelectedHDKH { get; set; }
        public HoaDonKhachHang SelectedHDKH
        {
            get => _SelectedHDKH;
            set
            {
                _SelectedHDKH = value;
                OnPropertyChanged();
                if (_SelectedHDKH != null)
                {
                    ClickOnHoaDon(_SelectedHDKH).GetAwaiter();
                }
            }
        }
        #endregion

        #region Services
        private MockHoaDonRepository _hoaDon;
        private MockKhachHangRepository _khachHang;
        #endregion

        #region Constructors
        public HoaDonViewModel()
        {
            GetDataAsync().GetAwaiter();
        }
        #endregion

        #region Commands
        public Command RefeshCommand
        {
            get => new Command(async () =>
            {
                await GetDataAsync();
            });
        }
        #endregion

        #region Methods
        async Task ClickOnHoaDon(HoaDonKhachHang hoaDon)
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });
            var currentPage = GetCurrentPage();
            await currentPage.Navigation.PushAsync(new HoaDonTabbedPage(hoaDon));
            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }
        public async Task GetDataAsync()
        {
            _myList = new List<HoaDonKhachHang>();

            var t1 = GetHoaDonAsync();
            var t2 = GetKhachHangAsync();

            await Task.WhenAll(t1, t2);
            foreach (var hoaDon in LstHoaDon)
            {
                KhachHangModel myKh = LstKhachHang.Find(kh => kh.MaKH == hoaDon.MaKH);
                MyList.Add(new HoaDonKhachHang(hoaDon.MaHD, myKh.MaKH, hoaDon.NgayTrangTri, hoaDon.NgayThaoDo, hoaDon.TinhTrang, myKh.TenKH, myKh.SoDT, myKh.DiaChi, hoaDon.TongTien));
            }
        }

        async Task GetHoaDonAsync()
        {
            _hoaDon = new MockHoaDonRepository();
            LstHoaDon = await _hoaDon.GetDataAsync();
        }

        async Task GetKhachHangAsync()
        {
            _khachHang = new MockKhachHangRepository();
            LstKhachHang = await _khachHang.GetDataAsync();
        }
        #endregion
    }
}
