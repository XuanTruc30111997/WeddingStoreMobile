using System;
using System.Collections.Generic;
using System.Text;

using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Collections;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Threading.Tasks;
using WeddingStoreMoblie.Functions;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using System.Linq;

namespace WeddingStoreMoblie.ViewModels
{
    public class ThemPhatSinhPopupViewModel : BaseViewModel
    {
        #region Properties
        private string _maHD;
        private HoaDonModel _myHoaDon;

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

        private List<VatLieuModel> _lstVatLieu;
        public List<VatLieuModel> LstVatLieu
        {
            get { return _lstVatLieu; }
            set
            {
                _lstVatLieu = value;
                OnPropertyChanged();
            }
        }

        private VatLieuModel _selectedVL;
        public VatLieuModel SelectedVL
        {
            get { return _selectedVL; }
            set
            {
                if (_selectedVL != value)
                {
                    _selectedVL = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Constructors
        public ThemPhatSinhPopupViewModel(string maHD)
        {
            _soLuong = 1;
            _maHD = maHD;
            // GetData().GetAwaiter();
        }
        #endregion

        #region Services
        MockPhatSinhRepository phatSinh = new MockPhatSinhRepository();
        MockVatLieuRepository vatLieu = new MockVatLieuRepository();
        MockHoaDonRepository hoaDon = new MockHoaDonRepository();
        MockKhoVatLieuAoRepository khoVatLieuAo = new MockKhoVatLieuAoRepository();
        #endregion

        #region Commands
        public Command ThemCommand
        {
            get
            {
                return new Command(async () => await Them());
            }
        }
        public Command DoneCommand
        {
            get => new Command(() => PopupNavigation.Instance.PopAsync(true).GetAwaiter());
        }
        #endregion

        #region Methods
        public async Task GetData()
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });
            _myHoaDon = await hoaDon.GetById(_maHD);
            // LstVatLieu = await GetThongTin.getLstVatLieu(_maHD);
            LstVatLieu = await khoVatLieuAo.GetVatLieuCan(_myHoaDon.NgayTrangTri, _myHoaDon.NgayThaoDo, _myHoaDon.MaHD);
            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }

        private async Task Them()
        {
            var currentPage = GetCurrentPage();
            if (_selectedVL == null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await currentPage.DisplayAlert("Error!!", "Chọn vật liệu...", "OK").ConfigureAwait(false);
                });
            }
            else
            {
                if (_soLuong == 0 || (_soLuong > _selectedVL.SoLuongTon && !_selectedVL.IsNhap))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await currentPage.DisplayAlert("Error!!", "Số lượng không hợp lệ, số lượng tối đa: " + _selectedVL.SoLuongTon, "OK").ConfigureAwait(false);
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await currentPage.DisplayAlert("Thêm mới!!", "Thêm vật liệu " + _selectedVL.TenVL + " vào hóa đơn?", "OK", "Cancel").ConfigureAwait(false);
                        if (result)
                        {
                            Constant.isNewDanhSachVatLieu = true;
                            Constant.isNewPS = true;
                            List<PhatSinhModel> myLst = await phatSinh.GetByIdHD(_maHD).ConfigureAwait(false);
                            bool response;
                            bool isExist = false;
                            // Thêm vật liệu phát sinh vào hóa đơn
                            foreach (var ps in myLst)
                            {
                                if (ps.MaVL == _selectedVL.MaVL)
                                {
                                    response = await phatSinh.SaveDataAsync(new PhatSinhModel
                                    {
                                        MaHD = _maHD,
                                        MaVL = _selectedVL.MaVL,
                                        SoLuong = _soLuong + ps.SoLuong,
                                        ThanhTien = (_selectedVL.GiaTien * _soLuong) + ps.ThanhTien
                                    }, "PhatSinh", false).ConfigureAwait(false);
                                    isExist = true;
                                    break;
                                }
                            }
                            if (!isExist)
                            {
                                response = await phatSinh.SaveDataAsync(new PhatSinhModel
                                {
                                    MaHD = _maHD,
                                    MaVL = _selectedVL.MaVL,
                                    SoLuong = _soLuong,
                                    ThanhTien = _selectedVL.GiaTien * _soLuong
                                }, "PhatSinh", true).ConfigureAwait(false);
                            }

                            // Update kho vật liệu
                            var t1 = Task.Run(async () =>
                              {
                                  if (_myHoaDon.TinhTrang == 1 && !_selectedVL.IsNhap)
                                  {
                                      await UpdateVatLieu();
                                  }
                              });

                            // Update tổng tiền hóa đơn
                            var t2 = Task.Run(async () =>
                              {
                                  await UpdateTongTienHoaDon();
                              });

                            // Update vật liệu ảo
                            var t3 = Task.Run(() =>
                            {
                                if (!_selectedVL.IsNhap)
                                {
                                    VatLieuModel myVL = _lstVatLieu.FirstOrDefault(vl => vl.MaVL == _selectedVL.MaVL);
                                    myVL.SoLuongTon -= _soLuong;
                                }
                            });

                            await Task.WhenAll(t1, t2, t3);

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await currentPage.DisplayAlert("Thành công!!", "Thêm vật liệu " + _selectedVL.TenVL + " vào danh sách phát sinh thành công", "OK");
                                OnPropertyChanged(nameof(LstVatLieu));
                            });
                            // await GetData();
                        }
                    });
                }
            }
        }

        private async Task UpdateVatLieu()
        {
            VatLieuModel myVatLieu = await vatLieu.GetById(_selectedVL.MaVL).ConfigureAwait(false);
            myVatLieu.SoLuongTon -= _soLuong;
            bool responseVL = await vatLieu.SaveDataAsync(myVatLieu, "VatLieu", false);
        }

        private async Task UpdateTongTienHoaDon()
        {
            HoaDonModel myHoaDon = await hoaDon.GetById(_maHD).ConfigureAwait(false);
            myHoaDon.TongTien += _selectedVL.GiaTien * _soLuong;
            bool responseHD = await hoaDon.SaveDataAsync(myHoaDon, "HoaDon", false);
        }
        #endregion
    }
}
