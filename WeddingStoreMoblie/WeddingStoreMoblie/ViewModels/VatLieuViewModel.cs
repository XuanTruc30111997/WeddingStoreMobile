using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class VatLieuViewModel : BaseViewModel
    {
        #region Properties
        private List<VatLieuModel> _lstVatLieu { get; set; }
        public List<VatLieuModel> LstVatLieu
        {
            get => _lstVatLieu;
            set
            {
                if (_lstVatLieu != value)
                {
                    _lstVatLieu = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> LstOption
        {
            get => new List<string>
            {
                "All",
                "Còn Hàng",
                "$->$$",
                "$$->$"
            };
        }

        private string _keyWord { get; set; }
        public string keyWord
        {
            get { return _keyWord; }
            set
            {
                if (_keyWord != value)
                {
                    _keyWord = value;
                    OnPropertyChanged();
                    Search();
                }
            }
        }

        private string _selectedOption;
        public string selectedOption
        {
            get { return _selectedOption; }
            set
            {
                if (_selectedOption != value)
                {
                    _selectedOption = value;
                    OnPropertyChanged();
                    SearchByOption();
                }
            }
        }

        private bool isSearching;
        // Danh sách lưu tất cả vật liệu không thay đổi
        private List<VatLieuModel> _lstAllVatLieu = new List<VatLieuModel>();
        // Danh sách lưu vật liệu sắp xếp theo Option
        private List<VatLieuModel> _lstVatLieuOption = new List<VatLieuModel>();
        #endregion

        #region Services
        private MockVatLieuRepository _vatLieu;
        #endregion

        #region Constructors
        public VatLieuViewModel()
        {
            //GetData().GetAwaiter();
            //selectedOption = "All";

            //OptionClick = new Command(ClickOnOption);
        }
        #endregion

        #region Commands
        public Command SearchCommand
        {
            get
            {
                return new Command(Search);
            }
        }
        public Command RefreshCommand
        {
            get => new Command(async () =>
             {
                 await GetData();
             });
        }
        #endregion

        #region Methods

        public async Task GetData()
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });
            _vatLieu = new MockVatLieuRepository();
            //_lstVatLieu = await _vatLieu.GetVatLieu();
            LstVatLieu = await _vatLieu.GetDataAsync();
            _lstAllVatLieu = _lstVatLieu;
            selectedOption = "All";
            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }

        private void Search()
        {
            if (!String.IsNullOrEmpty(_keyWord)) //nhập tìm kiếm
            {
                LstVatLieu = _lstVatLieuOption.Where(vl => vl.TenVL.ToLower().Contains(_keyWord.ToLower())).ToList();
            }
            else
            {
                LstVatLieu = _lstVatLieuOption;
            }
        }

        private void SearchByOption()
        {
            switch (_selectedOption)
            {
                case "All":
                    _lstVatLieuOption = _lstAllVatLieu;
                    break;
                case "Còn Hàng":
                    _lstVatLieuOption = _lstAllVatLieu.Where(vl => vl.SoLuongTon > 0).ToList();
                    break;
                case "$->$$":
                    _lstVatLieuOption = _lstAllVatLieu.OrderBy(vl => vl.GiaTien).ToList();
                    break;
                case "$$->$":
                    _lstVatLieuOption = _lstAllVatLieu.OrderByDescending(vl => vl.GiaTien).ToList();
                    break;
            }
            LstVatLieu = _lstVatLieuOption;
            Search();
        }

        #endregion
    }
}
