using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyPhatSinhPopupView : PopupPage
	{
		public ModifyPhatSinhPopupView (ThongTinPhatSinh thongTin,HoaDonModel hoaDon)
		{
			InitializeComponent ();
            ViewModels.ModifyPhatSinhPopupViewModel myVM = new ViewModels.ModifyPhatSinhPopupViewModel(thongTin, hoaDon);
            BindingContext = myVM;
		}

        public EventHandler<bool> CallbackEvent;
        protected override void OnDisappearing() => CallbackEvent?.Invoke(this, true);
        private void InvoiceCallback()
        {
            CallbackEvent?.Invoke(this, true);
        }
    }
}