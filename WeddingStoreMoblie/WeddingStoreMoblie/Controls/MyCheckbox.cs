using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WeddingStoreMoblie.Controls
{
    class MyCheckbox : ImageButton
    {
        public MyCheckbox()
        {
            base.Source = ImageSource.FromResource("WeddingStoreMoblie.Images.unchecked.png");
            base.Clicked += new EventHandler(OnClicked);
            base.BackgroundColor = Color.Transparent;
            base.BorderWidth = 0;
            base.WidthRequest = 35;
            base.HeightRequest = 30;
        }

        public static BindableProperty CheckedProperty = BindableProperty.Create(
            propertyName: "Checked",
            returnType: typeof(Boolean?),
            declaringType: typeof(MyCheckbox),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: CheckedValueChanged);

        public Boolean? Checked
        {
            get
            {
                if (GetValue(CheckedProperty) == null)
                {
                    return null;
                }
                return (Boolean)GetValue(CheckedProperty);
            }
            set
            {
                SetValue(CheckedProperty, value);
                OnPropertyChanged();
                RaiseCheckedChanged();
            }
        }

        private static void CheckedValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null && (Boolean)newValue == true)
                ((MyCheckbox)bindable).Source = ImageSource.FromResource("WeddingStoreMoblie.Images.checked.png");
            else
                ((MyCheckbox)bindable).Source = ImageSource.FromResource("WeddingStoreMoblie.Images.unchecked.png");
        }

        public event EventHandler CheckedChanged;
        private void RaiseCheckedChanged()
        {
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }


        public void OnClicked(object sender, EventArgs e)
        {
            Checked = !Checked;
        }
    }
}
