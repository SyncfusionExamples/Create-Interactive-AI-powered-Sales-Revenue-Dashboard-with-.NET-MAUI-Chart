using System.ComponentModel;

namespace SalesDashboard
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Properties

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        private DateRange _selectedDateRange = DateRange.Last30Days;
        public DateRange SelectedDateRange
        {
            get => _selectedDateRange;
            set
            {
                if (_selectedDateRange != value)
                {
                    _selectedDateRange = value;
                    OnPropertyChanged(nameof(SelectedDateRange));
                }
            }
        }

        #endregion

        public BaseViewModel()
        {
            IsBusy = true;
        }

        #region Methods

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 

        #endregion
    }
}