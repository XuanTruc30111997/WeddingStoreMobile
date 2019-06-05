using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.SystemModels
{
    public class VatLieuModel
    {
        public string MaVL { get; set; }
        public string TenVL { get; set; }
        public int SoLuongTon { get; set; }
        public string DonVi { get; set; }
        public float GiaTien { get; set; }
        public byte[] AnhMoTa { get; set; }
        public bool IsNhap { get; set; }
    }
}
