using System;
using System.Collections.Generic;
using System.Text;

using WeddingStoreMoblie.Models.SystemModels;
using WeddingStoreMoblie.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using WeddingStoreMoblie.MockDatas.MockDataSystem;
using System.Net.Http;

namespace WeddingStoreMoblie.MockDatas.MockDataSystem
{
    public class MockChiTietHoaDonRepository : WorkData<ChiTietHoaDonModel>,IBaseRepository<ChiTietHoaDonModel>
    {
        private string _name = "ChiTietHoaDon";
        private string _action;

        public Task<ChiTietHoaDonModel> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChiTietHoaDonModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstChiTiet = JsonConvert.DeserializeObject<List<ChiTietHoaDonModel>>(json);
                    return lstChiTiet;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public async Task<List<ChiTietHoaDonModel>> GetByIdHD(string maHD)
        {
            List<ChiTietHoaDonModel> lstChiTiet = await GetDataAsync();
            return lstChiTiet.Where(ct => ct.MaHD == maHD).ToList();
        }

        public override async Task<bool> DeleteDataAsync(ChiTietHoaDonModel obj, string name)
        {
            return await base.DeleteDataAsync(obj, name);
        }
        public override async Task<bool> SaveDataAsync(ChiTietHoaDonModel obj, string name, bool isNew)
        {
            return await base.SaveDataAsync(obj, name, isNew);
        }

        //public async Task<bool> SaveDataAsync(ChiTietHoaDonModel obj,bool isNew)
        //{
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.MaxResponseContentBufferSize = 256000;
        //            var json = JsonConvert.SerializeObject(obj);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            HttpResponseMessage response = null;
        //            if(isNew)
        //            {
        //                _action = "/Post";
        //                response = await client.PostAsync(Constant.RestApiWeddingStore + _name + _action, content);
        //            }
        //            else
        //            {
        //                _action = "/Put";
        //                response = await client.PutAsync(Constant.RestApiWeddingStore + _name + _action, content);
        //            }
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

        //public async Task<bool> DeleteDataAsync(ChiTietHoaDonModel obj)
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
        //    catch(Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}
