using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.AppModels
{
    public class ThongTinNhanVienPhanCong
    {
        public byte[] Avatar { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string SoDT { get; set; }
        public TimeSpan? ThoiGianDen { get; set; }
        public TimeSpan? ThoiGianDi { get; set; }
        public string MaHD { get; set; }
        public DateTime Ngay { get; set; }

        //public ThongTinNhanVienPhanCong(string Avatar, string maNV, string tenNV, string sDT, TimeSpan thoiGianDen, TimeSpan thoiGianDi,string maHD, DateTime ngay)
        //{
        //    Avatar = Avatar;
        //    MaNV = maNV;
        //    TenNV = tenNV;
        //    SoDT = sDT;
        //    ThoiGianDen = thoiGianDen;
        //    ThoiGianDi = thoiGianDi;
        //    MaHD = maHD;
        //    Ngay = ngay;
        //}
    }
}
