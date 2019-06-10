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
		public HomePage (string maNV,int request)
		{
            InitializeComponent();
            this.Master = new MasterPage(maNV);
            switch(request)
            {
                case 1: // Hóa đơn
                    this.Detail = new NavigationPage(new NhanVienPage());
                    break;
                case 2: // Nhân viên
                    this.Detail = new NavigationPage(new HoaDonPage());
                    break;
                case 3: // Kho Vật liệu
                    this.Detail = new NavigationPage(new KhoVatLieuPage());
                    break;
                case 4: // Thông tin tài khoản
                    this.Detail = new NavigationPage(new ThongTinTaiKhoanPage(maNV));
                    break;
            }
            NavigationPage.SetHasNavigationBar(this, false); //turn off toolbar default
        }
	}
}