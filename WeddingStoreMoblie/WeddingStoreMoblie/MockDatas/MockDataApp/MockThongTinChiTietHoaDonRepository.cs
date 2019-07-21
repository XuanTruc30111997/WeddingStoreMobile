using System;
using System.Collections.Generic;
using System.Text;
using WeddingStoreMoblie.Interfaces;
using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.AppModels;
using System.Threading.Tasks;

namespace WeddingStoreMoblie.MockDatas.MockDataApp
{
    public class MockThongTinChiTietHoaDonRepository
    {
        private List<ChiTietHoaDonModel> _lstChiTietHD; //danh sach chi tiet hoa don
        private List<ThongTinChiTietVatLieu> _lstThongTinChiTietVL; // Danh sách thông tin chi tiết vật liệu có trong mẫu
        private List<ThongTinChiTietHoaDon> _lstThongTinChiTietHD; // Danh sách thông tin chi tiết của hóa đơn
        public MockThongTinChiTietHoaDonRepository()
        {

        }
        public async Task<List<ThongTinChiTietHoaDon>> GetThongTinChiTietHoaDon(string maHD)
        {
            await GetThongTin(maHD).ConfigureAwait(false);
            return _lstThongTinChiTietHD;
        }

        private MockChiTietHoaDonRepository _chiTietHoaDon;
        private MockChiTietSanPhamRepository _chiTietMau;
        private MockVatLieuRepository _vatLieu;
        private MockSanPhamRepository _mauTrangTri;
        async Task GetThongTin(string maHD)
        {
            _chiTietHoaDon = new MockChiTietHoaDonRepository();
            _lstChiTietHD = await _chiTietHoaDon.GetByIdHD(maHD); // danh sách các mẫu có trong hóa đơn

            _chiTietMau = new MockChiTietSanPhamRepository();
            _lstThongTinChiTietHD = new List<ThongTinChiTietHoaDon>();

            List<ChiTietSanPhamModel> lstChiTietMau = new List<ChiTietSanPhamModel>();
            foreach (var cthd in _lstChiTietHD)
            {
                lstChiTietMau = await _chiTietMau.GetByIdSP(cthd.MaSP);
                _lstThongTinChiTietVL = new List<ThongTinChiTietVatLieu>();
                _vatLieu = new MockVatLieuRepository();
                VatLieuModel myVL = new VatLieuModel();
                foreach (var ctm in lstChiTietMau)
                {
                    myVL = await _vatLieu.GetById(ctm.MaVL);
                    ThongTinChiTietVatLieu thongTinVatLieu = new ThongTinChiTietVatLieu
                                            (myVL.AnhMoTa, myVL.TenVL, ctm.SoLuong * cthd.SoLuong, myVL.DonVi, myVL.GiaTien * ctm.SoLuong * cthd.SoLuong);
                    _lstThongTinChiTietVL.Add(thongTinVatLieu);
                }
                _mauTrangTri = new MockSanPhamRepository();
                SanPhamModel myMauTrangTri = await _mauTrangTri.GetById(cthd.MaSP);
                ThongTinChiTietHoaDon thongTinChiTietHD = new ThongTinChiTietHoaDon
                            (myMauTrangTri.HinhMoTa, cthd.MaSP, myMauTrangTri.TenSP, cthd.SoLuong, myMauTrangTri.GiaTien * cthd.SoLuong, _lstThongTinChiTietVL);
                _lstThongTinChiTietHD.Add(thongTinChiTietHD);
            }
            //await Task.Run(() =>
            //{
            //    Parallel.ForEach(_lstChiTietHD, async (cthd) =>
            //    {
            //        lstChiTietMau = await _chiTietMau.GetByIdSP(cthd.MaSP);
            //        _lstThongTinChiTietVL = new List<ThongTinChiTietVatLieu>();
            //        _vatLieu = new MockVatLieuRepository();
            //        VatLieuModel myVL = new VatLieuModel();
            //        foreach (var ctm in lstChiTietMau)
            //        {
            //            myVL = await _vatLieu.GetById(ctm.MaVL);
            //            ThongTinChiTietVatLieu thongTinVatLieu = new ThongTinChiTietVatLieu
            //                                    (myVL.AnhMoTa, myVL.TenVL, ctm.SoLuong * cthd.SoLuong, myVL.DonVi, myVL.GiaTien * ctm.SoLuong * cthd.SoLuong);
            //            _lstThongTinChiTietVL.Add(thongTinVatLieu);
            //        }
            //        _mauTrangTri = new MockSanPhamRepository();
            //        SanPhamModel myMauTrangTri = await _mauTrangTri.GetById(cthd.MaSP);
            //        ThongTinChiTietHoaDon thongTinChiTietHD = new ThongTinChiTietHoaDon
            //                    (myMauTrangTri.HinhMoTa, cthd.MaSP, myMauTrangTri.TenSP, cthd.SoLuong, myMauTrangTri.GiaTien * cthd.SoLuong, _lstThongTinChiTietVL);
            //        _lstThongTinChiTietHD.Add(thongTinChiTietHD);
            //        Console.WriteLine("Done: " + cthd.MaSP);
            //    });
            //});
        }

        public async Task<List<ThongTinPhatSinh>> GetThongTinPhatSinhByIdHD(string maHD)
        {
            MockPhatSinhRepository _phatSinh = new MockPhatSinhRepository();
            List<PhatSinhModel> lstPhatSinh = await _phatSinh.GetByIdHD(maHD);
            List<ThongTinPhatSinh> lstThongTinPhatSinh = new List<ThongTinPhatSinh>();
            _vatLieu = new MockVatLieuRepository();
            //VatLieuModel myVL = new VatLieuModel();
            foreach (var phatSinh in lstPhatSinh)
            {
                VatLieuModel myVL = await _vatLieu.GetById(phatSinh.MaVL);
                lstThongTinPhatSinh.Add(new ThongTinPhatSinh(myVL.AnhMoTa, myVL.MaVL, myVL.TenVL, myVL.IsNhap, myVL.DonVi, phatSinh.SoLuong, phatSinh.ThanhTien));
            }
            return lstThongTinPhatSinh;
        }

        public List<ThongTinPhatSinh> TestPhatSinh(string maHD)
        {
            //IPhatSinhRepository _phatSinh = new MockPhatSinhRepository();
            //List<PhatSinhModel> lstPhatSinh = _phatSinh.TestPhatSinh(maHD);
            //List<ThongTinPhatSinh> lstThongTinPhatSinh = new List<ThongTinPhatSinh>();
            //_vatLieu = new MockVatLieuRepository();
            ////VatLieuModel myVL = new VatLieuModel();
            //foreach (var phatSinh in lstPhatSinh)
            //{
            //    VatLieuModel myVL = _vatLieu.GetById(phatSinh.MaVL);
            //    lstThongTinPhatSinh.Add(new ThongTinPhatSinh(myVL.AnhMoTa, myVL.MaVL, myVL.TenVL, myVL.NhaCungCap, myVL.DonVi, phatSinh.SoLuong, phatSinh.ThanhTien));
            //}
            //return lstThongTinPhatSinh;
            return null;
        }
    }
}
