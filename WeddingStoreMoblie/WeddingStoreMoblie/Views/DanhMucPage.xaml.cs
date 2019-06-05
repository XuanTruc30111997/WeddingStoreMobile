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
    public partial class DanhMucPage : ContentPage
    {
        ViewModels.DanhMucViewModel vm;
        public DanhMucPage(string maNV)
        {
            InitializeComponent();
            vm = new ViewModels.DanhMucViewModel(maNV);
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.GetData();
        }
    }
}