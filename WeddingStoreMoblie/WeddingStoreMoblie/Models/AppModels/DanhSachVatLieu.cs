using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.AppModels
{
    public class DanhSachVatLieu
    {
        public byte[] AnhMoTa { get; set; }
        public string MaVL { get; set; }
        public string TenVL { get; set; }
        public int SoLuong { get; set; }
        public bool IsNhap { get; set; }
    }
}
