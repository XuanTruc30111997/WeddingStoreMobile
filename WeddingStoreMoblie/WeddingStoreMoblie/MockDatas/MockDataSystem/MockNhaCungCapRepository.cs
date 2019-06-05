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
    public class MockNhaCungCapRepository : IBaseRepository<NhaCungCapModel>
    {
        //HttpClient httpClient;
        private string _name = "LoaiDichVu";
        private string _action;

        public async Task<NhaCungCapModel> GetById(string id)
        {
            List<NhaCungCapModel> lstNhaCungCap = await GetDataAsync();
            return lstNhaCungCap.FirstOrDefault(ncc => ncc.MaNCC == id);
        }

        public async Task<List<NhaCungCapModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstNhaCungCap = JsonConvert.DeserializeObject<List<NhaCungCapModel>>(json);
                    return lstNhaCungCap;
                }
            }
            catch(Exception)
            {

            }
            return null;
        }
    }
}
