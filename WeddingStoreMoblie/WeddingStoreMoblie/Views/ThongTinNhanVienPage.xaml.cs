using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.SystemModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ThongTinNhanVienPage : ContentPage
	{
        ViewModels.ThongTinNhanVienViewModel vm;
        public ThongTinNhanVienPage(string maNV)
        {
            InitializeComponent();
            Title = "Thông tin nhân viên";
            vm = new ViewModels.ThongTinNhanVienViewModel(maNV);
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.GetData();
        }
    }
}