using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Models.SystemModels
{
    public class HoaDonModel : BaseModel
    {
        public string MaHD { get; set; }
        public string MaKH { get; set; }
        public DateTime NgayLap { get; set; }
        public DateTime NgayTrangTri { get; set; }
        public DateTime NgayThaoDo { get; set; }
        private float _TongTien { get; set; }
        public float TongTien
        {
            get => _TongTien;
            set
            {
                _TongTien = value;
                OnPropertyChanged();
            }
        }
        //public int TinhTrang { get; set; }
        private int _TinhTrang { get; set; }
        public int TinhTrang { get => _TinhTrang; set { _TinhTrang = value; OnPropertyChanged(); } }
    }
}
