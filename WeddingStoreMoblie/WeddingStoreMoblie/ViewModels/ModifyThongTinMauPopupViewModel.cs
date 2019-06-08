using System;
using System.Collections.Generic;
using System.Text;

using WeddingStoreMoblie.Models.AppModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using WeddingStoreMoblie.Functions;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using System.Linq;

namespace WeddingStoreMoblie.ViewModels
{
    public class ModifyThongTinMauPopupViewModel : BaseViewModel
    {
        #region Properties
        private string _maHD;
        private HoaDonModel _myHoaDon { get; set; }
        private List<SanPhamAo> _lstAo { get; set; }
        private List<ThongTinMau> _lstThongTinMau { get; set; }
        public List<ThongTinMau> LstThongTinMau
        {
            get => _lstThongTinMau;
            set
            {
                if (_lstThongTinMau != value)
                {
                    _lstThongTinMau = value;
                    OnPropertyChanged();
                }
            }
        }

        private ThongTinMau _selectedMau { get; set; }
        public ThongTinMau SelectedMau
        {
            get => _selectedMau;
            set
            {
                if (_selectedMau != value)
                {
                    _selectedMau = value;
                    OnPropertyChanged();
                    if (_selectedMau != null)
                        soLuong = _selectedMau.SoLuong;
                }
            }
        }

