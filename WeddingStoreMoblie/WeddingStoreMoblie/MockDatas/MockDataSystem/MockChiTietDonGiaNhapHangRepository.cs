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
    public class MockChiTietDonGiaNhapHangRepository : IBaseRepository<ChiTietDonGiaNhapHangModel>
    {
        private string _name = "ChiTietDonGiaNhapHang";
        private string _action;

        public Task<ChiTietDonGiaNhapHangModel> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChiTietDonGiaNhapHangModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstChiTiet = JsonConvert.DeserializeObject<List<ChiTietDonGiaNhapHangModel>>(json);
                    return lstChiTiet;
                }
                //string json = await MockDataBase.Ins.httpClient.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                //var lstChiTiet = JsonConvert.DeserializeObject<List<ChiTietDonGiaNhapHangModel>>(json);
                //return lstChiTiet;
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
