using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.AppModels
{
    public class ThongTinMau : BaseModel
    {
        public byte[] Hinh { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        private int _SoLuong { get; set; }
        public int SoLuong { get => _SoLuong; set { _SoLuong = value;OnPropertyChanged(); } }
        private float _ThanhTien { get; set; }
        public float ThanhTien { get => _ThanhTien; set { _ThanhTien = value;OnPropertyChanged(); } }
    }
}
