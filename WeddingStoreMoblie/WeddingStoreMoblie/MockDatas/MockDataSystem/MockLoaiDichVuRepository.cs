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
    public class MockLoaiDichVuRepository : IBaseRepository<LoaiDichVuModel>
    {
        //HttpClient httpClient;
        private string _name = "LoaiDichVu";
        private string _action;

        public async Task<LoaiDichVuModel> GetById(string id)
        {
            List<LoaiDichVuModel> lstLDV = await GetDataAsync();
            return lstLDV.FirstOrDefault(ldv => ldv.MaLoaiDV == id);
        }

        public async Task<List<LoaiDichVuModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstLoaiDichVu = JsonConvert.DeserializeObject<List<LoaiDichVuModel>>(json);
                    return lstLoaiDichVu;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
