using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;

namespace WeddingStoreMoblie.Functions
{
    public class GetThongTin
    {
        public async static Task<List<ThongTinMau>> getThongTinMau(string maHD)
        {
            MockChiTietHoaDonRepository chiTietHd = new MockChiTietHoaDonRepository();
            List<ChiTietHoaDonModel> lstCTHD = await chiTietHd.GetByIdHD(maHD);

            MockSanPhamRepository _iMau = new MockSanPhamRepository();
            List<SanPhamModel> lstMau = await _iMau.GetDataAsync();

            List<ThongTinMau> myLst = new List<ThongTinMau>();
            SanPhamModel myMau = new SanPhamModel();
            foreach (var ct in lstCTHD)
            {
                myMau = lstMau.Where(mau => mau.MaSP == ct.MaSP).ToList().First();
                myLst.Add(new ThongTinMau { Hinh = myMau.HinhMoTa, MaSP = myMau.MaSP, TenSP = myMau.TenSP, SoLuong = ct.SoLuong, ThanhTien = ct.ThanhTien});
            }
            return myLst;
        }

        public async static Task<List<VatLieuModel>> getLstVatLieu(string maHD)
        {
            List<VatLieuModel> myLst = new List<VatLieuModel>();

            MockHoaDonRepository hoaDon = new MockHoaDonRepository();
            // Danh sách tất cả hóa đơn có ngày trang trí > 3 ngày so với ngày tháo dở của hóa đơn
            // Danh sách tất cả hóa đơn có ngày tháo dở < 3 ngày so với ngày trang trí của hóa đơn
            List<HoaDonModel> lstHoaDon = await hoaDon.GetLstHoaDonByNgay(maHD);

            MockChiTietHoaDonRepository chiTietHoaDon = new MockChiTietHoaDonRepository();
            List<ChiTietHoaDonModel> lstChiTietHoaDon = new List<ChiTietHoaDonModel>();

            MockChiTietSanPhamRepository chiTietMau = new MockChiTietSanPhamRepository();
            MockVatLieuRepository vatLieu = new MockVatLieuRepository();

            foreach (var hd in lstHoaDon)
            {
                // Danh sách chi tiết mẫu hóa đơn
                lstChiTietHoaDon = await chiTietHoaDon.GetByIdHD(hd.MaHD);
                foreach (var cthd in lstChiTietHoaDon)
                {
                    List<ChiTietSanPhamModel> lstChiTietMau = await chiTietMau.GetByIdSP(cthd.MaSP);
                    foreach (var ctm in lstChiTietMau)
                    {
                        VatLieuModel myVL = await vatLieu.GetById(ctm.MaVL); // vật liệu có trong mẫu

                        if (myLst.Count == 0) // danh sách vật liệu chưa có
                        {
                            myVL.SoLuongTon = cthd.SoLuong * ctm.SoLuong;
                            myLst.Add(myVL); // thêm vật liệu vào danh sách
                        }
                        else
                        {
                            bool isEsist = false;
                            foreach (var vl in myLst)
                            {
                                if (myVL.MaVL == vl.MaVL) // vật liệu đã có trong danh sách
                                {
                                    vl.SoLuongTon += cthd.SoLuong * ctm.SoLuong;
                                    isEsist = true;
                                    break;
                                }
                            }
                            if (!isEsist) // vật liệu chưa có trong danh sách
                            {
                                myVL.SoLuongTon = cthd.SoLuong * ctm.SoLuong;
                                myLst.Add(myVL); // thêm vật liệu vào danh sách
                            }
                        }
                    }
                }
            }
            vatLieu = new MockVatLieuRepository();
            List<VatLieuModel> lstVatLieu = await vatLieu.GetDataAsync();
            foreach (var vlKho in lstVatLieu)
            {
                foreach (var vl in myLst)
                {
                    if (vl.MaVL == vlKho.MaVL)
                    {
                        vlKho.SoLuongTon += vl.SoLuongTon;
                        break;
                    }
                }
            }
            return lstVatLieu;
        }

        public async static Task<List<SanPhamModel>> getLstMau(string maHD)
        {
            List<SanPhamModel> myLst = new List<SanPhamModel>();

            MockSanPhamRepository mauTrangTri = new MockSanPhamRepository();
            MockChiTietSanPhamRepository chiTietMau = new MockChiTietSanPhamRepository();

            List<SanPhamModel> lstMauTrangTri = await mauTrangTri.GetDataAsync();
            List<VatLieuModel> lstVatLieu = await getLstVatLieu(maHD);

            foreach (var mtt in lstMauTrangTri)
            {
                bool isEnough = true;
                List<ChiTietSanPhamModel> lstChiTietMau = await chiTietMau.GetByIdSP(mtt.MaSP);
                foreach (var ctMau in lstChiTietMau)
                {
                    foreach (var vl in lstVatLieu)
                    {
                        if (ctMau.MaVL == vl.MaVL && vl.SoLuongTon < ctMau.SoLuong)
                        {
                            isEnough = false;
                            break;
                        }
                    }
                    if (!isEnough)
                        break;
                }
                if (isEnough)
                    myLst.Add(mtt);
            }

            return myLst;
        }
    }
}
