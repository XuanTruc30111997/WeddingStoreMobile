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
    public class MockKhachHangRepository : IBaseRepository<KhachHangModel>
    {
        private string _name = "KhachHang";
        private string _action;

        public async Task<KhachHangModel> GetById(string id)
        {
            List<KhachHangModel> lstKhachHang = await GetDataAsync();
            return lstKhachHang.FirstOrDefault(kh => kh.MaKH == id);
        }

        public async Task<List<KhachHangModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstKhachHang = JsonConvert.DeserializeObject<List<KhachHangModel>>(json);
                    return lstKhachHang;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
