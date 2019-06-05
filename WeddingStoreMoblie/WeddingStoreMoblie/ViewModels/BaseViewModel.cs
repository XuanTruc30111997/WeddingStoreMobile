using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WeddingStoreMoblie.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region properties
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private bool _isChanged;
        public bool isChanged
        {
            get { return _isChanged; }
            set
            {
                _isChanged = value;
                OnPropertyChanged();
            }
        }

        private bool _isBusy { get; set; }
        public bool isBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private bool _isFirst { get; set; }
        public bool isFirst
        {
            get => _isFirst;
            set
            {
                _isFirst = value;
                OnPropertyChanged();
            }
        }

        #endregion

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        protected Page GetCurrentPage()
        {
            var _currentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
            //var _currentPage = Application.Current.MainPage;
            return _currentPage;
        }
    }
}
