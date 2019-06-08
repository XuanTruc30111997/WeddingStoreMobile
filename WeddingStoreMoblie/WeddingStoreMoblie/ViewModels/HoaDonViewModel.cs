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

        private List<KhachHangModel> _lstKhachHang { get; set; }

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

        public List<string> LstThang
        {
            get => new List<string>
            {
                "Tháng 1",
                "Tháng 2",
                "Tháng 3",
                "Tháng 4",
                "Tháng 5",
                "Tháng 6",
                "Tháng 7",
                "Tháng 8",
                "Tháng 9",
                "Tháng 10",
                "Tháng 11",
                "Tháng 12",
            };
        }
        private string _SelectedThang { get; set; }
        public string SelectedThang
        {
            get => _SelectedThang;
            set
            {
                if (_SelectedThang != value)
                {
                    _SelectedThang = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _Nam { get; set; }
        public string Nam
        {
            get => _Nam;
            set
            {
                _Nam = value;
                OnPropertyChanged();
            }
        }
        public List<string> LstOption
        {
            get => new List<string>
            {
                "Ngày trang trí" ,
                "Ngày tháo dở"
            };
        }

        private string _SelectedOption { get; set; }
        public string SelectedOption
        {
            get => _SelectedOption;
            set
            {
                _SelectedOption = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Services
        private MockHoaDonRepository _hoaDon = new MockHoaDonRepository();
        private MockKhachHangRepository _khachHang = new MockKhachHangRepository();
        #endregion

        #region Constructors
        public HoaDonViewModel()
        {
            Nam = DateTime.Now.Year.ToString();
            SelectedThang = "Tháng " + DateTime.Now.Month.ToString();
            SelectedOption = "Ngày trang trí";

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
        public Command SearchCommand
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
            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = true;
            });

            _myList = new List<HoaDonKhachHang>();

            var t1 = GetHoaDonAsync();
            var t2 = GetKhachHangAsync();

            await Task.WhenAll(t1, t2);
            if (_lstHoaDon != null)
            {
                foreach (var hoaDon in _lstHoaDon)
                {
                    KhachHangModel myKh = _lstKhachHang.Find(kh => kh.MaKH == hoaDon.MaKH);
                    MyList.Add(new HoaDonKhachHang(hoaDon.MaHD, myKh.MaKH, hoaDon.NgayTrangTri, hoaDon.NgayThaoDo, hoaDon.TinhTrang, myKh.TenKH, myKh.SoDT, myKh.DiaChi, hoaDon.TongTien));
                }
                OnPropertyChanged(nameof(MyList));
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = false;
            });
        }

        async Task GetHoaDonAsync()
        {
            if (int.TryParse(Nam, out int myNam))
            {
                string[] strThang = _SelectedThang.Split(' ');
                if (_SelectedOption == "Ngày trang trí")
                {
                    _lstHoaDon = await _hoaDon.GetLstHOaDonByThangNam(int.Parse(strThang[1]), myNam, true);
                }
                else
                {
                    _lstHoaDon = await _hoaDon.GetLstHOaDonByThangNam(int.Parse(strThang[1]), myNam, false);
                }
            }
            else
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var currentPage = GetCurrentPage();
                    await currentPage.DisplayAlert("Lỗi!", "Nằm không đúng định dạng", "OK");
                });
        }

        async Task GetKhachHangAsync()
        {
            _lstKhachHang = await _khachHang.GetDataAsync();
        }
        #endregion
    }
}
