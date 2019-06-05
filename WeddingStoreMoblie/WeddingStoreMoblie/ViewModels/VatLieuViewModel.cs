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

        // Danh sách lưu tất cả vật liệu không thay đổi
        private List<VatLieuModel> _lstAllVatLieu = new List<VatLieuModel>();
        // Danh sách lưu vật liệu sắp xếp theo Option
        private List<VatLieuModel> _lstVatLieuOption = new List<VatLieuModel>();

        private bool isSearching;
        private MockVatLieuRepository _vatLieu;

        public VatLieuViewModel()
        {
            //GetData().GetAwaiter();
            //selectedOption = "All";

            //OptionClick = new Command(ClickOnOption);
        }

        public async Task GetData()
        {
            _vatLieu = new MockVatLieuRepository();
            //_lstVatLieu = await _vatLieu.GetVatLieu();
            LstVatLieu = await _vatLieu.GetDataAsync();
            _lstAllVatLieu = _lstVatLieu;
            selectedOption = "All";
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

        //public ICommand OptionClick { get; set; }
        //public void ClickOnOption()
        //{
        //    switch (selectedOption)
        //    {
        //        case "All":
        //            _myLstVatLieuByOption = _lstAllVatLieu;
        //            if (isSearching)
        //            {
        //                Search();
        //            }
        //            else
        //            {
        //                LstVatLieu = _myLstVatLieuByOption;
        //            }
        //            break;
        //        case "Còn Hàng":
        //            _myLstVatLieuByOption = _lstAllVatLieu.Where(vl => vl.SoLuongTon >= 1).ToList();
        //            if (isSearching)
        //            {
        //                Search();
        //            }
        //            else
        //            {
        //                LstVatLieu = _myLstVatLieuByOption;
        //            }
        //            break;
        //        case "$->$$":
        //            _myLstVatLieuByOption = _lstAllVatLieu.OrderBy(vl => vl.GiaTien).ToList();
        //            if (isSearching)
        //            {
        //                Search();
        //            }
        //            else
        //            {
        //                LstVatLieu = _myLstVatLieuByOption;
        //            }
        //            break;
        //        case "$$->$":
        //            _myLstVatLieuByOption = _lstAllVatLieu.OrderByDescending(vl => vl.GiaTien).ToList();
        //            if (isSearching)
        //            {
        //                Search();
        //            }
        //            else
        //            {
        //                LstVatLieu = _myLstVatLieuByOption;
        //            }
        //            break;
        //    }
        //}

        public Command SearchCommand
        {
            get
            {
                return new Command(Search);
            }
        }

        //private void Search()
        //{
        //    if (!string.IsNullOrEmpty(keyWord))
        //    {
        //        LstVatLieu = _myLstVatLieuByOption.Where(vl => vl.TenVL.ToLower().Contains(keyWord.ToLower())).ToList();
        //        isSearching = true;
        //    }
        //    else
        //    {
        //        if (isSearching)
        //        {
        //            LstVatLieu = _myLstVatLieuByOption;
        //            isSearching = false;
        //        }
        //    }
        //}
    }
}
