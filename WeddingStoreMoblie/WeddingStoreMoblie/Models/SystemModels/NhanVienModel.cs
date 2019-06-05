using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.SystemModels
{
    public class NhanVienModel
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string SoDT { get; set; }
        public float Luong { get; set; }
        public byte[] Avatar { get; set; }
    }
}
