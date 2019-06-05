using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class ModifyPhanCongPopupViewModel : BaseViewModel
    {
        #region Properties
        private TimeSpan _time { get; set; }
        public TimeSpan Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        private ThongTinNhanVienPhanCong _thongTinNhanVienPhanCong { get; set; }
        public ThongTinNhanVienPhanCong thongTinNhanVienPhanCong
        {
            get => _thongTinNhanVienPhanCong;
            set
            {
                _thongTinNhanVienPhanCong = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Services
        MockPhanCongRepository phanCong = new MockPhanCongRepository();
        #endregion

        #region Constructors
        public ModifyPhanCongPopupViewModel(ThongTinNhanVienPhanCong thongTin)
        {
            thongTinNhanVienPhanCong = thongTin;
            Time = DateTime.Now.TimeOfDay;
        }
        #endregion

        #region Commands
        public Command CheckInCommand
        {
            get => new Command(async () => await CheckIn());
        }

        public Command CheckOutCommand
        {
            get => new Command(async () => await CheckOut());
        }

        public Command DeleteCommand
        {
            get => new Command(Delete);
        }

        public Command DoneCommand
        {
            get => new Command(async () => await PopupNavigation.Instance.PopAsync(true));
        }
        #endregion

        #region Methods
        private async Task CheckIn()
        {
            var current = GetCurrentPage();
            PhanCongModel myPhanCong = await phanCong.GetByIdNVNgay(_thongTinNhanVienPhanCong.MaNV, _thongTinNhanVienPhanCong.Ngay);
            myPhanCong.ThoiGianDen = _time;
            bool response = await phanCong.SaveDataAsync(myPhanCong, "PhanCong", false);
            if (response)
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await current.DisplayAlert("Thành công!!", "CheckIn thành công.", "OK");
                });
            else
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await current.DisplayAlert("Thất bại!!", "CheckIn thất bại.", "OK");
                });
        }

        private async Task CheckOut()
        {
            var current = GetCurrentPage();
            PhanCongModel myPhanCong = await phanCong.GetByIdNVNgay(_thongTinNhanVienPhanCong.MaNV, _thongTinNhanVienPhanCong.Ngay);
            myPhanCong.ThoiGianDi = _time;
            bool response = await phanCong.SaveDataAsync(myPhanCong, "PhanCong", false);
            if (response)
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await current.DisplayAlert("Thành công!!", "CheckOut thành công.", "OK");
                });
            else
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await current.DisplayAlert("Thất bại!!", "CheckOut thất bại.", "OK");
                });
        }

        private void Delete()
        {
            var current = GetCurrentPage();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await current.DisplayAlert("Xóa", "Xóa nhân viên " + _thongTinNhanVienPhanCong.TenNV + " khỏi ngày " + _thongTinNhanVienPhanCong.Ngay + " ?"
                                                    , "Yes", "No").ConfigureAwait(false);
                if (result)
                {
                    PhanCongModel myPhanCong = await phanCong.GetByIdNVNgay(_thongTinNhanVienPhanCong.MaNV, _thongTinNhanVienPhanCong.Ngay);
                    bool response = await phanCong.DeleteDataAsync(myPhanCong, "PhanCong");
                    if (response)
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await current.DisplayAlert("Thành công!!", "Xóa thành công.", "OK");
                        });
                    else
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await current.DisplayAlert("Thất bại!!", "Xóa thành công.", "OK");
                        });
                    await PopupNavigation.Instance.PopAsync(true);
                }
            });
        }
        #endregion
    }
}
