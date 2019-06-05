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
    public class MockVatLieuRepository : WorkData<VatLieuModel>,IBaseRepository<VatLieuModel>
    {
        //HttpClient httpClient;
        private string _name = "VatLieu";
        private string _action;

        public async Task<VatLieuModel> GetById(string id)
        {
            List<VatLieuModel> lstVatLieu = await GetDataAsync();
            return lstVatLieu.FirstOrDefault(vl => vl.MaVL == id);
        }

        public async Task<List<VatLieuModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var listAnswer = JsonConvert.DeserializeObject<List<VatLieuModel>>(json);
                    return listAnswer;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<bool> SaveDataAsync(VatLieuModel obj, string name, bool isNew)
        {
            return await base.SaveDataAsync(obj, name, isNew);
        }

        public override async Task<bool> DeleteDataAsync(VatLieuModel obj, string name)
        {
            return await base.DeleteDataAsync(obj, name);
        }
    }
}
