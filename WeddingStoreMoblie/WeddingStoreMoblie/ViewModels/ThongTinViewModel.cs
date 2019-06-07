using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Models.AppModels;

using WeddingStoreMoblie.Interfaces;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class ThongTinViewModel : BaseViewModel
    {
        #region Properties
        private HoaDonModel _myHD;
        public HoaDonModel MyHD
        {
            get { return _myHD; }
            set
            {
                if (_myHD != value)
                {
                    _myHD = value;
                    OnPropertyChanged();
                }
            }
        }

        private KhachHangModel _myKH;
        public KhachHangModel MyKH
        {
            get { return _myKH; }
            set
            {
                if (_myKH != value)
                {
                    _myKH = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Services
        MockHoaDonRepository _hoaDon = new MockHoaDonRepository();
        private MockKhachHangRepository _khachHang = new MockKhachHangRepository();
        private MockChiTietHoaDonRepository _chiTietHoaDon = new MockChiTietHoaDonRepository();
        private MockChiTietSanPhamRepository _chiTietSanPham = new MockChiTietSanPhamRepository();
        private MockVatLieuRepository _vatLieu = new MockVatLieuRepository();
        private MockPhatSinhRepository _phatSinh = new MockPhatSinhRepository();
        #endregion

        #region Constructors
        public ThongTinViewModel(string maHD, string maKH)
        {
            GetData(maHD, maKH).GetAwaiter();
        }
        #endregion

        #region Commands
        public Command ChangeTinhTrangTo1Command
        {
            get => new Command(() =>  ChangeTinhTrang1());
        }

        public Command ChangeTinhTrangTo2Command
        {
            get => new Command(() => ChangeTinhTrang2());
        }

        public Command ChangeTinhTrangTo3Command
        {
            get => new Command(() => ChangeTinhTrang3());
        }
        #endregion

        #region Methods
        #endregion

        public async Task GetData(string maHD, string maKH)
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });
            var t1 = Task.Run(async () =>
              {
                  await GetHoaDon(maHD);
              });
            var t2 = Task.Run(async () =>
            {
                MyKH = await _khachHang.GetById(maKH);
            });
            await Task.WhenAll(t1, t2);
            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }

        async Task GetHoaDon(string maHD)
        {
            MyHD = await _hoaDon.GetById(maHD);
        }

        void ChangeTinhTrang1()
        {
            bool result = false;
            var page = GetCurrentPage();
            Device.BeginInvokeOnMainThread(async () =>
            {
                result = await page.DisplayAlert("Thông báo!", "Thay đổi tình trạng hóa đơn --> Chưa trang trí?", "Yes", "No").ConfigureAwait(false);
                if (result)
                {
                    if (_myHD.TinhTrang == 1) // Đã trang trí
                    {
                        await UpdateKhoVatLieuThat(false);
                    }

                    MyHD.TinhTrang = 0;
                    bool response = await _hoaDon.SaveDataAsync(_myHD, "HoaDon", false);
                    //MyHD.TinhTrang = 0;
                    Constant.isNewPS = true;
                }
            });

        }

        void ChangeTinhTrang2()
        {
            bool result = false;
            var page = GetCurrentPage();
            Device.BeginInvokeOnMainThread(async () =>
            {
                result = await page.DisplayAlert("Thông báo!", "Thay đổi tình trạng hóa đơn --> Đã trang trí?", "Yes", "No").ConfigureAwait(false);
                if (result)
                {
                    if (_myHD.TinhTrang == 0 || _myHD.TinhTrang == 2)
                    {
                        await UpdateKhoVatLieuThat(true);
                    }
                    MyHD.TinhTrang = 1;
                    bool response = await _hoaDon.SaveDataAsync(_myHD, "HoaDon", false);
                    Constant.isNewPS = true;
                }
            });
        }

        void ChangeTinhTrang3()
        {
            bool result = false;
            var page = GetCurrentPage();
            Device.BeginInvokeOnMainThread(async () =>
            {
                result = await page.DisplayAlert("Thông báo!", "Thay đổi tình trạng hóa đơn --> Đã tháo dở?", "Yes", "No").ConfigureAwait(false);

                if (result)
                {
                    if (_myHD.TinhTrang == 1) // Đã trang trí
                    {
                        await UpdateKhoVatLieuThat(false);
                    }
                    MyHD.TinhTrang = 2;
                    bool response = await _hoaDon.SaveDataAsync(_myHD, "HoaDon", false);
                    Constant.isNewPS = true;
                }
            });
        }

        async Task UpdateKhoVatLieuThat(bool type)
        {
            // Update vật liệu trong chi tiết hóa đơn
            var t1 = Task.Run(async () =>
            {
                List<ChiTietHoaDonModel> lstChiTietHoaDon = await _chiTietHoaDon.GetByIdHD(_myHD.MaHD);
                Parallel.ForEach(lstChiTietHoaDon, async (cthd) =>
                {
                    await UpdateVatLieuInCTHD(cthd, type);
                });
            });

            // Update vật liệu trong danh sách phát sinh.
            var t2 = Task.Run(async () =>
            {
                List<PhatSinhModel> lstPhatSinh = await _phatSinh.GetByIdHD(_myHD.MaHD);
                Parallel.ForEach(lstPhatSinh, async (ps) =>
                {
                    await UpdateVatLieuInPS(ps, type);
                });
            });

            await Task.WhenAll(t1, t2);
        }

        // type: true ==> - vật liệu tồn (chưa trang trí, đã tháo dở ==> đã trang trí)
        // type: false ==> + vật liệu tồn (đã trang trí ==> chưa trang trí, đã tháo dở)
        async Task UpdateVatLieuInCTHD(ChiTietHoaDonModel cthd, bool type)
        {
            List<ChiTietSanPhamModel> lstChiTietSanPham = await _chiTietSanPham.GetByIdSP(cthd.MaSP);
            foreach (var ctsp in lstChiTietSanPham)
            {
                VatLieuModel myVatLieu = await _vatLieu.GetById(ctsp.MaVL);
                if (!myVatLieu.IsNhap)
                {
                    if (type)
                        myVatLieu.SoLuongTon -= (cthd.SoLuong * ctsp.SoLuong);
                    else
                        myVatLieu.SoLuongTon += (cthd.SoLuong * ctsp.SoLuong);
                    bool resule = await _vatLieu.SaveDataAsync(myVatLieu, "VatLieu", false);
                    Console.WriteLine("Update --> " + myVatLieu.SoLuongTon);
                }
            }
        }

        async Task UpdateVatLieuInPS(PhatSinhModel ps, bool type)
        {
            VatLieuModel myVatLieu = await _vatLieu.GetById(ps.MaVL);
            if (!myVatLieu.IsNhap)
            {
                if (type)
                {
                    myVatLieu.SoLuongTon -= ps.SoLuong;
                }
                else
                {
                    myVatLieu.SoLuongTon += ps.SoLuong;
                }
                bool resule = await _vatLieu.SaveDataAsync(myVatLieu, "VatLieu", false);
                Console.WriteLine("Update --> ", +myVatLieu.SoLuongTon);
            }
        }
    }
}
