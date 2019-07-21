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
using System.Collections.ObjectModel;

namespace WeddingStoreMoblie.ViewModels
{
    public class VatLieuViewModel : BaseViewModel
    {
        #region Properties
        private ObservableCollection<VatLieuModel> _lstVatLieu { get; set; }
        public ObservableCollection<VatLieuModel> LstVatLieu
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

        private int _num = 0; // Số lượng items đã lấy
        private bool _canLoadMore;

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
        private ObservableCollection<VatLieuModel> _lstAllVatLieu = new ObservableCollection<VatLieuModel>();
        // Danh sách lưu vật liệu sắp xếp theo Option
        private ObservableCollection<VatLieuModel> _lstVatLieuOption = new ObservableCollection<VatLieuModel>();
        #endregion

        #region Services
        private MockVatLieuRepository _vatLieu = new MockVatLieuRepository();
        #endregion

        #region Constructors
        public VatLieuViewModel()
        {
            //GetData().GetAwaiter();
            //selectedOption = "All";

            //OptionClick = new Command(ClickOnOption);
            _canLoadMore = true;
            LstVatLieu = new ObservableCollection<VatLieuModel>();
            LoadMoreDataCommand = new Command<object>(LoadMore, CanLoadMore);
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

        //public Command GetDataCommand
        //{
        //    get => new Command(async () =>
        //      {
        //          await GetData();
        //      });
        //}

        public Command<object> LoadMoreDataCommand { get; set; }
        #endregion

        #region Methods

        private bool CanLoadMore(object obj)
        {
            return _canLoadMore;
        }
        public async Task GetData()
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });

            //LstVatLieu = await _vatLieu.GetDataAsync();
            //_lstAllVatLieu = _lstVatLieu;

            //List<VatLieuModel> myLst = await _vatLieu.GetTenItems(_num);

            //List<VatLieuModel> myLst = _lstAllVatLieu;
            //myLst.AddRange(await _vatLieu.GetTenItems(_num));
            //LstVatLieu = myLst;

            //LstVatLieu.AddRange(await _vatLieu.GetTenItems(_num));
            //OnPropertyChanged(nameof(LstVatLieu));

            LstVatLieu = await _vatLieu.GetTenItems(_num);

            _lstAllVatLieu = _lstVatLieu;
            _num += 10;

            selectedOption = "All";
            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }

        private async void LoadMore(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;

            ObservableCollection<VatLieuModel> myLst = await _vatLieu.GetTenItems(_num);
            if (myLst.Count > 0)
            {
                //LstVatLieu.AddRange(myLst);
                //OnPropertyChanged(nameof(LstVatLieu));

                foreach (var vatLieu in myLst)
                {
                    LstVatLieu.Add(vatLieu);
                    //OnPropertyChanged(nameof(LstVatLieu));
                }

                _num += 10;
            }
            else
                _canLoadMore = false;

            listview.IsBusy = false;
            //SearchByOption();
        }

        //private async Task LoadMore()
        //{
        //    List<VatLieuModel> myLst = await _vatLieu.GetTenItems(_num);
        //    foreach(var vl in myLst)
        //    {
        //        LstVatLieu.Add(vl);
        //        OnPropertyChanged(nameof(LstVatLieu));
        //    }
        //    _num += 10;
        //}

        private void Search()
        {
            if (!String.IsNullOrEmpty(_keyWord)) //nhập tìm kiếm
            {
                LstVatLieu = new ObservableCollection<VatLieuModel>(_lstVatLieuOption.Where(vl => vl.TenVL.ToLower().Contains(_keyWord.ToLower())).ToList());
                //LstVatLieu = _lstVatLieuOption.Where(vl => vl.TenVL.ToLower().Contains(_keyWord.ToLower())).ToList();
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
                    _lstVatLieuOption = new ObservableCollection<VatLieuModel>(_lstAllVatLieu.Where(vl => vl.SoLuongTon > 0).ToList());
                    //_lstVatLieuOption = _lstAllVatLieu.Where(vl => vl.SoLuongTon > 0).ToList();
                    break;
                case "$->$$":
                    _lstVatLieuOption = new ObservableCollection<VatLieuModel>(_lstAllVatLieu.OrderBy(vl => vl.GiaTien).ToList());
                    //_lstVatLieuOption = _lstAllVatLieu.OrderBy(vl => vl.GiaTien).ToList();
                    break;
                case "$$->$":
                    _lstVatLieuOption = new ObservableCollection<VatLieuModel>(_lstAllVatLieu.OrderByDescending(vl => vl.GiaTien).ToList());
                    //_lstVatLieuOption = _lstAllVatLieu.OrderByDescending(vl => vl.GiaTien).ToList();
                    break;
            }
            LstVatLieu = _lstVatLieuOption;
            Search();
        }

        #endregion
    }
}
