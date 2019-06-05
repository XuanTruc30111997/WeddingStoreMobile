using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.Interfaces;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Threading.Tasks;
using System.Threading;
using WeddingStoreMoblie.Converters;

namespace WeddingStoreMoblie.MockDatas.MockDataApp
{
    public class MockPhanCongNhanVienRepository
    {
        public async Task<List<PhanCongNhanVien>> GetPhanCongNhanVienByIdHDNgay(string maHD, DateTime ngayTrangTri, DateTime ngayThaoDo)
        {
            MockThongTinNhanVienPhanCongRepository ttNVPC = new MockThongTinNhanVienPhanCongRepository();
            List<PhanCongModel> lstPhanCong = new List<PhanCongModel>(); // Danh sách phân công theo hóa đơn
            List<PhanCongNhanVien> lstPhanCongNhanVien = new List<PhanCongNhanVien>(); // Danh sách phân công nhân viên theo ngày trang trí và ngày tháo dở

            List<ThongTinNhanVienPhanCong> lstTrangTri = await Task.Run(()=>ttNVPC.GetThongTinByIdHDVaNgayTrangTri(maHD,ngayTrangTri));
            List<ThongTinNhanVienPhanCong> lstThaoDo = await Task.Run(()=>ttNVPC.GetThongTinByIdHDVaNgayThaoDo(maHD, ngayThaoDo));

            lstPhanCongNhanVien.Add(new PhanCongNhanVien(maHD,ngayTrangTri,lstTrangTri));
            lstPhanCongNhanVien.Add(new PhanCongNhanVien(maHD, ngayThaoDo, lstThaoDo));

            return lstPhanCongNhanVien;
        }
    }
}
