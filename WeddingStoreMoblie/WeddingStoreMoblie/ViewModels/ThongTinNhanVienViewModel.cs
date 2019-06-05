using System;
using System.Collections.Generic;
using System.Text;

using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class ThongTinNhanVienViewModel : BaseViewModel
    {
        private string _maNV;
        private List<PhanCongModel> _lstPhanCong { get; set; }
        public List<PhanCongModel> LstPhanCong
        {
            get => _lstPhanCong;
            set
            {
                if (_lstPhanCong != value)
                {
                    _lstPhanCong = value;
                    OnPropertyChanged();
                }
            }
        }

        private NhanVienModel _myNhanVien { get; set; }
        public NhanVienModel myNhanVien
        {
            get => _myNhanVien;
            set
            {
                _myNhanVien = value;
                OnPropertyChanged();
            }
        }

        private string _gioCong { get; set; }
        public string gioCong
        {
            get => _gioCong;
            set
            {
                if (_gioCong != value)
                {
                    _gioCong = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _tongLuong { get; set; }
        public double tongLuong
        {
            get => _tongLuong;
            set
            {
                if (_tongLuong != value)
                {
                    _tongLuong = value;
                    OnPropertyChanged();
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

        public ThongTinNhanVienViewModel(string maNV)
        {
            Nam = DateTime.Now.Year.ToString();
            _SelectedThang = "Tháng " + DateTime.Now.Month;
            _maNV = maNV;
            GetData().GetAwaiter();
            Console.WriteLine("All Done");
        }

        async Task GetData()
        {
            //var taskPhanCong = GetPhanCongAsync();
            var taskPhanCong = Search();
            var taskNhanVien = GetNhanVienAsync();

            await Task.WhenAll(taskNhanVien, taskPhanCong); // wait for all Task complete

            gioCong = TongGioCong();
            tongLuong = TinhLuong(TimeSpan.Parse(gioCong), _myNhanVien.Luong);
        }

        async Task GetNhanVienAsync()
        {
            MockNhanVienRepository nhanVien = new MockNhanVienRepository();
            myNhanVien = await nhanVien.GetById(_maNV);
        }

        async Task GetPhanCongAsync(int thang, int nam)
        {
            MockPhanCongRepository phanCong = new MockPhanCongRepository();
            //LstPhanCong = await phanCong.GetByIdNV(_maNV);
            LstPhanCong = await phanCong.GetByIdNVThangNam(_maNV, thang, nam);
        }

        private string TongGioCong()
        {
            TimeSpan myTime = TimeSpan.Zero;
            foreach (var pc in _lstPhanCong)
            {
                if (pc.ThoiGianDen.HasValue && pc.ThoiGianDen.Value != TimeSpan.Zero
                    && pc.ThoiGianDi.HasValue && pc.ThoiGianDi.Value != TimeSpan.Zero)
                {
                    myTime += pc.ThoiGianDi.Value - pc.ThoiGianDen.Value;
                }
            }
            return myTime.ToString();
        }

        //private string TinhGioCong()
        //{
        //    double gio = 0;
        //    double phut = 0;
        //    string[] thoiGian;
        //    foreach (var nhanvien in LstPhanCong)
        //    {
        //        if (nhanvien.ThoiGianDen != null && nhanvien.ThoiGianDi != null)
        //        {
        //            double gioDen = 0;
        //            double phutDen = 0;
        //            double gioDi = 0;
        //            double phutDi = 0;

        //            //thoiGian = nhanvien.ThoiGianDen.Split(':');
        //            //gioDen = double.Parse(thoiGian[0]);
        //            //phutDen = double.Parse(thoiGian[1]);

        //            //thoiGian = nhanvien.ThoiGianDi.Split(':');
        //            //gioDi = double.Parse(thoiGian[0]);
        //            //phutDi = double.Parse(thoiGian[1]);

        //            //if (phutDi < phutDen)
        //            //{
        //            //    phut += 60 + phutDi - phutDen;
        //            //    gio += gioDi - gioDen - 1;
        //            //}
        //            //else
        //            //{
        //            //    phut += phutDi - phutDen;
        //            //    gio += gioDi - gioDen;
        //            //}
        //        }
        //    }

        //    gio += phut / 60;
        //    phut += phut % 60;

        //    return gio + ":" + phut;
        //}

        private float TinhLuong(TimeSpan? gioCong, float luong)
        {
            if (gioCong.HasValue)
            {
                return (gioCong.Value.Hours * luong) + (gioCong.Value.Minutes * (luong / 60));
            }
            return 0;
        }

        private ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand ?? (_SearchCommand = new Command(async () => await Search()));
            }
        }

        private async Task Search()
        {
            string[] strThang = _SelectedThang.Split(' ');
            int myThang = int.Parse(strThang[1]);

            if (int.TryParse(_Nam, out int myNam))
            {
                await GetPhanCongAsync(myThang, myNam);
            }
        }

        //private double TinhLuong(string gioCong, double luong)
        //{
        //    string[] gc = gioCong.Split(':');
        //    return (luong * double.Parse(gc[0])) + (luong / 60 * double.Parse(gc[1]));
        //}
    }
}
