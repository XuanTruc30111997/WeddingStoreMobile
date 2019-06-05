using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
using System.Net.Http;

namespace WeddingStoreMoblie.MockDatas.MockDataSystem
{
    public class MockDataBase
    {
        private static MockDataBase _ins;
        public static MockDataBase Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new MockDataBase();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public HttpClient httpClient { get; set; }

        private MockDataBase()
        {
            httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
        }
    }
}
