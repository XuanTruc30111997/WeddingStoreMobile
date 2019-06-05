using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Converters
{
    public class ConvertDateTimeToString
    {
        public static string ConverToMyDateFormat(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : String.Empty;
        }
    }
}
