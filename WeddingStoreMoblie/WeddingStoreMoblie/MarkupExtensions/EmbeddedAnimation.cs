using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeddingStoreMoblie.MarkupExtensions
{
    [ContentProperty("ReourceId")]
    public class EmbeddedAnimation : IMarkupExtension
    {
        public string ReourceId { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!String.IsNullOrWhiteSpace(ReourceId))
                return ImageSource.FromResource("WeddingStoreMoblie.Animations." + ReourceId);
            return null;
        }
    }
}
