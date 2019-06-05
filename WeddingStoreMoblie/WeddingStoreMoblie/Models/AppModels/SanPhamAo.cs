using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.AppModels
{
    public class SanPhamAo : BaseModel
    {
        public byte[] HinhMoTa { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public float GiaTien { get; set; }
        private int _SoLuong { get; set; }
        public int SoLuong { get => _SoLuong; set { _SoLuong = value; OnPropertyChanged(); } }
        public bool AllNhap { get; set; }
    }
}
