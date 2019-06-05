using Rg.Plugins.Popup.Pages;
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
    public partial class ThemChiTietHDPopupView : PopupPage
    {
        ViewModels.ThemChiTietHDPopupViewModel vm;
        public ThemChiTietHDPopupView(string maHD)
        {
            InitializeComponent();
            vm = new ViewModels.ThemChiTietHDPopupViewModel(maHD);
            BindingContext = vm;
        }

        public event EventHandler<bool> CallbackEvent;
        protected override void OnDisappearing() => CallbackEvent?.Invoke(this, true);
        private void InvoiceCalback()
        {
            CallbackEvent?.Invoke(this, true);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm.isBusy = true;
            await vm.GetData();
            vm.isBusy = false;
        }
    }
}