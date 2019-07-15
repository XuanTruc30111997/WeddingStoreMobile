using System;
using System.Collections.Generic;
using System.Text;

using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.Interfaces;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using WeddingStoreMoblie.MockDatas.MockDataSystem;

namespace WeddingStoreMoblie.MockDatas.MockDataSystem
{
    public class MockHoaDonRepository : WorkData<HoaDonModel>, IBaseRepository<HoaDonModel>
    {
        private string _name = "HoaDon";
        private string _action;

        public async Task<HoaDonModel> GetById(string id)
        {
            List<HoaDonModel> lstHoaDon = await GetDataAsync();
            HoaDonModel myHD = lstHoaDon.FirstOrDefault(hd => hd.MaHD == id);
            return myHD;
        }

        public async Task<List<HoaDonModel>> GetLstHoaDonByNgay(string maHD)
        {
            var thTH = new System.Globalization.CultureInfo("th-TH");
            HoaDonModel myHD = await GetById(maHD);

            List<HoaDonModel> lstHoaDon = await GetDataAsync();

            List<HoaDonModel> myLst = lstHoaDon.Where(hd
                => (hd.NgayTrangTri - myHD.NgayThaoDo).TotalDays > 3
                || (myHD.NgayTrangTri - hd.NgayThaoDo).TotalDays > 3).ToList();
            return myLst;
        }
        public async Task<List<HoaDonModel>> GetLstHOaDonByThangNam(int thang, int nam, bool type)
        {
            List<HoaDonModel> lstHoaDon = await GetDataAsync();
            if (type) // trang trí
                return lstHoaDon.Where(hd => hd.NgayTrangTri.Month == thang && hd.NgayTrangTri.Year == nam).ToList();
            return lstHoaDon.Where(hd => hd.NgayThaoDo.Month == thang && hd.NgayThaoDo.Year == nam).ToList();
        }

        public async Task<List<HoaDonModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstHoaDon = JsonConvert.DeserializeObject<List<HoaDonModel>>(json);
                    return lstHoaDon;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<bool> SaveDataAsync(HoaDonModel obj, string name, bool isNew)
        {
            return await base.SaveDataAsync(obj, name, isNew);
        }

        public async Task<List<HoaDonModel>> GetHoaDonPhuHop(string maHD)
        {
            List<HoaDonModel> lstHoaDon = await GetDataAsync();
            return lstHoaDon.Where(hd => (hd.TinhTrang == 0 && hd.MaHD != maHD)
                                        || (hd.TinhTrang != 1 && hd.MaHD == maHD)
                                        || (hd.TinhTrang == 1 && hd.MaHD != maHD)).ToList();
        }
    }
}
