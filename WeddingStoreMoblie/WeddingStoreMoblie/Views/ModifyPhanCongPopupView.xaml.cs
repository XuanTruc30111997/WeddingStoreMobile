using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.AppModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyPhanCongPopupView : PopupPage
    {
		public ModifyPhanCongPopupView (ThongTinNhanVienPhanCong thongTinNVPC)
		{
			InitializeComponent ();
            BindingContext = new ViewModels.ModifyPhanCongPopupViewModel(thongTinNVPC);
		}

        public event EventHandler<bool> CallbackEvent;
        protected override void OnDisappearing() => CallbackEvent?.Invoke(this, true);
        private void InvoiceCalback()
        {
            CallbackEvent?.Invoke(this, true);
        }
    }
}