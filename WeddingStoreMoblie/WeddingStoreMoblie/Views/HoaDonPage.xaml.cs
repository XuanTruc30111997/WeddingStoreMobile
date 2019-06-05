using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeddingStoreMoblie.ViewModels;
using WeddingStoreMoblie.Models.AppModels;

namespace WeddingStoreMoblie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HoaDonPage : ContentPage
	{
        HoaDonViewModel vm;
		public HoaDonPage ()
		{
			InitializeComponent ();
            vm = new HoaDonViewModel();
            BindingContext = vm;
        }
    }
}