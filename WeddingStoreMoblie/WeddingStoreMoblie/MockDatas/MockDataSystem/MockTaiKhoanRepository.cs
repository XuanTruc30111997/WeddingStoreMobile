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
    class MockTaiKhoanRepository : WorkData<TaiKhoanModel>,IBaseRepository<TaiKhoanModel>
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
                    string json = await client.GetStringAsync(Constant.RestApiWeddingStore + _name + _action);
                    var lstTaiKhoan = JsonConvert.DeserializeObject<List<TaiKhoanModel>>(json);
                    return lstTaiKhoan;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error --> " + ex.Message);
            }
            return null;
        }

        public async Task<TaiKhoanModel> GetByIdNhanVien(string maNV)
        {
            List<TaiKhoanModel> lstTaiKhoan = await GetDataAsync();
            return lstTaiKhoan.FirstOrDefault(tk => tk.MaNV == maNV);
        }

        public override async Task<bool> SaveDataAsync(TaiKhoanModel obj, string name, bool isNew)
        {
            return await base.SaveDataAsync(obj, name, isNew);
        }
    }
}
