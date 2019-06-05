using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.Converters;

namespace WeddingStoreMoblie.MockDatas.MockDataApp
{
    public class MockThongTinNhanVienPhanCongRepository
    {
        public async Task<List<ThongTinNhanVienPhanCong>> GetThongTinByIdHDVaNgayTrangTri(string maHD, DateTime ngayTrangTri)
        {
            MockNhanVienRepository nhanVien = new MockNhanVienRepository();
            List<ThongTinNhanVienPhanCong> myLst = new List<ThongTinNhanVienPhanCong>();
            MockPhanCongRepository phanCong = new MockPhanCongRepository();

            //Task<List<NhanVienModel>> taskNhanVien = nhanVien.GetDataAsync();
            //Task<List<PhanCongModel>> taskPhanCong = phanCong.GetByIdHdVaNgay(maHD, ngayTrangTri);

            List<PhanCongModel> lstPhanCong = await Task.Run(() => phanCong.GetByIdHdVaNgay(maHD, ngayTrangTri));
            List<NhanVienModel> lstNhanVien = await Task.Run(() => nhanVien.GetDataAsync());

            foreach (var pc in lstPhanCong)
            {
                NhanVienModel myNV = lstNhanVien.FirstOrDefault(nv => nv.MaNV == pc.MaNV);
                myLst.Add(new ThongTinNhanVienPhanCong
                {
                    Avatar = myNV.Avatar,
                    MaNV = myNV.MaNV,
                    TenNV = myNV.TenNV,
                    SoDT = myNV.SoDT,
                    MaHD = maHD,
                    Ngay = ngayTrangTri,
                    ThoiGianDen = pc.ThoiGianDen,
                    ThoiGianDi = pc.ThoiGianDi
                });
            }
            return myLst;
        }

        public async Task<List<ThongTinNhanVienPhanCong>> GetThongTinByIdHDVaNgayThaoDo(string maHD, DateTime ngayThaoDo)
        {
            MockNhanVienRepository nhanVien = new MockNhanVienRepository();
            List<ThongTinNhanVienPhanCong> myLst = new List<ThongTinNhanVienPhanCong>();
            MockPhanCongRepository phanCong = new MockPhanCongRepository();

            //Task<List<NhanVienModel>> taskNhanVien = nhanVien.GetDataAsync();
            //Task<List<PhanCongModel>> taskPhanCong = phanCong.GetByIdHdVaNgay(maHD, ngayThaoDo);

            List<PhanCongModel> lstPhanCong = await Task.Run(() => phanCong.GetByIdHdVaNgay(maHD, ngayThaoDo));
            List<NhanVienModel> lstNhanVien = await Task.Run(() => nhanVien.GetDataAsync());

            foreach (var pc in lstPhanCong)
            {
                NhanVienModel myNV = lstNhanVien.FirstOrDefault(nv => nv.MaNV == pc.MaNV);
                myLst.Add(new ThongTinNhanVienPhanCong
                {
                    Avatar = myNV.Avatar,
                    MaNV = myNV.MaNV,
                    TenNV = myNV.TenNV,
                    SoDT = myNV.SoDT,
                    MaHD = maHD,
                    Ngay = ngayThaoDo,
                    ThoiGianDen = pc.ThoiGianDen,
                    ThoiGianDi = pc.ThoiGianDi
                });
            }
            return myLst;
        }
    }
}
