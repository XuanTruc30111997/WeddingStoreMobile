using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.AppModels
{
    public class ThongTinChiTietVatLieu
    {
        public byte[] AnhMoTa { get; set; }
        public string TenVL { get; set; }
        public int SoLuong { get; set; }
        public string DonVi { get; set; }
        public double ThanhTien { get; set; }

        public ThongTinChiTietVatLieu(byte[] anhMoTa,string tenVL,int soLuong,string donVi,double thanhTien)
        {
            AnhMoTa = anhMoTa;
            TenVL = tenVL;
            SoLuong = soLuong;
            DonVi = soLuong + " " + donVi;
            ThanhTien = thanhTien;
        }
    }
}
