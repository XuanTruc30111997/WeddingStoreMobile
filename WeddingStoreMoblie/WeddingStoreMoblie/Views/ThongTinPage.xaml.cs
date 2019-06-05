using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThongTinPage : ContentPage
    {
        ViewModels.ThongTinViewModel vm;
        private string _maHD;
        private string _maKH;
        public ThongTinPage(string maHD, string maKH)
        {
            InitializeComponent();
            Constant.isNew = false;
            _maHD = maHD;
            _maKH = maKH;
            vm = new ViewModels.ThongTinViewModel(maHD, maKH);
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            if (Constant.isNew)
            {
                vm.GetData(_maHD, _maKH).GetAwaiter();
                Constant.isNew = false;
            }
        }
    }
}