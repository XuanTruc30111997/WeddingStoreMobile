using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeddingStoreMoblie.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetDataAsync();
        Task<T> GetById(string id);
    }
}
