using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.AppModels
{
    public class ThongTinPhatSinh
    {
        public byte[] AnhMoTa { get; set; }
        public string MaVL { get; set; }
        public string TenVL { get; set; }
        public bool IsNhap { get; set; }
        public string DonVi { get; set; }
        public int SoLuong { get; set; }
        public float ThanhTien { get; set; }

        public ThongTinPhatSinh(byte[] anhMoTa, string maVL, string tenVL,bool isNhap, string donVi, int soLuong, float thanhTien)
        {
            AnhMoTa = anhMoTa;
            MaVL = maVL;
            TenVL = tenVL;
            IsNhap = isNhap;
            DonVi = soLuong + " " + donVi;
            SoLuong = soLuong;
            ThanhTien = thanhTien;
        }
    }
}
