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
    public class MockNhanVienRepository : IBaseRepository<NhanVienModel>
    {
        //HttpClient httpClient;
        private string _name = "NhanVien";
        private string _action;

        //public MockNhanVienRepository()
        //{
        //    httpClient = new HttpClient();
        //    httpClient.MaxResponseContentBufferSize = 256000;
        //}

        public async Task<NhanVienModel> GetById(string id)
        {
            List<NhanVienModel> lstNhanVien = await GetDataAsync();
            return lstNhanVien.FirstOrDefault(nv => nv.MaNV == id);
        }

        public async Task<List<NhanVienModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstNhanVien = JsonConvert.DeserializeObject<List<NhanVienModel>>(json);
                    return lstNhanVien;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex.Message);
            }
            return null;
        }
    }
}
