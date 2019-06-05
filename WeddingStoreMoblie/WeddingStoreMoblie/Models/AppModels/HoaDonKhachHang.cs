using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.AppModels
{
    public class HoaDonKhachHang
    {
        public string MaHD { get; set; }
        public string MaKH { get; set; }
        public DateTime NgayTrangTri { get; set; }
        public DateTime NgayThaoDo { get; set; }
        public int TinhTrang { get; set; }
        public string TenKH { get; set; }
        public string SoDT { get; set; }
        public string DiaChi { get; set; }
        public float TongTien { get; set; }

        public HoaDonKhachHang(string maHD, string maKH, DateTime ngayTrangTri, DateTime ngayThaoDo,int tinhTrang, string tenKH, string soDT, string diaChi, float tongTien)
        {
            MaHD = maHD;
            MaKH = maKH;
            NgayTrangTri = ngayTrangTri;
            NgayThaoDo = ngayThaoDo;
            TinhTrang = tinhTrang;
            TenKH = tenKH;
            SoDT = soDT;
            DiaChi = diaChi;
            TongTien = tongTien;
        }
    }
}
