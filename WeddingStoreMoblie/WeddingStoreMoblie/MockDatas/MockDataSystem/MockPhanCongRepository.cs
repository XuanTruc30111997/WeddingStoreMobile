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
    public class MockPhanCongRepository : WorkData<PhanCongModel>,IBaseRepository<PhanCongModel>
    {
        private string _name = "PhanCong";
        private string _action;
        public Task<PhanCongModel> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PhanCongModel>> GetByIdNV(string maNV)
        {
            List<PhanCongModel> lstPhanCong = await GetDataAsync();
            return lstPhanCong.Where(pc => pc.MaNV == maNV).ToList();
        }

        public async Task<List<PhanCongModel>> GetByIdNVThangNam(string maNV, int thang, int nam)
        {
            List<PhanCongModel> lstPhanCong = await GetDataAsync();
            return lstPhanCong.Where(pc => pc.MaNV == maNV
                                        && pc.Ngay.Month == thang
                                        && pc.Ngay.Year == nam).ToList();
        }

        public async Task<List<PhanCongModel>> GetByIdHdVaNgay(string maHD, DateTime ngay)
        {
            List<PhanCongModel> lstPhanCong = await GetDataAsync();
            return lstPhanCong.Where(pc => pc.MaHD == maHD && pc.Ngay == ngay).ToList();
        }

        public async Task<List<PhanCongModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstPhanCong = JsonConvert.DeserializeObject<List<PhanCongModel>>(json);
                    return lstPhanCong;
                }
                //string json = await MockDataBase.Ins.httpClient.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                //var lstPhanCong = JsonConvert.DeserializeObject<List<PhanCongModel>>(json);
                //return lstPhanCong;
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<bool> SaveDataAsync(PhanCongModel obj, string name, bool isNew)
        {
            return await base.SaveDataAsync(obj, name, isNew);
        }

        public override async Task<bool> DeleteDataAsync(PhanCongModel obj, string name)
        {
            return await base.DeleteDataAsync(obj, name);
        }

        public async Task<PhanCongModel> GetByIdNVNgay(string maNV,DateTime ngay)
        {
            List<PhanCongModel> lstPhanCong = await GetDataAsync();
            return lstPhanCong.FirstOrDefault(pc => pc.MaNV == maNV && pc.Ngay == ngay);
        }
    }
}
