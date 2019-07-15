using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Threading.Tasks;
using System.Linq;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Functions;

namespace WeddingStoreMoblie.MockDatas.MockDataApp
{
    public class MockKhoVatLieuAoRepository
    {
        public async Task<List<VatLieuModel>> GetVatLieuCan(DateTime ntt, DateTime ntd, string maHD)
        {
            MockVatLieuRepository vatLieuMock = new MockVatLieuRepository();
            MockHoaDonRepository hoaDonMock = new MockHoaDonRepository();
            MockChiTietHoaDonRepository chiTietHDMock = new MockChiTietHoaDonRepository();
            MockChiTietSanPhamRepository chiTietSPMock = new MockChiTietSanPhamRepository();
            MockPhatSinhRepository phatSinhMock = new MockPhatSinhRepository();

            List<VatLieuModel> lstVatLieu = new List<VatLieuModel>();

            var t1 = Task.Run(async () =>
            {
                lstVatLieu = await vatLieuMock.GetDataAsync();
                //foreach (var vl in lstVatLieu)
                //{
                //    myLst.Add(new KhoVatLieuAoModel
                //    {
                //        MaVL = vl.MaVL,
                //        TenVL = vl.TenVL,
                //        SoLuong = vl.SoLuongTon,
                //        GiaTien = vl.GiaTien,
                //        IsNhap = vl.IsNhap
                //    });
                //}
            });
            List<HoaDonModel> lstHoaDon = new List<HoaDonModel>();
            var t2 = Task.Run(async () =>
            {
                lstHoaDon = await hoaDonMock.GetHoaDonPhuHop(maHD);
            });

            await Task.WhenAll(t1, t2);

            foreach (var hd in lstHoaDon)
            {
                bool flag = false;
                if (hd.TinhTrang == 1 && hd.NgayThaoDo < ntt && ntt.Subtract(hd.NgayThaoDo).TotalDays > 3)
                    flag = true;
                else
                {
                    if (hd.TinhTrang == 0 && hd.NgayTrangTri <= ntt && ntt <= hd.NgayThaoDo || hd.NgayTrangTri <= ntd && ntd <= hd.NgayThaoDo
                               || ntt <= hd.NgayTrangTri && hd.NgayThaoDo <= ntd
                               || hd.NgayTrangTri <= ntt && hd.NgayThaoDo >= ntd
                               || (ntt >= hd.NgayThaoDo && ntt.Subtract(hd.NgayThaoDo).TotalDays <= 3)
                               || (ntd <= hd.NgayTrangTri && hd.NgayTrangTri.Subtract(ntd).TotalDays <= 3))
                        flag = true;
                }
                if (flag)
                {
                    List<ChiTietHoaDonModel> lstCTHD = await chiTietHDMock.GetByIdHD(hd.MaHD);
                    foreach (var cthd in lstCTHD)
                    {
                        List<ChiTietSanPhamModel> lstCTSP = await chiTietSPMock.GetByIdSP(cthd.MaSP);
                        foreach (var ctsp in lstCTSP)
                        {
                            VatLieuModel myVL = lstVatLieu.FirstOrDefault(vl => vl.MaVL == ctsp.MaVL);
                            if (hd.TinhTrang == 1)
                                myVL.SoLuongTon += (ctsp.SoLuong * cthd.SoLuong);
                            else
                                myVL.SoLuongTon -= (ctsp.SoLuong * cthd.SoLuong);
                        }
                    }

                    List<PhatSinhModel> lstPhatSinh = await phatSinhMock.GetByIdHD(hd.MaHD);
                    foreach (var ps in lstPhatSinh)
                    {
                        VatLieuModel myVL = lstVatLieu.FirstOrDefault(vl => vl.MaVL == ps.MaVL);
                        if (hd.TinhTrang == 1)
                            myVL.SoLuongTon += ps.SoLuong;
                        else
                            myVL.SoLuongTon -= ps.SoLuong;
                    }
                }
            }
            return lstVatLieu;
        }

        public async Task<List<SanPhamAo>> GetSanPhamAo(DateTime ntt, DateTime ntd, string maHD)
        {
            List<VatLieuModel> lstVatLieuAo = new List<VatLieuModel>();
            List<SanPhamAo> lstAo = new List<SanPhamAo>();
            var t1 = Task.Run(async () =>
            {
                lstVatLieuAo = await GetVatLieuCan(ntt, ntd, maHD);
            });

            MockSanPhamRepository sanPham = new MockSanPhamRepository();
            List<SanPhamModel> lstSanPham = new List<SanPhamModel>();
            var t2 = Task.Run(async () =>
            {
                lstSanPham = await sanPham.GetDataAsync();
            });

            await Task.WhenAll(t1, t2);

            MockChiTietSanPhamRepository CTSPMock = new MockChiTietSanPhamRepository();

            foreach (var sp in lstSanPham)
            {
                if (await Check.checkAllNhap(sp))
                {
                    lstAo.Add(new SanPhamAo
                    {
                        HinhMoTa = sp.HinhMoTa,
                        MaSP = sp.MaSP,
                        TenSP = sp.TenSP,
                        AllNhap = true,
                        GiaTien = sp.GiaTien,
                        SoLuong = 0
                    });
                }
                else
                {
                    int max = int.MaxValue;
                    List<ChiTietSanPhamModel> lstCTSP = await CTSPMock.GetByIdSP(sp.MaSP);
                    foreach (var ctsp in lstCTSP)
                    {
                        VatLieuModel myVl = lstVatLieuAo.FirstOrDefault(vl => vl.MaVL == ctsp.MaVL);
                        if (!myVl.IsNhap)
                        {
                            if ((myVl.SoLuongTon / ctsp.SoLuong) < max)
                                max = myVl.SoLuongTon / ctsp.SoLuong;
                        }
                    }
                    lstAo.Add(new SanPhamAo
                    {
                        HinhMoTa = sp.HinhMoTa,
                        MaSP = sp.MaSP,
                        TenSP = sp.TenSP,
                        AllNhap = false,
                        GiaTien = sp.GiaTien,
                        SoLuong = max
                    });

                }
            }

            return lstAo;
        }
    }
}
