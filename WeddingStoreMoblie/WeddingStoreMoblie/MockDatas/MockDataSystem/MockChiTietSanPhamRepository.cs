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
    public class MockChiTietSanPhamRepository : IBaseRepository<ChiTietSanPhamModel>
    {
        private string _name = "ChiTietSanPham";
        private string _action;

        public Task<ChiTietSanPhamModel> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChiTietSanPhamModel>> GetByIdSP(string maSP)
        {
            List<ChiTietSanPhamModel> lstChiTiet = await GetDataAsync();
            return lstChiTiet.Where(ct => ct.MaSP == maSP).ToList();
        }

        public async Task<List<ChiTietSanPhamModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstChiTiet = JsonConvert.DeserializeObject<List<ChiTietSanPhamModel>>(json);
                    return lstChiTiet;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

    }
}
