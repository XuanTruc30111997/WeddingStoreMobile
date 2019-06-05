﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThongTinMauPage : ContentPage
    {
        private string _maHD;
        ViewModels.ThongTinMauViewModel myVM;
        public ThongTinMauPage(string maHD)
        {
            InitializeComponent();
            _maHD = maHD;

            myVM = new ViewModels.ThongTinMauViewModel(maHD);
            BindingContext = myVM;

            more.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = myVM.MenuCommand
            });

            them.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = myVM.ThemChiTietCommand
            });

            modify.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = myVM.ModifyCommand
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (myVM.isFirst)
            {
                myVM.isBusy = true;
                await myVM.GetData(_maHD);
                myVM.isBusy = false;
            }
        }
    }
}