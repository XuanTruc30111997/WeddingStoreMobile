using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;

namespace WeddingStoreMoblie.ViewModels
{
    public class ThemPhanCongPopupViewModel : BaseViewModel
    {
        #region Properties
        private string _maHD;
        // ngày trang trí
        private bool _checkNTT { get; set; }
        public bool checkNTT
        {
            get => _checkNTT;
            set
            {
                _checkNTT = value;
                OnPropertyChanged();
            }
        }

        // ngày tháo dở
        private bool _checkNTD { get; set; }
        public bool checkNTD
        {
            get => _checkNTD;
            set
            {
                _checkNTD = value;
                OnPropertyChanged();
            }
        }

        // Danh sách nhân viên
        private List<NhanVienModel> _lstNhanVien { get; set; }
        public List<NhanVienModel> LstNhanVien
        {
            get => _lstNhanVien;
            set
            {
                _lstNhanVien = value;
                OnPropertyChanged();
            }
        }

        private NhanVienModel _selectedNV { get; set; }
        public NhanVienModel SelectedNV
        {
            get => _selectedNV;
            set
            {
                if (_selectedNV != value)
                {
                    _selectedNV = value;
                    OnPropertyChanged();
                }
            }
        }

        private HoaDonModel _hoaDon { get; set; }
        public HoaDonModel hoaDon
        {
            get => _hoaDon;
            set
            {
                if (_hoaDon != value)
                {
                    _hoaDon = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Services
        MockNhanVienRepository nhanVien = new MockNhanVienRepository();
        MockHoaDonRepository mockHoaDon = new MockHoaDonRepository();
        MockPhanCongRepository phanCong = new MockPhanCongRepository();
        #endregion

        #region Constructors
        public ThemPhanCongPopupViewModel(string maHD)
        {
            _maHD = maHD;
            GetData().GetAwaiter();
            //var thTH = new System.Globalization.CultureInfo("th-TH");
            //var ntt = hoaDon.NgayThaoDo;
            //var ntd = hoaDon.NgayTrangTri;
            //double ahaha = (ntt - ntd).TotalDays;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new Command(async () => { await Save(); });
            }
        }
        public Command DoneCommand
        {
            get => new Command(async () => await PopupNavigation.Instance.PopAsync(true));
        }
        #endregion

        #region Methods
        async Task GetData()
        {
            // Get NhanVien
            var t1 = Task.Run(async () => LstNhanVien = await nhanVien.GetDataAsync());
            // Get Thong Tin Hoa Don
            var t2 = Task.Run(async () => hoaDon = await mockHoaDon.GetById(_maHD));

            await Task.WhenAll(t1, t2);
        }
        private async Task Save()
        {
            var page = GetCurrentPage();
            if (!checkNTT && !checkNTD)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await page.DisplayAlert("Error!!!", "Chọn một trong hai ngày.", "OK");
                });
            }
            else
            {
                if (SelectedNV == null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await page.DisplayAlert("Error!!!", "Chọn nhân viên.", "OK");
                    });
                }
                else
                {
                    var t1 = Task.Run(async () =>
                      {
                          if (checkNTT)
                          {
                              if (await Functions.Check.CheckThemPhanCong(_maHD, SelectedNV.MaNV, hoaDon.NgayTrangTri))
                              {
                                  bool response = await UpdatePhanCong(_hoaDon.NgayTrangTri);
                                  if (response)
                                      Device.BeginInvokeOnMainThread(async () =>
                                      {
                                          await page.DisplayAlert("SaveCompleted"
                                          , "Thêm nhân viên: " + SelectedNV.TenNV + " vào ngày " + hoaDon.NgayTrangTri.Date + " thành công!!"
                                          , "OK");
                                      });
                                  else
                                  {
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await page.DisplayAlert("Fail!!!!"
                                          , "Thêm nhân viên phân công thất bại."
                                          , "OK");
                                    });
                                  }
                              }
                              else
                              {
                                  Device.BeginInvokeOnMainThread(async () =>
                                  {
                                      await page.DisplayAlert("Fail!!!!"
                                          , "Nhân viên: " + SelectedNV.TenNV + "đã được phân công vào ngày " + hoaDon.NgayTrangTri.Date
                                          , "OK");
                                  });
                              }
                          }
                      });

                    var t2 = Task.Run(async () =>
                      {
                          if (checkNTD)
                          {
                              if (await Functions.Check.CheckThemPhanCong(_maHD, SelectedNV.MaNV, hoaDon.NgayThaoDo))
                              {
                                  bool response = await UpdatePhanCong(_hoaDon.NgayThaoDo);
                                  if (response)
                                  {
                                      Device.BeginInvokeOnMainThread(async () =>
                                      {
                                          await page.DisplayAlert("SaveCompleted"
                                          , "Thêm nhân viên: " + SelectedNV.TenNV + " vào ngày " + hoaDon.NgayThaoDo.Date + "thành công!!"
                                          , "OK");
                                      });
                                  }
                                  else
                                  {
                                      Device.BeginInvokeOnMainThread(async () =>
                                      {
                                          await page.DisplayAlert("Fail!!!!"
                                            , "Thêm nhân viên phân công thất bại."
                                            , "OK");
                                      });
                                  }
                              }
                              else
                              {
                                  Device.BeginInvokeOnMainThread(async () =>
                                  {
                                      await page.DisplayAlert("Fail!!!!"
                                          , "Nhân viên: " + SelectedNV.TenNV + "đã được phân công vào ngày " + hoaDon.NgayThaoDo.Date
                                          , "OK");
                                  });
                              }
                          }
                      });
                }
            }
        }

        private async Task<bool> UpdatePhanCong(DateTime ngay)
        {
            bool reponsePhanCong = await phanCong.SaveDataAsync(new PhanCongModel
            {
                MaHD = _maHD,
                MaNV = _selectedNV.MaNV,
                Ngay = ngay,
                ThoiGianDen = TimeSpan.Zero,
                ThoiGianDi = TimeSpan.Zero
            }, "PhanCong", true);

            return reponsePhanCong;
        }
        #endregion
    }
}
