using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.MarkupExtensions
{
    [ContentProperty("ReourceId")]
    public class EmbeddedImage : IMarkupExtension
    {
        public string ReourceId { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!String.IsNullOrWhiteSpace(ReourceId))
                return ImageSource.FromResource("WeddingStoreMoblie.Images." + ReourceId);
            return null;
        }
    }
}
