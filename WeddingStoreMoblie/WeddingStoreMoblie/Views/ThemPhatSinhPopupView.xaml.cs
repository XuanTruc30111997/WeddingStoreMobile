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
		public ThemPhatSinhPopupView (string maHD)
		{
			InitializeComponent ();
            BindingContext = new ViewModels.ThemPhatSinhPopupViewModel(maHD);

		}
        public event EventHandler<bool> CallbackEvent;
        protected override void OnDisappearing() => CallbackEvent?.Invoke(this, true);
        //private void InvoiceCallback()
        //{
        //    CallbackEvent?.Invoke(this, true);
        //}
    }
}