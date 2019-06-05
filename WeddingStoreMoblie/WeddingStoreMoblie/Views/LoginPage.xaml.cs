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
    public partial class LoginPage : ContentPage
    {
        ViewModels.LoginViewModel vm;
        public LoginPage()
        {
            InitializeComponent();
            vm = new ViewModels.LoginViewModel();
            BindingContext = vm;
        }
    }
}