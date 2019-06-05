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
        MasterPageViewModel vm;
        public MasterPage (string maNV)
		{
            InitializeComponent();
            vm = new MasterPageViewModel(maNV);
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.GetDataAsync();
        }
    }
}