using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.SystemModels
{
    public class PhanCongModel
    {
        public string MaNV { get; set; }
        public string MaHD { get; set; }
        public DateTime Ngay { get; set; }
        public Nullable<TimeSpan> ThoiGianDen { get; set; }
        public Nullable<TimeSpan> ThoiGianDi { get; set; }
    }
}
