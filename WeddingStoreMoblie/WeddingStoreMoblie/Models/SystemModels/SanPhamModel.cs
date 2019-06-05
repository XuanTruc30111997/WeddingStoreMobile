using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.SystemModels
{
    public class SanPhamModel
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public byte[] HinhMoTa { get; set; }
        public string DichVu { get; set; }
        public float GiaTien  { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
