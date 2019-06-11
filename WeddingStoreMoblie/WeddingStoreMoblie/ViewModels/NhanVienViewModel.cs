using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.Views;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class NhanVienViewModel : BaseViewModel
    {
        #region Properties
        private List<NhanVienModel> _LstNhanVien { get; set; }
        public List<NhanVienModel> LstNhanVien
        {
            get => _LstNhanVien;
            set
            {
                if (_LstNhanVien != value)
                {
                    _LstNhanVien = value;
                    OnPropertyChanged();
                }
            }
        }

        private NhanVienModel _selectedNV { get; set; }
        public NhanVienModel SelectedNV
        {
            get => _selectedNV;
            set
            {
                _selectedNV = value;
                OnPropertyChanged();
                //if (_selectedNV != null)
                //{
                //    GoToThongTinPage().GetAwaiter();
                //}
                GoToThongTinPage().GetAwaiter();
            }
        }
        #endregion

        #region Services
        #endregion

        #region Constructors
        public NhanVienViewModel()
        {
            //GetData().GetAwaiter();
        }
        #endregion

        #region Commands
        public ICommand NhanVienCommand
        {
            get => new Command(async () =>
            {
                await GoToThongTinPage();
            });
        }
        #endregion

        #region Methods
        public async Task GetData()
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });

            MockNhanVienRepository _nhanVien = new MockNhanVienRepository();
            LstNhanVien = await _nhanVien.GetDataAsync();

            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }
        private async Task GoToThongTinPage()
        {
            var currentPage = GetCurrentPage();
            await currentPage.Navigation.PushAsync(new ThongTinNhanVienPage(_selectedNV.MaNV));
        }
        #endregion
    }
}
