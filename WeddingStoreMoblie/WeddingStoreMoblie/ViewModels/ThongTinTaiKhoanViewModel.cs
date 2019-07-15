using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.Services;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class ThongTinTaiKhoanViewModel : BaseViewModel
    {
        #region Properties
        private string _maNV { get; set; }
        private NhanVienModel _MyNhanVien { get; set; }
        public NhanVienModel MyNhanVien
        {
            get => _MyNhanVien;
            set
            {
                _MyNhanVien = value;
                OnPropertyChanged();
            }
        }

        private TaiKhoanModel _MyTaiKhoan { get; set; }
        public TaiKhoanModel MyTaiKhoan
        {
            get => _MyTaiKhoan;
            set
            {
                _MyTaiKhoan = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Services
        MockNhanVienRepository nhanVienMock = new MockNhanVienRepository();
        MockTaiKhoanRepository taiKhoanMock = new MockTaiKhoanRepository();
        NavigationService myNavigation = new NavigationService();
        #endregion

        #region Constructors
        public ThongTinTaiKhoanViewModel(string maNV)
        {
            _maNV = maNV;
        }
        #endregion

        #region Commands
        public Command LuuCommand
        {
            get => new Command(async () => { await Luu(); });
        }

        public Command HuyCommand
        {
            get => new Command(async () => { await Huy(); });
        }

        public Command LogoutCommand
        {
            get => new Command(Logout);
        }
        #endregion

        #region Methods
        public async Task GetDataAsync()
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });
            var t1 = Task.Run(async () =>
            {
                MyNhanVien = await nhanVienMock.GetById(_maNV);
            });

            var t2 = Task.Run(async () =>
            {
                MyTaiKhoan = await taiKhoanMock.GetByIdNhanVien(_maNV);
            });

            await Task.WhenAll(t1, t2);
            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }

        async Task Luu()
        {
            var currentPage = GetCurrentPage();
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool result = await currentPage.DisplayAlert("Chỉnh sửa tài khoản?", "Bạn muốn thay đổi UserName và Password", "Yes", "No").ConfigureAwait(false);
                if (result)
                {
                    List<TaiKhoanModel> lstTK = await taiKhoanMock.GetDataAsync().ConfigureAwait(false);
                    bool flag = false;
                    foreach (var tk in lstTK)
                    {
                        if (tk.MaNV != _MyTaiKhoan.MaNV && tk.UserName == _MyTaiKhoan.UserName)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await currentPage.DisplayAlert("Thất bại!", "Thay đổi thông tin tài khoản thất bại. UserName đã được sử dụng", "OK").ConfigureAwait(false);
                        });
                    }
                    else
                    {
                        bool response = await taiKhoanMock.SaveDataAsync(_MyTaiKhoan, "TaiKhoan", false);
                        if (response)
                        {
                            MyTaiKhoan = await taiKhoanMock.GetByIdNhanVien(_maNV);
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await currentPage.DisplayAlert("Thành công", "Thay đổi thông tin tài khoản thành công", "OK").ConfigureAwait(false);
                            });
                        }
                        else
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await currentPage.DisplayAlert("Thất bại!", "Thay đổi thông tin tài khoản thất bại", "OK").ConfigureAwait(false);
                            });
                    }
                }
            });
        }

        async Task Huy()
        {
            MyTaiKhoan = await taiKhoanMock.GetByIdNhanVien(_maNV);
        }

        void Logout()
        {
            var currentPage = GetCurrentPage();
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool result = await currentPage.DisplayAlert("Đăng Xuất?", "Bạn muốn thoát tài khoản???", "Yes", "No").ConfigureAwait(false);
                if (result)
                {
                    myNavigation.NavigateToMaster(null, 3, null);
                }
            });
        }
        #endregion
    }
}
