using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WeddingStoreMoblie.Triggers
{
    public class ExpandButtonTriggerAction : TriggerAction<Button>
    {
        protected override async void Invoke(Button myBtn)
        {
            await myBtn.ScaleTo(0.95, 50, Easing.CubicOut);
            await myBtn.ScaleTo(1, 50, Easing.CubicIn);
        }
    }
}
