using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.Services;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        private string _UserName { get; set; }
        public string UserName
        {
            get => _UserName;
            set
            {
                _UserName = value;
                OnPropertyChanged();
            }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged(); }
        }

        private List<TaiKhoanModel> _LstTaiKhoan = new List<TaiKhoanModel>();

        #endregion

        #region Services
        MockTaiKhoanRepository taiKhoanMock = new MockTaiKhoanRepository();
        NavigationService _myNavigationService = new NavigationService();
        #endregion

        #region Constructors
        public LoginViewModel()
        {
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () => await Login());
            }
        }
        #endregion

        #region Methods
        async Task GetData()
        {
            _LstTaiKhoan = await taiKhoanMock.GetDataAsync();
        }
        async Task Login()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = true;
            });

            await GetData();

            Device.BeginInvokeOnMainThread(() =>
            {
                isBusy = false;
            });
            var ahihi = CurrentMainPage();
            if (!String.IsNullOrEmpty(_UserName) || !String.IsNullOrEmpty(_Password))
            {
                TaiKhoanModel myTK = new TaiKhoanModel();
                myTK = _LstTaiKhoan.FirstOrDefault(tk => tk.UserName == _UserName && tk.PassWord == _Password);

                if (myTK != null)
                {
                    _myNavigationService.NavigateToMaster(myTK.MaNV,1,null);
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ahihi.DisplayAlert("Thất bại!", "UserName hoặc Password không đúng. Mời nhập lại.", "OK").ConfigureAwait(false);
                    });
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ahihi.DisplayAlert("Thất bại!", "UserName hoặc Password không đúng. Mời nhập lại.", "OK").ConfigureAwait(false);
                });
            }
        }
        #endregion
    }
}
