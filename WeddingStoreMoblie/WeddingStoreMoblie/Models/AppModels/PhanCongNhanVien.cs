using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Converters;

namespace WeddingStoreMoblie.Models.AppModels
{
    public class PhanCongNhanVien : List<ThongTinNhanVienPhanCong>
    {
        public string MaHD { get; set; }
        //public DateTime Ngay { get; set; }
        public string Ngay { get; set; }

        public PhanCongNhanVien(string maHD, DateTime ngay, List<ThongTinNhanVienPhanCong> lstThongTin)
        {
            MaHD = maHD;
            Ngay = ConvertDateTimeToString.ConverToMyDateFormat(ngay);
            //Ngay = ngay;
            this.AddRange(lstThongTin);
        }
    }
}
