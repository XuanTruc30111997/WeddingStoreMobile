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
    public class MockPhatSinhRepository : WorkData<PhatSinhModel>, IBaseRepository<PhatSinhModel>
    {
        private string _name = "PhatSinh";
        private string _action;

        public override Task<bool> DeleteDataAsync(PhatSinhModel obj,string name)
        {
            return base.DeleteDataAsync(obj,name);
        }

        public override Task<bool> SaveDataAsync(PhatSinhModel obj, string name, bool isNew)
        {
            return base.SaveDataAsync(obj, name, isNew);
        }

        //public async Task<bool> DeleteDataAsync(PhatSinhModel obj)
        //{
        //    _action = "/Delete";
        //    try
        //    {

        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.MaxResponseContentBufferSize = 256000;
        //            var json = JsonConvert.SerializeObject(obj);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            HttpResponseMessage response = null;
        //            response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, Constant.RestApiWeddingStore + _name + _action)
        //            {
        //                Content = content
        //            });
        //            if (response.IsSuccessStatusCode)
        //                return true;
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public Task<PhatSinhModel> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PhatSinhModel>> GetByIdHD(string maHD)
        {
            List<PhatSinhModel> lstPhatSinh = await GetDataAsync();
            return lstPhatSinh.Where(ps => ps.MaHD == maHD).ToList();
        }

        public async Task<List<PhatSinhModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstPhatSinh = JsonConvert.DeserializeObject<List<PhatSinhModel>>(json);
                    return lstPhatSinh;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
