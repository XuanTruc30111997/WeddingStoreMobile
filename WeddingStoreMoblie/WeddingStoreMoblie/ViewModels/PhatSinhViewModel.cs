using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.AppModels;
using Xamarin.Forms;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.Functions;
using WeddingStoreMoblie.MockDatas.MockDataApp;

namespace WeddingStoreMoblie.ViewModels
{
    public class PhatSinhViewModel : BaseViewModel
    {
        #region Properties
        private string _maHD;
        private HoaDonModel _myHoaDon;
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

        private ThongTinPhatSinh _selectedVL { get; set; }
        public ThongTinPhatSinh SelectedVL
        {
            get
            {
                return _selectedVL;
            }
            set
            {
                _selectedVL = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Services
        private MockThongTinChiTietHoaDonRepository _thongTinChiTietHD = new MockThongTinChiTietHoaDonRepository();
        MockPhatSinhRepository phatSinh = new MockPhatSinhRepository();
        MockVatLieuRepository vatLieu = new MockVatLieuRepository();
        MockHoaDonRepository hoaDon = new MockHoaDonRepository();
        #endregion

        #region Constructors
        public PhatSinhViewModel(string maHD)
        {
            isFirst = true;
            Constant.isNewPS = false;
            _maHD = maHD;
            //GetData(_maHD).GetAwaiter();
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
        public Command ModifyCommand
        {
            get
            {
                return new Command(Modify);
            }
        }
        public Command DeleteCommand
        {
            get
            {
                return new Command(async () => await Delete());
            }
        }
        #endregion

        #region Methods
        public async Task GetData()
        {
            Device.BeginInvokeOnMainThread(() => {
                isBusy = true;
            });

            var t1 = Task.Run(async () =>
            {
                await GetThongTinHoaDon();
            });
            var t2 = Task.Run(async () =>
            {
                await GetDanhSachPhatSinh();
            });
            await Task.WhenAll(t1, t2);
            isFirst = false;

            Device.BeginInvokeOnMainThread(() => {
                isBusy = false;
            });
        }

        public async Task GetThongTinHoaDon()
        {
            _myHoaDon = await hoaDon.GetById(_maHD);
            Constant.isNewPS = false;
            Console.WriteLine("Tình trạng hóa đơn: " + _myHoaDon.TinhTrang);
        }

        async Task GetDanhSachPhatSinh()
        {
            LstThongTinPhatSinh = await _thongTinChiTietHD.GetThongTinPhatSinhByIdHD(_maHD);
        }

        private void ThemPhatSinh()
        {
            Constant.isNew = true;
            var themPhatSinhPopupView = new Views.ThemPhatSinhPopupView(_maHD);
            themPhatSinhPopupView.CallbackEvent += (object sender, bool e) => GetData().GetAwaiter();
            PopupNavigation.Instance.PushAsync(themPhatSinhPopupView);
        }

        private Task ToPopupPage => PopupNavigation.Instance.PushAsync(new Views.ThemPhatSinhPopupView(_maHD));

        private void Modify()
        {
            Console.WriteLine("Tình trạng hóa đơn: " + _myHoaDon.TinhTrang);
            Constant.isNew = true;
            var modifyPhatSinhPopupView = new Views.ModifyPhatSinhPopupView(_selectedVL, _myHoaDon);
            modifyPhatSinhPopupView.CallbackEvent += (object sender, bool e) => GetData().GetAwaiter();
            PopupNavigation.Instance.PushAsync(modifyPhatSinhPopupView);
        }

        private async Task Delete()
        {
            Constant.isNew = true;
            if (_selectedVL != null)
            {
                var currentPage = GetCurrentPage();
                var result = await currentPage.DisplayAlert("Xóa phát sinh!", "Xóa vật liệu " + _selectedVL.TenVL + " khỏi danh sách phát sinh?", "Yes", "No").ConfigureAwait(false);
                if (result)
                {
                    Constant.isNewDanhSachVatLieu = true;
                    bool response = await phatSinh.DeleteDataAsync(new PhatSinhModel
                    {
                        MaHD = _maHD,
                        MaVL = _selectedVL.MaVL,
                        SoLuong = _selectedVL.SoLuong,
                        ThanhTien = _selectedVL.ThanhTien
                    }, "PhatSinh").ConfigureAwait(false);
                    if (response)
                    {
                        // Update số lượng tồn vật liệu
                        var t1 = Task.Run(async () =>
                        {
                            if (_myHoaDon.TinhTrang == 1 && !_selectedVL.IsNhap)
                                await UpdateVatLieu();
                        });

                        // Update tổng tiền hóa đơn
                        var t2 = Task.Run(async () => await UpdateTongTienHoaDon());
                        await Task.WhenAll(t1, t2);

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await currentPage.DisplayAlert("Thành công!", "Xóa vật liệu " + _selectedVL.TenVL + " thành công.", "OK");
                        });
                    }
                    else
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await currentPage.DisplayAlert("Thất bại!", "Xóa vật liệu " + _selectedVL.TenVL + " thất bại.", "OK");
                        });
                    await GetDanhSachPhatSinh();
                }
            }
        }

        private async Task UpdateVatLieu()
        {
            VatLieuModel myVatLieu = await vatLieu.GetById(_selectedVL.MaVL);
            myVatLieu.SoLuongTon += _selectedVL.SoLuong;
            bool responseVL = await vatLieu.SaveDataAsync(myVatLieu, "VatLieu", false);
        }

        private async Task UpdateTongTienHoaDon()
        {
            HoaDonModel myHoaDon = await hoaDon.GetById(_maHD);
            myHoaDon.TongTien -= _selectedVL.ThanhTien;
            bool responseHD = await hoaDon.SaveDataAsync(myHoaDon, "HoaDon", false);
        }
        #endregion
    }
}
