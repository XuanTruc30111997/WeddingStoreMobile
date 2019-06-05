using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.AppModels
{
    public class ThongTinChiTietHoaDon : List<ThongTinChiTietVatLieu>
    {
        public byte[] Hinh { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien { get; set; }

        // public List<ThongTinChiTietVatLieu> myLstThongTinChiTietVL { get; set; }

        public ThongTinChiTietHoaDon(byte[] hinh, string maSP, string tenSP, int soLuong, double thanhTien, List<ThongTinChiTietVatLieu> lstChiTietVL)
        {
            Hinh = hinh;
            MaSP = maSP;
            TenSP = tenSP;
            SoLuong = soLuong;
            ThanhTien = thanhTien;
            // myLstThongTinChiTietVL = lstChiTietVL;
            this.AddRange(lstChiTietVL);
        }
    }
}
