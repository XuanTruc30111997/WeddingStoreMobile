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
	public partial class MasterPage : ContentPage
	{
		public MasterPage ()
		{
			InitializeComponent ();
            BindingContext = new MasterPageViewModel();
        }

        private void ChucNang_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var chucNang = e.Item as TinhNang;
            var _chucNang = new MasterPageViewModel();
            _chucNang.CLickOnChucNang(chucNang.id);
        }
    }
}