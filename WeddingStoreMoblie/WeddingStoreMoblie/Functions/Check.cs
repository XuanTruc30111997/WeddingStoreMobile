using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Threading.Tasks;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.SystemModels;

namespace WeddingStoreMoblie.Functions
{
    public class Check
    {
        public static async Task<bool> CheckThemPhanCong(string maHD, string maNV, DateTime ngay)
        {
            MockPhanCongRepository phanCong = new MockPhanCongRepository();
            List<PhanCongModel> lstPhanCong = await phanCong.GetDataAsync();
            // Kiểm tra nhân viên đã được phân công vào hóa đơn chưa.
            PhanCongModel myPhanCong = lstPhanCong.FirstOrDefault(pc => pc.MaHD == maHD && pc.Ngay == ngay && pc.MaNV == maNV);
            if (myPhanCong != null) // đã xuất hiện --> không hợp lệ
                return false;
            return true; // chưa xuất hiện --> hợp lệ
        }

        public static async Task<int> CheckMau(string maHD, string MaSP)
        {
            int maxSoLuong = int.MaxValue;
            MockChiTietSanPhamRepository chiTietMau = new MockChiTietSanPhamRepository();

            // Danh sách tất cả vật liệu có trong hóa đơn
            //List<VatLieuModel> lstVatLieu = await GetThongTin.getLstVatLieu(maHD);
            List<VatLieuModel> lstVatLieu = new List<VatLieuModel>();
            // Danh sách chi tiết vật liệu trong sản phẩm
            //List<ChiTietSanPhamModel> lstChiTietMau = await chiTietMau.GetByIdSP(MaSP);
            List<ChiTietSanPhamModel> lstChiTietMau = new List<ChiTietSanPhamModel>();
            var t1 = Task.Run(async () =>
            {
                lstVatLieu = await GetThongTin.getLstVatLieu(maHD);
            });
            var t2 = Task.Run(async () =>
            {
                lstChiTietMau = await chiTietMau.GetByIdSP(MaSP);
            });
            await Task.WhenAll(t1, t2);

            foreach (var ctm in lstChiTietMau)
            {
                foreach (var vl in lstVatLieu)
                {
                    if (ctm.MaVL == vl.MaVL)
                    {
                        if ((vl.SoLuongTon / ctm.SoLuong) == 0)
                            return 0;
                        else if ((vl.SoLuongTon / ctm.SoLuong) < maxSoLuong)
                            maxSoLuong = vl.SoLuongTon / ctm.SoLuong;

                        break;
                    }
                }
            }
            return maxSoLuong;
        }

        public static async Task<int> CheckMauAo(List<VatLieuModel> lstVatLieu,string maSP)
        {
            MockChiTietSanPhamRepository chiTietSPMock = new MockChiTietSanPhamRepository();
            MockVatLieuRepository vatLieuMock = new MockVatLieuRepository();
            int maxSoLuong = int.MaxValue;
            bool AllNhap = true;
            List<ChiTietSanPhamModel> lstChiTiet = await chiTietSPMock.GetByIdSP(maSP);

            foreach (var ct in lstChiTiet)
            {
                foreach (var vl in lstVatLieu)
                {
                    if(!vl.IsNhap)
                    {
                        if(ct.MaVL==vl.MaVL)
                        {
                            if (vl.SoLuongTon / ct.SoLuong == 0)
                                return 0;
                            else if ((vl.SoLuongTon / ct.SoLuong) < maxSoLuong)
                            {
                                AllNhap = false;
                                maxSoLuong = vl.SoLuongTon / ct.SoLuong;
                                break;
                            }
                        }
                    }
                }
            }

            return AllNhap ? -1 : maxSoLuong;
        }

        public static int CheckVatLieuAo(List<VatLieuModel> lstVatLieu, string maVL)
        {
            return lstVatLieu.FirstOrDefault(vl => vl.MaVL == maVL).SoLuongTon;
        }

        public static async Task<bool> checkAllNhap(SanPhamModel sanPham)
        {
            MockChiTietSanPhamRepository CTSPMock = new MockChiTietSanPhamRepository();
            MockVatLieuRepository vatLieuMock = new MockVatLieuRepository();
            List<ChiTietSanPhamModel> lstChiTiet = await CTSPMock.GetByIdSP(sanPham.MaSP);
            foreach(var ct in lstChiTiet)
            {
                VatLieuModel myVL = await vatLieuMock.GetById(ct.MaVL);
                if (!myVL.IsNhap)
                    return false;
            }
            return true;
        }
    }
}
