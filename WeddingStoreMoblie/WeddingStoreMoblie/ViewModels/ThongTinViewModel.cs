using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Models.AppModels;

using WeddingStoreMoblie.Interfaces;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class ThongTinViewModel : BaseViewModel
    {
        private HoaDonModel _myHD;
        public HoaDonModel MyHD
        {
            get { return _myHD; }
            set
            {
                if (_myHD != value)
                {
                    _myHD = value;
                    OnPropertyChanged();
                }
            }
        }

        private KhachHangModel _myKH;
        public KhachHangModel MyKH
        {
            get { return _myKH; }
            set
            {
                if (_myKH != value)
                {
                    _myKH = value;
                    OnPropertyChanged();
                }
            }
        }

        private MockHoaDonRepository _hoaDon;
        private MockKhachHangRepository _khachHang;

        public ThongTinViewModel(string maHD, string maKH)
        {
            GetData(maHD, maKH).GetAwaiter();
        }

        public async Task GetData(string maHD, string maKH)
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });
            var t1 = Task.Run(async () =>
              {
                  _hoaDon = new MockHoaDonRepository();
                  MyHD = await _hoaDon.GetById(maHD);
              });
            var t2 = Task.Run(async () =>
            {
                _khachHang = new MockKhachHangRepository();
                MyKH = await _khachHang.GetById(maKH);
            });
            await Task.WhenAll(t1, t2);
            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }
    }
}
