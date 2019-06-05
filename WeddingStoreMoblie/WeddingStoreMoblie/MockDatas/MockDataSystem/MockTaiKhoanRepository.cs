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
    class MockTaiKhoanRepository : IBaseRepository<TaiKhoanModel>
    {
        private string _name = "TaiKhoan";
        private string _action;
        public async Task<TaiKhoanModel> GetById(string id)
        {
            List<TaiKhoanModel> lstTaiKhoan = await GetDataAsync();
            return lstTaiKhoan.FirstOrDefault(tk => tk.ID == id);
        }

        public async Task<List<TaiKhoanModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstTaiKhoan = JsonConvert.DeserializeObject<List<TaiKhoanModel>>(json);
                    return lstTaiKhoan;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
