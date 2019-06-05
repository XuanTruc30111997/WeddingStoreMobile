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
	public partial class ModifyThongTinMauPopupView : PopupPage
	{
        ViewModels.ModifyThongTinMauPopupViewModel myVm;
        public ModifyThongTinMauPopupView (string maHD)
		{
			InitializeComponent ();
            myVm = new ViewModels.ModifyThongTinMauPopupViewModel(maHD);
            BindingContext = myVm;
		}

        public event EventHandler<bool> CallbackEvent;
        protected override void OnDisappearing() => CallbackEvent?.Invoke(this, true);
        private void InvoiceCalback()
        {
            CallbackEvent?.Invoke(this, true);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            myVm.isBusy = true;
            await myVm.GetData();
            myVm.isBusy = false;
        }
    }
}