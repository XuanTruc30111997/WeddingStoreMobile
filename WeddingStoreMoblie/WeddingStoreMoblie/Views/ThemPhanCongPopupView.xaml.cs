using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using WeddingStoreMoblie.Controls;
using WeddingStoreMoblie.Models.AppModels;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemPhanCongPopupView : PopupPage
    {
        public ThemPhanCongPopupView(string maHD)
        {
            InitializeComponent();
            ViewModels.ThemPhanCongPopupViewModel themPhanCongPopupVM = new ViewModels.ThemPhanCongPopupViewModel(maHD);
            BindingContext = themPhanCongPopupVM;
            //RadioButtonBinding(themPhanCongPopupVM);
        }

        public event EventHandler<bool> CallbackEvent;
        protected override void OnDisappearing() => CallbackEvent?.Invoke(this, true);
        private void InvoiceCalback()
        {
            CallbackEvent?.Invoke(this, true);
        }

        void RadioButtonBinding(ViewModels.ThemPhanCongPopupViewModel myVM)
        {
            RadioButtonProperty myRBtnNgayTrangTri = new RadioButtonProperty();
            myRBtnNgayTrangTri.Name = "Ngày Trang Trí: " + myVM.hoaDon.NgayTrangTri;
            myRBtnNgayTrangTri.IsVisible = true;
            myRBtnNgayTrangTri.IsSelected = true;

            RadioButtonProperty myRBtnNgayThaoDo = new RadioButtonProperty();
            myRBtnNgayThaoDo.Name = "Ngày Tháo Dở: " + myVM.hoaDon.NgayThaoDo;
            myRBtnNgayThaoDo.IsVisible = true;
            myRBtnNgayThaoDo.IsSelected = false;

            RadioButton rBtnNgayTrangTri = new RadioButton();
            rBtnNgayTrangTri.BindingContext = myRBtnNgayTrangTri;
            rBtnNgayTrangTri.SetBinding(RadioButton.IsCheckedProperty, "IsSelected", BindingMode.TwoWay);
            rBtnNgayTrangTri.SetBinding(RadioButton.IsVisibleProperty, "IsVisible");
            rBtnNgayTrangTri.SetBinding(RadioButton.TitleProperty, "Name");
            rBtnNgayTrangTri.BorderImageSource = "radioborder";
            rBtnNgayTrangTri.CheckedBackgroundImageSource = "radiocheckedbg";
            rBtnNgayTrangTri.CheckmarkImageSource = "radiocheckmark";

            RadioButton rBtnNgayThaoDo = new RadioButton();
            rBtnNgayThaoDo.BindingContext = myRBtnNgayThaoDo;
            rBtnNgayThaoDo.SetBinding(RadioButton.IsCheckedProperty, "IsSelected", BindingMode.TwoWay);
            rBtnNgayThaoDo.SetBinding(RadioButton.IsVisibleProperty, "IsVisible");
            rBtnNgayThaoDo.SetBinding(RadioButton.TitleProperty, "Name");
            rBtnNgayThaoDo.BorderImageSource = "radioborder";
            rBtnNgayThaoDo.CheckedBackgroundImageSource = "radiocheckedbg";
            rBtnNgayThaoDo.CheckmarkImageSource = "radiocheckmark";

            //stackNgay.Children.Add(rBtnNgayTrangTri);
            //stackNgay.Children.Add(rBtnNgayThaoDo);
        }
    }
}