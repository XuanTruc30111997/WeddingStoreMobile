using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using WeddingStoreMoblie.Models.AppModels;
using WeddingStoreMoblie.Models.SystemModels;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class DanhSachVatLieuViewModel : BaseViewModel
    {
        #region Properties
        private string _maHD;
        private List<DanhSachVatLieu> _LstDanhSachVatLieu { get; set; }
        public List<DanhSachVatLieu> LstDanhSachVatLieu
        {
            get => _LstDanhSachVatLieu;
            set
            {
                _LstDanhSachVatLieu = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Services
        MockVatLieuRepository vatLieuMock = new MockVatLieuRepository();
        MockPhatSinhRepository phatSinhMock = new MockPhatSinhRepository();
        MockChiTietHoaDonRepository chiTietHDMock = new MockChiTietHoaDonRepository();
        MockChiTietSanPhamRepository chiTietSPMock = new MockChiTietSanPhamRepository();
        #endregion

        #region Constructors
        public DanhSachVatLieuViewModel(string maHD)
        {
            _maHD = maHD;
            Constant.isNewDanhSachVatLieu = true;
        }
        #endregion

        #region Commands
        public Command RefreshCommand
        {
            get => new Command(async () =>
              {
                  await GetDataAsync();
              });
        }
        #endregion

        #region Methods
        public async Task GetDataAsync()
        {
            Device.BeginInvokeOnMainThread(() => { isBusy = true; });
            LstDanhSachVatLieu = await GetVatLieu();
            Device.BeginInvokeOnMainThread(() => { isBusy = false; });
        }

        async Task<List<DanhSachVatLieu>> GetVatLieu()
        {
            List<VatLieuModel> lstVatLieu = new List<VatLieuModel>();
            List<List<DanhSachVatLieu>> lstCT = new List<List<DanhSachVatLieu>>();
            var t1 = Task.Run(async () =>
            {
                lstVatLieu = await vatLieuMock.GetDataAsync();
                Console.WriteLine("Done For VatLieu in DanhSachVatLieu");
            });

            List<ChiTietHoaDonModel> lstChiTietHoaDon = new List<ChiTietHoaDonModel>();
            var t2 = Task.Run(async () =>
            {
                lstChiTietHoaDon = await chiTietHDMock.GetByIdHD(_maHD);
                Console.WriteLine("Done For Chi Tiet HoaDon in DanhSachVatLieu");
            });

            List<PhatSinhModel> lstPhatSinh = new List<PhatSinhModel>();
            var t3 = Task.Run(async () =>
            {
                lstPhatSinh = await GetPhatSinh();
                Console.WriteLine("Done For Phat Sinh in DanhSachVatLieu");
            });

            List<DanhSachVatLieu> myLst = new List<DanhSachVatLieu>();
            await Task.WhenAll(t1, t2,t3);

            foreach (var cthd in lstChiTietHoaDon)
            {
                List<ChiTietSanPhamModel> lstChiTietSP = new List<ChiTietSanPhamModel>();
                lstChiTietSP = await chiTietSPMock.GetByIdSP(cthd.MaSP);
                lstCT.Add(GetVatLieuChiTiet(cthd.SoLuong, lstChiTietSP, lstVatLieu));
            }

            foreach (var vlCT in lstCT)
            {
                foreach (var vl in vlCT)
                {
                    bool isExist = false;
                    foreach (var myVl in myLst)
                    {
                        if (myVl.MaVL == vl.MaVL)
                        {
                            myVl.SoLuong += vl.SoLuong;
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        myLst.Add(vl);
                    }
                }
            }
            //var t4 = Task.Run(() =>
            //  {
            //      Parallel.ForEach(lstChiTietHoaDon, async (cthd) =>
            //      {
            //          List<ChiTietSanPhamModel> lstChiTietSP = await chiTietSPMock.GetByIdSP(cthd.MaSP);
            //          await Task.Run( () =>
            //          {
            //              tasks.Add(GetVatLieuChiTiet(cthd.SoLuong, lstChiTietSP, lstVatLieu));
            //              Console.WriteLine("Done For Task in DanhSachVatLieu");
            //          });
            //      });
            //  }).ConfigureAwait(false);

            foreach (var ps in lstPhatSinh)
            {
                VatLieuModel myVL = lstVatLieu.FirstOrDefault(vl => vl.MaVL == ps.MaVL);
                bool isExist = false;
                foreach (var vl in myLst)
                {
                    if (ps.MaVL == vl.MaVL)
                    {
                        vl.SoLuong += ps.SoLuong;
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    myLst.Add(new DanhSachVatLieu
                    {
                        AnhMoTa = myVL.AnhMoTa,
                        MaVL = ps.MaVL,
                        TenVL = myVL.TenVL,
                        SoLuong = ps.SoLuong,
                        IsNhap = myVL.IsNhap
                    });
                }
            }
            Constant.isNewDanhSachVatLieu = false;
            return myLst;
        }

        List<DanhSachVatLieu> GetVatLieuChiTiet(int soLuong, List<ChiTietSanPhamModel> ctsp, List<VatLieuModel> lstVatLieu)
        {
            List<DanhSachVatLieu> myLst = new List<DanhSachVatLieu>();
            foreach (var my in ctsp)
            {
                VatLieuModel myVL = lstVatLieu.FirstOrDefault(vl => vl.MaVL == my.MaVL);
                myLst.Add(new DanhSachVatLieu
                {
                    AnhMoTa = myVL.AnhMoTa,
                    MaVL = my.MaVL,
                    TenVL = myVL.TenVL,
                    SoLuong = my.SoLuong * soLuong,
                    IsNhap = myVL.IsNhap
                });
            }
            return myLst;
        }

        async Task<List<PhatSinhModel>> GetPhatSinh()
        {
            List<PhatSinhModel> lstPhatSinh = new List<PhatSinhModel>();
            lstPhatSinh = await phatSinhMock.GetByIdHD(_maHD);
            return lstPhatSinh;
        }
        #endregion
    }
}