        private int _soLuong { get; set; }
        public int soLuong
        {
            get => _soLuong;
            set
            {
                _soLuong = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public ModifyThongTinMauPopupViewModel(string maHD)
        {
            _maHD = maHD;
            //GetData().GetAwaiter();
        }
        #endregion

        #region Services
        MockChiTietHoaDonRepository chiTietHoaDon = new MockChiTietHoaDonRepository();
        MockVatLieuRepository vatLieu = new MockVatLieuRepository();
        MockChiTietSanPhamRepository chiTietSanPham = new MockChiTietSanPhamRepository();
        MockHoaDonRepository hoaDon = new MockHoaDonRepository();
        MockSanPhamRepository sanPham = new MockSanPhamRepository();
        MockKhoVatLieuAoRepository vatLieuAoMock = new MockKhoVatLieuAoRepository();
        #endregion

        #region Commands
        public Command SaveCommand
        {
            get => new Command(async () => await Save());
        }

        public Command DoneCommand
        {
            get => new Command(Done);
        }

        public Command DeleteCommand
        {
            get => new Command(Delete);
        }
        #endregion

        #region Methods
        public async Task GetData()
        {
            var t1 = Task.Run(async () =>
            {
                await GetMau();
            });
            var t2 = Task.Run(async () =>
            {
                await GetThongTinHoaDon();
            });
            SelectedMau = null;
            await Task.WhenAll(t1, t2);
            await GetMauAo();
        }
        async Task GetMau()
        {
            LstThongTinMau = await GetThongTin.getThongTinMau(_maHD);
        }
        async Task GetThongTinHoaDon()
        {
            _myHoaDon = await hoaDon.GetById(_maHD);
        }
        async Task GetMauAo()
        {
            _lstAo = await vatLieuAoMock.GetSanPhamAo(_myHoaDon.NgayTrangTri, _myHoaDon.NgayThaoDo, _myHoaDon.MaHD);
        }
        private async Task Save()
        {
            var ahihi = GetCurrentPage();
            if (_selectedMau != null)
            {
                SanPhamAo mySPAo = _lstAo.FirstOrDefault(sp => sp.MaSP == _selectedMau.MaSP);
                if (mySPAo.AllNhap || (!mySPAo.AllNhap && _soLuong <= mySPAo.SoLuong + _selectedMau.SoLuong))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await ahihi.DisplayAlert("Chỉnh sủa mẫu!", "Chỉnh sửa mẫu " + SelectedMau.TenSP + " số lượng " + SelectedMau.SoLuong + " --> " + soLuong + " ?", "OK", "Cancel");
                        if (result)
                        {
                            Constant.isNewDanhSachVatLieu = true;
                            ChinhSua().GetAwaiter();
                            // Update kho sản phẩm ảo
                            SanPhamAo mySpAo = _lstAo.FirstOrDefault(sp => sp.MaSP == _selectedMau.MaSP);
                            mySPAo.SoLuong = mySPAo.SoLuong + _selectedMau.SoLuong - _soLuong;
                        }
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ahihi.DisplayAlert("Fail!!!", "Save mẫu " + SelectedMau.TenSP + " thất bại. Số lượng tối đa: " + (mySPAo.SoLuong + _selectedMau.SoLuong), "OK");
                    });
                }
            }
            else
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ahihi.DisplayAlert("Fail!!!", "Chọn một mẫu", "OK");
                });
        }

        private void Done(object obj)
        {
            PopupNavigation.Instance.PopAsync(true);
        }

        private void Delete()
        {
            var ahihi = GetCurrentPage();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await ahihi.DisplayAlert("Xóa mẫu?", "Xóa mẫu " + _selectedMau.TenSP + " ?", "OK", "Cancel").ConfigureAwait(false);
                if (result)
                {
                    Constant.isNewDanhSachVatLieu = true;
                    bool response = await chiTietHoaDon.DeleteDataAsync(new ChiTietHoaDonModel
                    {
                        MaHD = _maHD,
                        MaSP = _selectedMau.MaSP,
                        SoLuong = _selectedMau.SoLuong,
                        ThanhTien = _selectedMau.ThanhTien,
                    }, "ChiTietHoaDon").ConfigureAwait(false);
                    if (response)
                    {
                        SanPhamAo mySpAo = _lstAo.FirstOrDefault(spAo => spAo.MaSP == _selectedMau.MaSP);
                        // Update số lượng tồn của vật liệu
                        var t1 = Task.Run(async () =>
                        {
                            if (!mySpAo.AllNhap && _myHoaDon.TinhTrang == 1)
                            {
                                // Lấy danh sách các vật liệu trong chi tiết mẫu
                                List<ChiTietSanPhamModel> lstCTSP = await chiTietSanPham.GetByIdSP(_selectedMau.MaSP);
                                Parallel.ForEach(lstCTSP, async (ctsp) =>
                                {
                                    await UpdateVatLieuAfterDelete(ctsp);
                                });
                            }
                        });
                        // Update tổng tiền hóa đơn
                        var t2 = Task.Run(async () =>
                          {
                              await UpdateTongTienHoaDon(true);
                          });
                        // Update số lượng sản phẩm ảo
                        var t3 = Task.Run(() =>
                        {
                            if (!mySpAo.AllNhap)
                                mySpAo.SoLuong = mySpAo.SoLuong + _selectedMau.SoLuong - _soLuong;
                        });
                        await Task.WhenAll(t1, t2,t3);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await ahihi.DisplayAlert("Thành công!", "Xóa mẫu " + _selectedMau.TenSP + " thành công!", "OK").ConfigureAwait(false);
                            GetMau().GetAwaiter();
                        });
                    }
                    else
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await ahihi.DisplayAlert("Thất bại!", "Xóa mẫu " + _selectedMau.TenSP + " thất bại!", "OK").ConfigureAwait(false);
                        });
                }
            });
        }

        async Task ChinhSua()
        {
            var ahihi = GetCurrentPage();

            MockSanPhamRepository sanPham = new MockSanPhamRepository();
            SanPhamModel mySanPham = await sanPham.GetById(_selectedMau.MaSP).ConfigureAwait(false);
            // Update mẫu trong chi tiết hóa đơn
            bool response = await chiTietHoaDon.SaveDataAsync(new ChiTietHoaDonModel
            {
                MaHD = _maHD,
                MaSP = _selectedMau.MaSP,
                SoLuong = _soLuong,
                ThanhTien = mySanPham.GiaTien * _soLuong,
            }, "ChiTietHoaDon", false).ConfigureAwait(false);
            // Thông báo kết quả
            if (response) // Thành công
            {
                SanPhamAo mySpAo = _lstAo.FirstOrDefault(spAo => spAo.MaSP == _selectedMau.MaSP);
                // Update số lượng tồn của vật liệu
                var t1 = Task.Run(async () =>
                  {
                      if (!mySpAo.AllNhap && _myHoaDon.TinhTrang == 1)
                      {
                          // Lấy danh sách các vật liệu trong chi tiết mẫu
                          List<ChiTietSanPhamModel> lstCTSP = await chiTietSanPham.GetByIdSP(_selectedMau.MaSP);
                          Parallel.ForEach(lstCTSP, async (ctsp) =>
                          {
                              await UpdateVatLieu(ctsp);
                          });
                      }
                  });
                var t2 = Task.Run(async () =>
                  {
                      await UpdateTongTienHoaDon(false);
                  });

                await Task.WhenAll(t1, t2);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ahihi.DisplayAlert("Thành công", "Chỉnh sửa mẫu " + _selectedMau.TenSP + " thành công.", "OK").ConfigureAwait(false);
                });
                GetMau().GetAwaiter();
            }
            else // Thất bại
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ahihi.DisplayAlert("Thất bại", "Chỉnh sửa mẫu " + _selectedMau.TenSP + " thất bại.", "OK").ConfigureAwait(false);
                });
            }
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
                    //SoLuongTon = myVatLieu.SoLuongTon - (_soLuong * chiTiet.SoLuong),
                    SoLuongTon = _soLuong - _selectedMau.SoLuong > 0 ? myVatLieu.SoLuongTon - ((_soLuong - _selectedMau.SoLuong) * chiTiet.SoLuong)
                                                                  : myVatLieu.SoLuongTon + ((_selectedMau.SoLuong - _soLuong) * chiTiet.SoLuong),
                    GiaTien = myVatLieu.GiaTien
                }, "VatLieu", false);
            }
            Console.WriteLine("Done for" + chiTiet.MaVL);
        }

        private async Task UpdateVatLieuAfterDelete(ChiTietSanPhamModel chiTiet)
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
                    // Số lượng tồn mới = số lượng tồn cũ + (số lượng của mẫu trong chi tiết hóa đơn * số lượng của vật liệu trong chi tiết sản phâm)
                    SoLuongTon = myVatLieu.SoLuongTon + (_selectedMau.SoLuong * chiTiet.SoLuong),
                    GiaTien = myVatLieu.GiaTien
                }, "VatLieu", false);
            }
            Console.WriteLine("Done for" + chiTiet.MaVL);
        }

        private async Task UpdateTongTienHoaDon(bool isDelete)
        {
            HoaDonModel myHoaDon = new HoaDonModel();
            SanPhamModel mySanPham = new SanPhamModel();
            var t1 = Task.Run(async () =>
            {
                myHoaDon = await hoaDon.GetById(_maHD);
            });
            var t2 = Task.Run(async () =>
              {

                  mySanPham = await sanPham.GetById(_selectedMau.MaSP);
              });
            await Task.WhenAll(t1, t2);

            if (!isDelete)
            {
                bool response = await hoaDon.SaveDataAsync(new HoaDonModel
                {
                    MaHD = _maHD,
                    MaKH = myHoaDon.MaKH,
                    NgayLap = myHoaDon.NgayLap,
                    NgayTrangTri = myHoaDon.NgayTrangTri,
                    NgayThaoDo = myHoaDon.NgayThaoDo,
                    TongTien = myHoaDon.TongTien - _selectedMau.ThanhTien + (_soLuong * mySanPham.GiaTien)
                }, "HoaDon", false);
            }
            else
            {
                bool response = await hoaDon.SaveDataAsync(new HoaDonModel
                {
                    MaHD = _maHD,
                    MaKH = myHoaDon.MaKH,
                    NgayLap = myHoaDon.NgayLap,
                    NgayTrangTri = myHoaDon.NgayTrangTri,
                    NgayThaoDo = myHoaDon.NgayThaoDo,
                    TongTien = myHoaDon.TongTien - _selectedMau.ThanhTien
                }, "HoaDon", false);
            }
        }
        #endregion
    }
}
