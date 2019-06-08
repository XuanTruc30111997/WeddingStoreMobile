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
    public partial class ThemPhatSinhPopupView : PopupPage
    {
        ViewModels.ThemPhatSinhPopupViewModel vm;
        public ThemPhatSinhPopupView(string maHD)
        {
            InitializeComponent();
            vm = new ViewModels.ThemPhatSinhPopupViewModel(maHD);
            BindingContext = vm;

        }
        public event EventHandler<bool> CallbackEvent;
        protected override void OnDisappearing() => CallbackEvent?.Invoke(this, true);
        //private void InvoiceCallback()
        //{
        //    CallbackEvent?.Invoke(this, true);
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.GetData();
        }
    }
}