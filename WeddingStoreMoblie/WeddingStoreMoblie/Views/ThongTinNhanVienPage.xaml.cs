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
		//public ThongTinNhanVienPage (NhanVienModel nhanVien)
		//{
		//	InitializeComponent ();
  //          //Title = "Thông tin nhân viên " + nhanVien.TenNV;
  //          Title = "Thông tin nhân viên";
  //          BindingContext = new ViewModels.ThongTinNhanVienViewModel(nhanVien);
		//}

        public ThongTinNhanVienPage(string maNV)
        {
            InitializeComponent();
            Title = "Thông tin nhân viên";
            BindingContext = new ViewModels.ThongTinNhanVienViewModel(maNV);
        }
	}
}