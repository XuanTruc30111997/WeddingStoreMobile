using System;
using System.Collections.Generic;
using System.Text;

namespace WeddingStoreMoblie.Controls
{
    public class RadioButtonProperty : ViewModels.BaseViewModel
    {
        public string Name { get; set; }

        bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                SetProperty(ref _isVisible, value);
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }
    }
}
