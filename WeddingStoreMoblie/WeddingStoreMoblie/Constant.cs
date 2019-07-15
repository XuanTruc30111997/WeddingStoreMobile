using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie
{
    public class Constant
    {
        //public const string RestApiWeddingStore = "http://192.168.43.44:1997/api/";
        public const string RestApiWeddingStore = "http://192.168.1.217:1997/api/";
        //public const string RestApiWeddingStore = "http://192.168.178.2:3011/api/";
        //public const string RestApiWeddingStore = "http://192.168.1.218:1997/api/";
        public static bool isNew { get; set; }
        public static bool isNewTinhTrang { get; set; }
        public static bool isNewPS { get; set; }
        public static bool isNewMau { get; set; }
        public static bool isNewDanhSachVatLieu { get; set; }
        public const string ImagePatch = "WeddingStoreMoblie.Images.";
    }
}
