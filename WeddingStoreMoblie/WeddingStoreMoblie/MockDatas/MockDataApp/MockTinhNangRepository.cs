using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Models.AppModels;

namespace WeddingStoreMoblie.MockDatas.MockDataApp
{
    public class MockTinhNangRepository
    {
        private List<TinhNang> _lstTinhNang;

        public MockTinhNangRepository()
        {
            _lstTinhNang = new List<TinhNang>()
            {
                //new TinhNang{id=1,icon="WeddingStoreMoblie.Images.nhanvien.png",ChucNang="Nhân Viên"},
                //new TinhNang{id=2,icon="WeddingStoreMoblie.Images.hoadon.png",ChucNang="Hóa Đơn"},
                //new TinhNang{id=3,icon="WeddingStoreMoblie.Images.vatlieu.png",ChucNang="Vật Liệu"}

                new TinhNang{id=1,icon="nhanvien.png",ChucNang="Nhân Viên"},
                new TinhNang{id=2,icon="hoadon.png",ChucNang="Hóa Đơn"},
                new TinhNang{id=3,icon="vatlieu.png",ChucNang="Vật Liệu"},
                new TinhNang{id=4,icon="logout.png",ChucNang="Đăng xuất"}
            };
        }

        public List<TinhNang> GetTinhNang()
        {
            return _lstTinhNang;
        }
    }
}
