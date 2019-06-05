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
    public class MockDichVuRepository : IBaseRepository<DichVuModel>
    {
        private string _name = "DichVu";
        private string _action;

        public async Task<DichVuModel> GetById(string id)
        {
            List<DichVuModel> lstDV = await GetDataAsync();
            return lstDV.FirstOrDefault(dv => dv.MaDV == id);
        }

        public async Task<List<DichVuModel>> GetByIdLDV(string maLDV)
        {
            List<DichVuModel> lstDichVu = await GetDataAsync();
            return lstDichVu.Where(dv => dv.LoaiDV == maLDV).ToList();
        }

        public async Task<List<DichVuModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstDichVu = JsonConvert.DeserializeObject<List<DichVuModel>>(json);
                    return lstDichVu;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
