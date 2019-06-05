using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeddingStoreMoblie.Models.SystemModels;

namespace WeddingStoreMoblie.MockDatas.MockDataSystem
{
    public abstract class WorkData<T>
    {
        public virtual async Task<bool> DeleteDataAsync(T obj, string name)
        {
            string _action = "/Delete";
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, Constant.RestApiWeddingStore + name + _action)
                    {
                        Content = content
                    });
                    if (response.IsSuccessStatusCode)
                        return true;
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> SaveDataAsync(T obj, string name, bool isNew)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    if (isNew)
                    {
                        response = await client.PostAsync(Constant.RestApiWeddingStore + name + "/Post", content);
                    }
                    else
                    {
                        response = await client.PutAsync(Constant.RestApiWeddingStore + name + "/Put", content);
                    }
                    if (response.IsSuccessStatusCode)
                        return true;
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
