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
    public class MockSanPhamRepository : IBaseRepository<SanPhamModel>
    {
        //HttpClient httpClient;
        private string _name = "SanPham";
        private string _action;

        public async Task<SanPhamModel> GetById(string id)
        {
            List<SanPhamModel> lstSanPham = await GetDataAsync();
            return lstSanPham.FirstOrDefault(sp => sp.MaSP == id);
        }

        public async Task<List<SanPhamModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstSanPham = JsonConvert.DeserializeObject<List<SanPhamModel>>(json);
                    return lstSanPham;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
