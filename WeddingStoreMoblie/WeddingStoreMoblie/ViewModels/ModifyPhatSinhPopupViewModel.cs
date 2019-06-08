using System;
using System.Collections.Generic;
using System.Text;

using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using Xamarin.Forms;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using System.Linq;

namespace WeddingStoreMoblie.ViewModels
{
    public class ModifyPhatSinhPopupViewModel : BaseViewModel
    {
        #region Properties
        private HoaDonModel _myHoaDon = new HoaDonModel();
        private ThongTinPhatSinh _thongTinPhatSinh { get; set; }
        public ThongTinPhatSinh thongTinPhatSinh
        {
            get => _thongTinPhatSinh;
            set
            {
                _thongTinPhatSinh = value;
                OnPropertyChanged();
            }
        }

        public VatLieuModel myVL = new VatLieuModel();

        private int _soLuong { get; set; }
        public int soLuong
        {
            get => _soLuong;
            set
            {
                if (_soLuong != value)
                {
                    _soLuong = value;
                    OnPropertyChanged();
                    tongTien = myVL.GiaTien * _soLuong;
                    OnPropertyChanged("tongTien");
                }
            }
        }

        private float _tongTien { get; set; }
        public float tongTien
        {
            get => _tongTien;
            set
            {
                if (_tongTien != value)
                {
                    _tongTien = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Services
        MockVatLieuRepository vatLieu = new MockVatLieuRepository();
        MockPhatSinhRepository phatSinh = new MockPhatSinhRepository();
        MockHoaDonRepository hoaDon = new MockHoaDonRepository();

        MockKhoVatLieuAoRepository vatLieuAoMock = new MockKhoVatLieuAoRepository();
        #endregion

        #region Constructors
        public ModifyPhatSinhPopupViewModel(ThongTinPhatSinh thongTin, HoaDonModel myHoaDon)
        {
            _myHoaDon = myHoaDon;
            thongTinPhatSinh = thongTin;
            _soLuong = 1;
            GetData().GetAwaiter();
        }
        #endregion

        #region Commands
        public Command SaveCommand
        {
            get => new Command(async () => await Save());
        }
        public Command DoneCommand
        {
            get => new Command(async () => await PopupNavigation.Instance.PopAsync(true));
        }
        #endregion

        #region Methods
        async Task GetData()
        {
            if (!_thongTinPhatSinh.IsNhap)
            {
                List<VatLieuModel> lstVatLieuAo = await vatLieuAoMock.GetVatLieuCan(_myHoaDon.NgayTrangTri, _myHoaDon.NgayThaoDo, _myHoaDon.MaHD);
                myVL = lstVatLieuAo.FirstOrDefault(vl => vl.MaVL == _thongTinPhatSinh.MaVL);
            }
            else
                myVL = await vatLieu.GetById(_thongTinPhatSinh.MaVL);
            tongTien = myVL.GiaTien * _soLuong;
        }

        private async Task Save()
        {
            var currentPage = GetCurrentPage();
            if (_soLuong <= myVL.SoLuongTon + _thongTinPhatSinh.SoLuong)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    bool result = await currentPage.DisplayAlert("Chỉnh sửa!!", "Chỉnh sửa " + _thongTinPhatSinh.TenVL + " số lượng: " + _thongTinPhatSinh.SoLuong + "--> " + _soLuong, "OK", "Cancel").ConfigureAwait(false);
                    if (result)
                    {
                        Constant.isNewDanhSachVatLieu = true;
                        bool response = await phatSinh.SaveDataAsync(new PhatSinhModel
                        {
                            MaHD = _myHoaDon.MaHD,
                            MaVL = _thongTinPhatSinh.MaVL,
                            SoLuong = _soLuong,
                            ThanhTien = myVL.GiaTien * _soLuong
                        }, "PhatSinh", false);
                        if (response)
                        {
                            // Update Kho vật liệu
                            var t1 = Task.Run(async () =>
                            {
                                if (_myHoaDon.TinhTrang == 1 && !myVL.IsNhap)
                                    await UpdateVatLieu();
                            });
                            var t2 = Task.Run(async () => await UpdateTongTienHoaDon());

                            await Task.WhenAll(t1, t2);

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await currentPage.DisplayAlert("Thành công!!", "Chỉnh sửa số lượng vật liệu " + _thongTinPhatSinh.TenVL + " thành công.", "OK").ConfigureAwait(false);
                            });

                            await GetData();
                        }
                        else
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await currentPage.DisplayAlert("Thất bại!!", "Chỉnh sửa số lượng vật liệu " + _thongTinPhatSinh.TenVL + " thất bại.", "OK").ConfigureAwait(false);
                            });
                        await PopupNavigation.Instance.PopAsync(true);
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await currentPage.DisplayAlert("Thất bại!!", "Số lượng vật liệu tối đa " + (_thongTinPhatSinh.SoLuong + myVL.SoLuongTon), "OK");
                });
            }
        }

        private async Task UpdateVatLieu()
        {
            VatLieuModel myVatLieu = await vatLieu.GetById(_thongTinPhatSinh.MaVL).ConfigureAwait(false);
            myVatLieu.SoLuongTon = (_soLuong > _thongTinPhatSinh.SoLuong) ? myVatLieu.SoLuongTon - (_soLuong - _thongTinPhatSinh.SoLuong)
                                                                            : myVatLieu.SoLuongTon + (_thongTinPhatSinh.SoLuong - _soLuong);
            bool responseVL = await vatLieu.SaveDataAsync(myVatLieu, "VatLieu", false);
        }

        private async Task UpdateTongTienHoaDon()
        {
            HoaDonModel myHoaDon = new HoaDonModel();
            VatLieuModel myVatLieu = new VatLieuModel();

            var t1 = Task.Run(async () => myHoaDon = await hoaDon.GetById(_myHoaDon.MaHD));
            var t2 = Task.Run(async () => myVatLieu = await vatLieu.GetById(_thongTinPhatSinh.MaVL));
            await Task.WhenAll(t1, t2);

            //myHoaDon.TongTien -= (_thongTinPhatSinh.ThanhTien + (_soLuong * myVatLieu.GiaTien));
            myHoaDon.TongTien = myHoaDon.TongTien - _thongTinPhatSinh.ThanhTien + _tongTien;
            bool responsehoaDon = await hoaDon.SaveDataAsync(myHoaDon, "HoaDon", false);
        }
        #endregion
    }
}
