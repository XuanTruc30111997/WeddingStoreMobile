using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Interfaces;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using Xamarin.Forms;
using WeddingStoreMoblie.Functions;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using System.Linq;

namespace WeddingStoreMoblie.ViewModels
{
    public class ThemChiTietHDPopupViewModel : BaseViewModel
    {
        #region Properties
        private string _maHD;
        private HoaDonModel _myHoaDon;
        private List<SanPhamAo> _LstSanPhamAo;
        public List<SanPhamAo> LstSanPhamAo
        {
            get => _LstSanPhamAo;
            set
            {
                _LstSanPhamAo = value;
                OnPropertyChanged();
            }
        }

        private SanPhamAo _SelectedSanPhamAo;
        public SanPhamAo SelectedSanPhamAo
        {
            get => _SelectedSanPhamAo;
            set
            {
                _SelectedSanPhamAo = value;
                OnPropertyChanged();
            }
        }

        private int _soLuong;
        public int soLuong
        {
            get { return _soLuong; }
            set
            {
                if (_soLuong != value)
                {
                    _soLuong = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Services
        MockSanPhamRepository sanPham = new MockSanPhamRepository();
        MockChiTietSanPhamRepository chiTietSanPham = new MockChiTietSanPhamRepository();
        MockVatLieuRepository vatLieu = new MockVatLieuRepository();
        MockHoaDonRepository hoaDon = new MockHoaDonRepository();

        MockKhoVatLieuAoRepository vatLieuAo = new MockKhoVatLieuAoRepository();
        #endregion

        #region Constructors
        public ThemChiTietHDPopupViewModel(string maHD)
        {
            _maHD = maHD;
            //GetData().GetAwaiter();
        }
        #endregion

        #region Commands
        private ICommand themCommand;
        public ICommand ThemCommand
        {
            get
            {
                return themCommand ?? (themCommand = new Command(async () => await Them()));
            }
        }
        public Command DoneCommand
        {
            get => new Command(Done);
        }
        #endregion

        #region Methods
        public async Task GetData()
        {
            _myHoaDon = await hoaDon.GetById(_maHD);
            await GetSanPhamAo();
        }

        async Task GetSanPhamAo()
        {
            LstSanPhamAo = await vatLieuAo.GetSanPhamAo(_myHoaDon.NgayTrangTri, _myHoaDon.NgayThaoDo, _myHoaDon.MaHD);
        }

        public Func<object, bool, object> CallbackEvent { get; internal set; }
        private async Task Them()
        {
            MockChiTietHoaDonRepository chiTietHoaDon = new MockChiTietHoaDonRepository();
            if (_SelectedSanPhamAo != null)
            {
                var ahihi = GetCurrentPage();
                if (!_SelectedSanPhamAo.AllNhap && _soLuong > _SelectedSanPhamAo.SoLuong)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ahihi.DisplayAlert("Fail!!!", "Thêm mẫu " + _SelectedSanPhamAo.TenSP + " thất bại. Số lượng tối đa: " + _SelectedSanPhamAo.SoLuong, "OK").ConfigureAwait(false);
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await ahihi.DisplayAlert("Thêm mẫu!!!", "Bạn muốn thêm mẫu " + _SelectedSanPhamAo.TenSP + " ?", "Yes", "No").ConfigureAwait(false);
                        if (result)
                        {
                            Constant.isNewDanhSachVatLieu = true;
                            Constant.isNewMau = true;
                            // Danh sách các mẫu có trong hóa đơn
                            List<ChiTietHoaDonModel> lstChiTiet = await chiTietHoaDon.GetByIdHD(_maHD).ConfigureAwait(false);
                            bool isNew = true;
                            bool response = false;
                            foreach (var ct in lstChiTiet)
                            {
                                if (ct.MaSP == _SelectedSanPhamAo.MaSP) // kiểm tra sản phẩm chọn đã có trong danh sách chưa
                                {
                                    // Đã có
                                    isNew = false;
                                    // Update mẫu trong chi tiết hóa đơn
                                    response = await chiTietHoaDon.SaveDataAsync(new ChiTietHoaDonModel
                                    {
                                        MaHD = _maHD,
                                        MaSP = _SelectedSanPhamAo.MaSP,
                                        SoLuong = _soLuong + ct.SoLuong,
                                        ThanhTien = (_SelectedSanPhamAo.GiaTien * _soLuong) + ct.ThanhTien,
                                    }, "ChiTietHoaDon", false).ConfigureAwait(false);
                                    break;
                                }
                            }
                            if (isNew)
                            {
                                // Insert mẫu vào chi tiết hóa đơn
                                response = await chiTietHoaDon.SaveDataAsync(new ChiTietHoaDonModel
                                {
                                    MaHD = _maHD,
                                    MaSP = _SelectedSanPhamAo.MaSP,
                                    SoLuong = _soLuong,
                                    ThanhTien = _SelectedSanPhamAo.GiaTien * _soLuong,
                                }, "ChiTietHoaDon", true).ConfigureAwait(false);
                            }

                            // Thông báo tình trạng
                            if (response)
                            {
                                var t1 = Task.Run(async () =>
                                {
                                    if (_myHoaDon.TinhTrang == 1 && !_SelectedSanPhamAo.AllNhap)
                                    {
                                        // Update số lượng vật liệu trong kho
                                        List<ChiTietSanPhamModel> lstCTSP = await chiTietSanPham.GetByIdSP(_SelectedSanPhamAo.MaSP);
                                        Parallel.ForEach(lstCTSP, async (ctsp) =>
                                        {
                                            await UpdateVatLieu(ctsp);
                                        });
                                    }
                                });

                                var t2 = Task.Run(async () =>
                                  {
                                      // Update tổng tiền hóa đơn
                                      await UpdateTongTienHoaDon();
                                  });

                                var t3 = Task.Run(() =>
                                {
                                    if (!_SelectedSanPhamAo.AllNhap)
                                    {
                                        // Update Kho sản phẩm ảo
                                        SanPhamAo mySpAo = _LstSanPhamAo.FirstOrDefault(sp => sp.MaSP == _SelectedSanPhamAo.MaSP);
                                        mySpAo.SoLuong -= _soLuong;
                                    }
                                });
                                Console.WriteLine("Done ALl");
                                await Task.WhenAll(t1, t2, t3).ConfigureAwait(false);
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await ahihi.DisplayAlert("Thành công", "Thêm mẫu " + _SelectedSanPhamAo.TenSP + " thành công.", "OK").ConfigureAwait(false);
                                    OnPropertyChanged(nameof(LstSanPhamAo));
                                });


                                // GetData().GetAwaiter();
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await ahihi.DisplayAlert("Thất bại", "Thêm mẫu " + _SelectedSanPhamAo.TenSP + " thất bại.", "OK").ConfigureAwait(false);
                                });
                            }
                        }
                    });
                }
            }
        }
        private void Done(object obj)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
        private async Task UpdateVatLieu(ChiTietSanPhamModel chiTiet)
        {
            VatLieuModel myVatLieu = await vatLieu.GetById(chiTiet.MaVL).ConfigureAwait(false);
            if (!myVatLieu.IsNhap)
            {
                bool responseVL = await vatLieu.SaveDataAsync(new VatLieuModel
                {
                    AnhMoTa = myVatLieu.AnhMoTa,
                    MaVL = myVatLieu.MaVL,
                    TenVL = myVatLieu.TenVL,
                    DonVi = myVatLieu.DonVi,
                    SoLuongTon = myVatLieu.SoLuongTon - (_soLuong * chiTiet.SoLuong),
                    GiaTien = myVatLieu.GiaTien
                }, "VatLieu", false);
            }
            Console.WriteLine("Done for" + chiTiet.MaVL);
        }
        private async Task UpdateTongTienHoaDon()
        {
            HoaDonModel myHoaDon = new HoaDonModel();
            myHoaDon = await hoaDon.GetById(_maHD);
            //var t2 = Task.Run(async () =>
            //{
            //    mySanPham = await sanPham.GetById(_SelectedSanPhamAo.MaSP);
            //});
            //await Task.WhenAll(t1, t2);

            bool response = await hoaDon.SaveDataAsync(new HoaDonModel
            {
                MaHD = _maHD,
                MaKH = myHoaDon.MaKH,
                NgayLap = myHoaDon.NgayLap,
                NgayTrangTri = myHoaDon.NgayTrangTri,
                NgayThaoDo = myHoaDon.NgayThaoDo,
                TongTien = myHoaDon.TongTien + (_SelectedSanPhamAo.GiaTien * _soLuong)
            }, "HoaDon", false);
        }
        #endregion
    }
}