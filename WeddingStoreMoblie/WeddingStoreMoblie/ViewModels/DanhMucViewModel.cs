using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataApp;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class DanhMucViewModel:BaseViewModel
    {
        #region Properties
        private string _maNV;
        private List<TinhNang> _lstTinhNang;
        public List<TinhNang> LstTinhNang
        {
            get { return _lstTinhNang; }
            set
            {
                _lstTinhNang = value;
                OnPropertyChanged();
            }
        }

        private TinhNang _SelectedTinhNang { get; set; }
        public TinhNang SelectedTinhNang
        {
            get => _SelectedTinhNang;
            set
            {
                _SelectedTinhNang = value;
                OnPropertyChanged();
            }
        }

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

        #endregion

        #region Services
        MockTinhNangRepository tinhNang = new MockTinhNangRepository();
        MockNhanVienRepository nhanVien = new MockNhanVienRepository();
        #endregion

        #region Constructors
        public DanhMucViewModel(string maNV)
        {
            _maNV = maNV;
        }
        #endregion
        public async Task GetData()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = true;
            });
            MyNhanVien = await nhanVien.GetById(_maNV);
            LstTinhNang = tinhNang.GetTinhNang();
            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = false;
            });
        }
    }
}
