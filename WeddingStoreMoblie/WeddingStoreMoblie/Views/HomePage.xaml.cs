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
	public partial class HomePage : MasterDetailPage
	{
		public HomePage (string maNV)
		{
            InitializeComponent();
            this.Master = new MasterPage(maNV);
            this.Detail = new NavigationPage(new HoaDonPage());
            NavigationPage.SetHasNavigationBar(this, false); //turn off toolbar default
        }
	}
}