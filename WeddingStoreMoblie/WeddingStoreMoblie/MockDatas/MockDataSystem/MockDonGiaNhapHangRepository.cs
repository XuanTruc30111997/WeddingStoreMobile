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
    public class MockDonGiaNhapHangRepository : IBaseRepository<DonGiaNhapHangModel>
    {
        private string _name = "DonGiaNhapHang";
        private string _action;

        public async Task<DonGiaNhapHangModel> GetById(string id)
        {
            List<DonGiaNhapHangModel> lstDG = await GetDataAsync();
            return lstDG.FirstOrDefault(dg => dg.MaDG == id);
        }

        public async Task<List<DonGiaNhapHangModel>> GetByIdNCC(string maNCC)
        {
            List<DonGiaNhapHangModel> lstDG = await GetDataAsync();
            return lstDG.Where(dg => dg.MaNCC == maNCC).ToList();
        }

        public async Task<List<DonGiaNhapHangModel>> GetDataAsync()
        {
            _action = "/Get";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstDonGia = JsonConvert.DeserializeObject<List<DonGiaNhapHangModel>>(json);
                    return lstDonGia;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
